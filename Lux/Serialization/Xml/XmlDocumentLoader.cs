using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace Lux.Serialization.Xml
{
    public class XmlDocumentLoader
    {
        public XmlDocumentLoader()
            : this(new Lux.Serialization.Xml.docum)
        {
            
        }

        public XmlDocumentLoader(IXmlDocument document)
        {
            Document = document;

            XmlReaderSettings = new XmlReaderSettings();
            XmlReaderSettings.DtdProcessing = DtdProcessing.Parse;

            XmlWriterSettings = new XmlWriterSettings();
            XmlWriterSettings.Indent = true;
        }

        public IXmlDocument Document { get; }
        public IXmlPattern Pattern { get; set; }
        public XmlReaderSettings XmlReaderSettings { get; }
        public XmlWriterSettings XmlWriterSettings { get; }
        

        public virtual bool Load(Stream stream)
        {
            if (Document == null)
                throw new ArgumentNullException(nameof(Document));
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            var res = false;
            XDocument xdoc = null;
            try
            {
                using (var xmlReader = XmlReader.Create(stream, XmlReaderSettings))
                {
                    if (stream.Length > 0)
                    {
                        xdoc = XDocument.Load(xmlReader);
                    }

                    if (xdoc != null)
                    {
                        Document.Configure(xdoc);
                        res = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return res;
        }


        public virtual bool Save(IXmlDocument document, Stream stream)
        {
            if (!stream.CanWrite)
                throw new NotSupportedException("The stream cannot be written to");
            try
            {
                using (var xmlWriter = XmlWriter.Create(stream, XmlWriterSettings))
                {
                    var xdoc = _xdoc ?? new XDocument();
                    Export(xdoc);
                    xdoc.Save(xmlWriter);
                    _xdoc = xdoc;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
