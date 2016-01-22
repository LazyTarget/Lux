using System.Collections.Generic;

namespace Lux.Serialization.Xml
{
    public interface IXmlPattern : IXmlConfigurator, IXmlExporter
    {
        IList<XmlConventionBase> Conventions { get; set; }
    }
}