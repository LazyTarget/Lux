using System;

namespace Lux.Config
{
    public interface IXmlConfigLocation : IConfigLocation
    {
        Uri Uri { get; set; }
        string RootElementName { get; set; }
        string RootElementPath { get; set; }
    }
}