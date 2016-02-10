using System;

namespace Lux.Config
{
    public class XmlConfigSource : ConfigSource
    {
        public Uri Uri { get; set; }
        public string RootElementName { get; set; }
    }
}