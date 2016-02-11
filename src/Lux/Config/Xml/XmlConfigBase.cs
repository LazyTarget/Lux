using System.Xml.Linq;
using Lux.Serialization.Xml;

namespace Lux.Config
{
    public abstract class XmlConfigBase : ConfigBase, IXmlConfigurable, IXmlExportable
    {
        public new XmlConfigLocation Location
        {
            get { return (XmlConfigLocation)base.Location; }
            set { base.Location = value; }
        }

        public abstract void Configure(XElement element);
        public abstract void Export(XElement element);
    }
}