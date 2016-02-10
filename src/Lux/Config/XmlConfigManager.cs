using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
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


        public bool CanLoad<TConfig>(ConfigSource source)
            where TConfig : IConfig
        {
            var xmlConfigSource = source as XmlConfigSource;
            if (xmlConfigSource == null)
                return false;
            if (xmlConfigSource.Uri == null)
                return false;

            if (typeof (TConfig).IsAssignableFrom(typeof (IXmlConfigurable)))
                return false;   // todo: extend, use a XmlSerializer?

            return true;
        }


        public TConfig Load<TConfig>(ConfigSource source)
            where TConfig : IConfig
        {
            var xmlConfigSource = source as XmlConfigSource;
            if (xmlConfigSource == null)
                throw new ArgumentNullException(nameof(xmlConfigSource), "Invalid configuration source");
            
            var stream = GetStreamFromSource(source);
            var document = LoadXDocument(stream);

            //var config = Framework.TypeInstantiator.Instantiate<TConfig>();
            var config = ParseFromXDocument<TConfig>(document, source);
            config.Source = xmlConfigSource;
            return config;
        }


        public bool CanSave<TConfig>(TConfig config, ConfigSource target)
            where TConfig : IConfig
        {
            var xmlConfigTarget = target as XmlConfigSource;
            if (xmlConfigTarget == null)
                return false;
            if (xmlConfigTarget.Uri == null)
                return false;
            if (!xmlConfigTarget.Uri.IsFile)
                return false;

            if (typeof(TConfig).IsAssignableFrom(typeof(IXmlExportable)))
                return false;   // todo: extend, use a XmlSerializer?

            return true;
        }


        public virtual object Save<TConfig>(TConfig config, ConfigSource target)
            where TConfig : IConfig
        {
            var xmlConfigTarget = target as XmlConfigSource;
            if (xmlConfigTarget == null)
                throw new NotSupportedException($"ConfigSource of type '{target.GetType()}' is not supported");
            if (xmlConfigTarget.Uri == null)
                throw new ArgumentException("Invalid target uri", nameof(target));
            if (!xmlConfigTarget.Uri.IsFile)
                throw new NotSupportedException($"Saving against a network path is not supported");


            XDocument xdoc = null;
            if (SaveAndReplace)
            {
                // Should save as new
                //xdoc = new XDocument();
            }
            else
            {
                var source = (config.Source as XmlConfigSource) ?? target;
                var sourceStream = GetStreamFromSource(source);
                xdoc = LoadXDocument(sourceStream);
            }

            var document = ExportToXDocument(xdoc, config, target);
            var targetStream = GetStreamFromSource(target);
            var res = SaveXDocument(document, target, targetStream);
            if (res)
            {
                config.Source = target;
            }
            return res;
        }


        protected virtual Stream GetStreamFromSource(ConfigSource source)
        {
            var xmlConfigSource = source as XmlConfigSource;
            if (xmlConfigSource == null)
                throw new NotSupportedException($"ConfigSource of type '{source.GetType()}' is not supported");

            var configUri = xmlConfigSource.Uri;
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


        protected virtual bool SaveXDocument(XDocument xdoc, ConfigSource target, Stream stream)
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


        protected virtual TConfig ParseFromXDocument<TConfig>(XDocument document, ConfigSource source)
            where TConfig : IConfig
        {
            var xmlConfigSource = source as XmlConfigSource;
            if (xmlConfigSource == null)
                throw new NotSupportedException($"ConfigSource of type '{source.GetType()}' is not supported");

            if (document == null)
                document = new XDocument();
            //var rootElement = GetOrCreateRootElem(document, xmlConfigSource.RootElementName);
            var rootElement = GetRootElem(document, xmlConfigSource.RootElementName);
            if (rootElement == null)
            {
                
            }
            
            var config = Framework.TypeInstantiator.Instantiate<TConfig>();
            config.Source = xmlConfigSource;
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


        protected virtual XDocument ExportToXDocument(XDocument document, IConfig config, ConfigSource target)
        {
            var xmlConfigTarget = target as XmlConfigSource;
            if (xmlConfigTarget == null)
                throw new NotSupportedException($"ConfigSource of type '{target.GetType()}' is not supported");

            if (document == null)
                document = new XDocument();
            var rootElement = GetOrCreateRootElem(document, xmlConfigTarget.RootElementName);

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
    }
}