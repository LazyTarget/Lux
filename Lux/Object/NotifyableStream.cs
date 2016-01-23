using System;
using System.IO;

namespace Lux
{
    public class NotifyableStream : Stream
    {
        private readonly Stream _stream;
        private readonly Action<NotifyableStream> _onFlush;

        public NotifyableStream(Action<NotifyableStream> onFlush)
            : this(new MemoryStream(), onFlush)
        {
            
        }

        public NotifyableStream(Stream stream, Action<NotifyableStream> onFlush)
        {
            _stream = stream;
            _onFlush = onFlush;
        }


        public override void Flush()
        {
            _stream.Flush();
            if (_onFlush != null)
                _onFlush(this);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return _stream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            _stream.SetLength(value);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return _stream.Read(buffer, offset, count);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            _stream.Write(buffer, offset, count);
        }

        public override bool CanRead
        {
            get { return _stream.CanRead; }
        }

        public override bool CanSeek
        {
            get { return _stream.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return _stream.CanWrite; }
        }

        public override long Length
        {
            get { return _stream.Length; }
        }

        public override long Position
        {
            get { return _stream.Position; }
            set { _stream.Position = value; }
        }
    }
}
