using System.Configuration;
using System.Xml;
using System.Xml.Linq;
using Lux.Serialization.Xml;

namespace Lux.Config.Xml
{
    public abstract class XmlConfigBase : ConfigBase, IXmlConfigurable, IXmlExportable, IConfigurationSectionHandler
    {
        public new XmlConfigDescriptor Descriptor
        {
            get { return (XmlConfigDescriptor)base.Descriptor; }
            set { base.Descriptor = value; }
        }

        public abstract void Configure(XElement element);
        public abstract void Export(XElement element);

        public virtual object Create(object parent, object configContext, XmlNode section)
        {
            return section;
        }
    }
}