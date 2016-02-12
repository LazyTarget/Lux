using System;
using System.IO;

namespace Lux.Config.Xml
{
    public class XmlConfigLocation : IXmlConfigLocation
    {
        public Uri Uri { get; set; }
        public string RootElementName { get; set; }
        public string RootElementPath { get; set; }


        public bool CanRead { get; }
        public bool CanWrite { get; }

        public Stream GetStreamForRead(IConfigArguments arguments)
        {
            throw new NotImplementedException();
        }

        public Stream GetStreamForWrite(IConfigArguments arguments)
        {
            throw new NotImplementedException();
        }
    }
}