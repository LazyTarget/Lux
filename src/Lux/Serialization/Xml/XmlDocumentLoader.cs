using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace Lux.Serialization.Xml
{
    [Obsolete("Obsolete library")]
    public class XmlDocumentLoader
    {
        public XmlDocumentLoader()
        {
            var pattern = XmlPattern.Instance;
            Document = new XmlDocument(pattern);

            XmlReaderSettings = new XmlReaderSettings();
            XmlReaderSettings.DtdProcessing = DtdProcessing.Parse;

            XmlWriterSettings = new XmlWriterSettings();
            XmlWriterSettings.Indent = true;
        }

        public XmlDocumentLoader(IXmlDocument document)
        {
            if (document == null)
                throw new ArgumentNullException(nameof(document));
            Document = document;

            XmlReaderSettings = new XmlReaderSettings();
            XmlReaderSettings.DtdProcessing = DtdProcessing.Parse;

            XmlWriterSettings = new XmlWriterSettings();
            XmlWriterSettings.Indent = true;
        }


        public IXmlDocument Document { get; }
        public XmlReaderSettings XmlReaderSettings { get; }
        public XmlWriterSettings XmlWriterSettings { get; }


        protected virtual XDocument LoadXDocument(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));
            XDocument xdoc = null;
            try
            {
                using (var xmlReader = XmlReader.Create(stream, XmlReaderSettings))
                {
                    if (stream.Length > 0)
                    {
                        xdoc = XDocument.Load(xmlReader);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return xdoc;
        }


        public virtual bool Load(Stream stream)
        {
            var res = false;
            try
            {
                var xdoc = LoadXDocument(stream);
                if (xdoc != null)
                {
                    Document.Configure(xdoc);
                    res = true;
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
                var xdoc = LoadXDocument(stream);

                using (var xmlWriter = XmlWriter.Create(stream, XmlWriterSettings))
                {
                    Document.Export(xdoc);
                    xdoc.Save(xmlWriter);
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
