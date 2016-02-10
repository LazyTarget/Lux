using System.Xml.Linq;
using Lux.Serialization.Xml;

namespace Lux.Config
{
    public abstract class XmlConfigBase : ConfigBase, IXmlConfigurable, IXmlExportable
    {
        public new XmlConfigSource Source
        {
            get { return (XmlConfigSource)base.Source; }
            set { base.Source = value; }
        }

        public abstract void Configure(XElement element);
        public abstract void Export(XElement element);
    }
}