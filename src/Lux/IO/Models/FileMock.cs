using System;
using System.IO;
using System.Text;

namespace Lux.IO
{
    public class FileMock
    {
        private Lazy<byte[]> _bytes;
        private string _content;
        private Stream _stream;

        public FileMock(string path, Encoding encoding)
        {
            Path = path;
            Name = System.IO.Path.GetFileName(path);
            Encoding = encoding;

            _bytes = new Lazy<byte[]>(() => new byte[0]);
            _content = Encoding.GetString(_bytes.Value);
        }

        public FileMock(string path, byte[] bytes)
            : this(path, bytes, Lux.IO.Consts.DefaultEncoding)
        {

        }

        public FileMock(string path, byte[] bytes, Encoding encoding)
            : this(path, encoding)
        {
            _bytes = new Lazy<byte[]>(() => bytes ?? new byte[0]);
            _content = Encoding.GetString(_bytes.Value);
        }

        public FileMock(string path, Lazy<byte[]> bytes)
            : this(path, bytes, Lux.IO.Consts.DefaultEncoding)
        {

        }

        public FileMock(string path, Lazy<byte[]> bytes, Encoding encoding)
            : this(path, encoding)
        {
            _bytes = new Lazy<byte[]>(() =>
            {
                var value = bytes.Value ?? new byte[0];
                _content = Encoding.GetString(value);
                return value;
            });
        }


        public string Name { get; internal set; }

        public string Path { get; internal set; }

        public byte[] Bytes
        {
            get
            {
                var value = _bytes.Value;
                return value;
            }
        }

        public string Content
        {
            get { return _content; }
        }

        public Encoding Encoding { get; set; }

        public bool IsDeleted { get; protected internal set; }



        public virtual Stream GetStream()
        {
            if (_stream == null || !_stream.CanRead)
            {
                _stream = new NotifyableStream(onFlush: stream =>
                {
                    //_bytes = stream.ReadAllAsBytes() ?? new byte[0];
                    _bytes = new Lazy<byte[]>(() =>
                    {
                        var value = stream.ReadAllAsBytes() ?? new byte[0];
                        _content = Encoding.GetString(value);
                        return value;
                    });
                    //_content = Encoding.GetString(_bytes.Value);
                });
                if (_bytes.Value != null && _bytes.Value.Length > 0)
                {
                    _stream.Write(_bytes.Value, 0, _bytes.Value.Length);
                    _stream.Flush();

                    //using (var streamWriter = new StreamWriter(_stream, Encoding))
                    //{
                    //    streamWriter.Write(_content);
                    //}
                }
            }
            if (_stream.CanSeek)
                _stream.Position = 0;
            return _stream;
        }


        public override string ToString()
        {
            return Path;
            //return Name;
            //return Content;
        }




        public static FileMock Create(string path)
        {
            string content = null;
            var file = Create(path, content);
            return file;
        }

        public static FileMock Create(string path, string content)
        {
            var file = Create(path, content, Lux.IO.Consts.DefaultEncoding);
            return file;
        }

        public static FileMock Create(string path, string content, Encoding encoding)
        {
            var bytes = encoding.GetBytes(content ?? "");
            var file = Create(path, bytes, encoding);
            return file;
        }

        public static FileMock Create(string path, byte[] content)
        {
            var file = Create(path, content, Lux.IO.Consts.DefaultEncoding);
            return file;
        }

        public static FileMock Create(string path, byte[] content, Encoding encoding)
        {
            var file = new FileMock(path, content, encoding);
            return file;
        }

        public static FileMock Create(string path, Lazy<byte[]> content)
        {
            var file = Create(path, content, Lux.IO.Consts.DefaultEncoding);
            return file;
        }

        public static FileMock Create(string path, Lazy<byte[]> content, Encoding encoding)
        {
            var file = new FileMock(path, content, encoding);
            return file;
        }


        public static FileMock Copy(FileMock file, string targetPath)
        {
            var bytes = file.Bytes;
            var newFile = new FileMock(targetPath, bytes)
            {
                Encoding = file.Encoding,
            };
            return newFile;
        }

    }
}
