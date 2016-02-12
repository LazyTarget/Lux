using System.Configuration;
using System.Xml;
using System.Xml.Linq;
using Lux.Serialization.Xml;

namespace Lux.Config
{
    public abstract class XmlConfigBase : ConfigBase, IXmlConfigurable, IXmlExportable, IConfigurationSectionHandler
    {
        public new IXmlConfigLocation Location
        {
            get { return (IXmlConfigLocation)base.Location; }
            set { base.Location = value; }
        }

        public abstract void Configure(XElement element);
        public abstract void Export(XElement element);

        public virtual object Create(object parent, object configContext, XmlNode section)
        {
            return section;
        }
    }
}