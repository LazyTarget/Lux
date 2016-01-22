﻿using Lux.Interfaces;

namespace Lux.Xml
{
    public interface IXmlNode : INode, IXmlConfigurable, IXmlExportable
    {
        XmlPattern Pattern { get; }
        IXmlNode ParentNode { get; }
    }
}