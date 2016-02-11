using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using Lux.Extensions;
using Lux.Serialization.Xml;
using Lux.Xml;

namespace Lux.Config
{
    public class XmlConfigManager : IConfigManager
    {
        public XmlConfigManager()
        {
            XmlReaderSettings = new XmlReaderSettings();
            XmlReaderSettings.DtdProcessing = DtdProcessing.Parse;

            XmlWriterSettings = new XmlWriterSettings();
            XmlWriterSettings.Indent = true;
        }

        public XmlReaderSettings XmlReaderSettings { get; }

        public XmlWriterSettings XmlWriterSettings { get; }

        public bool SaveAndReplace { get; set; }


        public bool CanLoad<TConfig>(IConfigLocation location)
            where TConfig : IConfig
        {
            location = GetLocationOrDefault(location);
            ValidateLocation(location);

            var xmlConfigLocation = (IXmlConfigLocation)location;
            if (xmlConfigLocation.Uri == null)
                return false;

            if (typeof(TConfig).IsAssignableFrom(typeof(IXmlConfigurable)))
                return false;   // todo: extend, use a XmlSerializer?

            return true;
        }


        public TConfig Load<TConfig>(IConfigLocation location)
            where TConfig : IConfig
        {
            location = GetLocationOrDefault(location);
            ValidateLocation(location);

            var xmlConfigLocation = (IXmlConfigLocation)location;
            if (xmlConfigLocation.Uri == null)
                throw new ArgumentException("Invalid target uri", nameof(location));

            var stream = GetStreamFromLocation(location);
            var document = LoadXDocument(stream);

            //var config = Framework.TypeInstantiator.Instantiate<TConfig>();
            var config = ParseFromXDocument<TConfig>(document, location);
            config.Location = location;
            return config;
        }


        public bool CanSave<TConfig>(TConfig config, IConfigLocation location)
            where TConfig : IConfig
        {
            location = GetLocationOrDefault(location);
            ValidateLocation(location);

            var xmlConfigLocation = (IXmlConfigLocation)location;
            if (xmlConfigLocation.Uri == null)
                return false;
            if (!xmlConfigLocation.Uri.IsFile)
                return false;

            if (typeof(TConfig).IsAssignableFrom(typeof(IXmlExportable)))
                return false;   // todo: extend, use a XmlSerializer?

            return true;
        }


        public virtual object Save<TConfig>(TConfig config, IConfigLocation location)
            where TConfig : IConfig
        {
            location = GetLocationOrDefault(location);
            ValidateLocation(location);

            var xmlConfigLocation = (IXmlConfigLocation)location;
            if (xmlConfigLocation.Uri == null)
                throw new ArgumentException("Invalid target uri", nameof(location));
            if (!xmlConfigLocation.Uri.IsFile)
                throw new NotSupportedException($"Saving against a network path is not supported");


            XDocument xdoc = null;
            if (SaveAndReplace)
            {
                // Should save as new
                //xdoc = new XDocument();
            }
            else
            {
                var source = (config.Location as IXmlConfigLocation) ?? location;
                var sourceStream = GetStreamFromLocation(source);
                xdoc = LoadXDocument(sourceStream);
            }

            var document = ExportToXDocument(xdoc, config, location);
            var targetStream = GetStreamFromLocation(location);
            var res = SaveXDocument(document, targetStream);
            if (res)
            {
                config.Location = location;
            }
            return res;
        }



        protected virtual IConfigLocation GetLocationOrDefault(IConfigLocation location)
        {
            if (location != null)
            {
                if (!(location is IXmlConfigLocation))
                    throw new NotSupportedException($"ConfigSource of type '{location.GetType()}' is not supported");
            }
            return location;
        }

        protected virtual void ValidateLocation(IConfigLocation location)
        {
            if (location == null)
                throw new ArgumentNullException(nameof(location));
            if (!(location is IXmlConfigLocation))
                throw new NotSupportedException($"ConfigSource of type '{location.GetType()}' is not supported");
        }


        protected virtual Stream GetStreamFromLocation(IConfigLocation location)
        {
            ValidateLocation(location);
            var xmlConfigLocation = (IXmlConfigLocation)location;

            var configUri = xmlConfigLocation.Uri;
            if (!configUri.IsAbsoluteUri)
            {
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configUri.ToString());
                configUri = new Uri(path);
            }

            if (configUri.IsFile)
            {
                var fileInfo = new FileInfo(configUri.LocalPath);
                if (fileInfo.Exists)
                {
                    // todo: truncate?
                    var fileStream = File.Open(fileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.Read);
                    return fileStream;
                }
                else
                {
                    //throw new FileNotFoundException("Config file not found", fileInfo.FullName);

                    var fileStream = File.Open(fileInfo.FullName, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.Read);
                    return fileStream;
                }
            }
            else
            {
                var client = new System.Net.Http.HttpClient();
                try
                {
                    var task = client.GetAsync(configUri);
                    var response = task.WaitForResult();
                    response.EnsureSuccessStatusCode();

                    var task2 = response.Content.ReadAsStreamAsync();
                    var stream = task2.WaitForResult();
                    return stream;
                }
                catch (Exception ex)
                {
                    throw;
                }
                throw new NotImplementedException("Network paths not implemented");
            }
        }


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


        protected virtual bool SaveXDocument(XDocument xdoc, Stream stream)
        {
            if (!stream.CanWrite)
                throw new NotSupportedException("The stream cannot be written to");
            try
            {
                using (var xmlWriter = XmlWriter.Create(stream, XmlWriterSettings))
                {
                    xdoc.Save(xmlWriter);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        protected virtual TConfig ParseFromXDocument<TConfig>(XDocument document, IConfigLocation location)
            where TConfig : IConfig
        {
            ValidateLocation(location);
            var xmlConfigLocation = (IXmlConfigLocation)location;

            if (document == null)
                document = new XDocument();

            var expr = xmlConfigLocation.RootElementExpression ?? xmlConfigLocation.RootElementName;
            //var rootElement = GetRootElem(document, xmlConfigLocation.RootElementName);
            var rootElement = GetRootElemByExpression(document, expr);

            if (rootElement == null)
            {

            }

            var config = Framework.TypeInstantiator.Instantiate<TConfig>();
            config.Location = xmlConfigLocation;
            var xmlConfigurable = config as IXmlConfigurable;
            if (xmlConfigurable != null)
            {
                xmlConfigurable.Configure(rootElement);
            }
            else
            {
                // todo: Use XmlSerializer?
            }
            return config;
        }


        protected virtual XDocument ExportToXDocument(XDocument document, IConfig config, IConfigLocation location)
        {
            ValidateLocation(location);
            var xmlConfigLocation = (IXmlConfigLocation)location;

            if (document == null)
                document = new XDocument();

            var expr = xmlConfigLocation.RootElementExpression ?? xmlConfigLocation.RootElementName;
            //var rootElement = GetOrCreateRootElem(document, xmlConfigLocation.RootElementName);
            var rootElement = GetOrCreateRootElemByExpression(document, expr);

            var xmlExportable = config as IXmlExportable;
            if (xmlExportable != null)
            {
                xmlExportable.Export(rootElement);
            }
            else
            {
                // todo: Use XmlSerializer?
            }
            return document;
        }



        protected virtual XElement GetRootElem(XDocument document, string rootElementName)
        {
            var rootElement = document.Element(rootElementName);
            if (rootElement == null)
            {
                if (document.Root != null)
                    rootElement = document.Root.Element(rootElementName);
                else
                    rootElement = document.Element(rootElementName);
            }
            return rootElement;
        }

        protected virtual XElement GetOrCreateRootElem(XDocument document, string rootElementName)
        {
            var rootElement = document.Element(rootElementName);
            if (rootElement == null)
            {
                if (document.Root != null)
                    rootElement = document.Root.GetOrCreateElement(rootElementName);
                else
                    rootElement = document.GetOrCreateElement(rootElementName);
            }
            return rootElement;
        }



        protected virtual XElement GetRootElemByExpression(XDocument document, string rootElementExpression)
        {
            try
            {

                var expr = XPathExpression.Compile(rootElementExpression);
                var navigator = document.CreateNavigator();
                var select = navigator.SelectSingleNode(expr);

                var node = navigator.Evaluate(expr);
                var rootElement = node as XElement;
                if (rootElement == null)
                {
                    if (document.Root != null)
                        rootElement = document.Root.Element(rootElementExpression);
                    else
                        rootElement = document.Element(rootElementExpression);
                }
                return rootElement;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        protected virtual XElement GetOrCreateRootElemByExpression(XDocument document, string rootElementExpression)
        {
            try
            {

                var rootElement = document.Element(rootElementExpression);
                if (rootElement == null)
                {
                    if (document.Root != null)
                        rootElement = document.Root.GetOrCreateElement(rootElementExpression);
                    else
                        rootElement = document.GetOrCreateElement(rootElementExpression);
                }
                return rootElement;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}