using System;
using System.Linq;
using System.Xml.Linq;

namespace Lux.Serialization.Xml
{
    public class DefaultArrayXmlConvention : XmlConventionBase
    {
        public override void Configure(IXmlConfigurable configurable, XElement element)
        {
            ConfigureArray(configurable, element);
        }


        protected virtual void ConfigureArray(IXmlConfigurable configurable, XElement element)
        {
            var array = configurable as IXmlArray;
            if (array == null)
                return;

            // Clear
            array.Clear();

            var itemElems = element.Elements("item").Where(x => x != null).ToList();
            if (itemElems.Any())
            {
                foreach (var elem in itemElems)
                {
                    try
                    {
                        // Append items
                        var node = XmlInstantiator.InstantiateNode(elem);
                        array.AddItem(node);
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
            }
        }




        public override void Export(IXmlExportable exportable, XElement element)
        {
            ExportArray(exportable, element);
        }


        protected virtual void ExportArray(IXmlExportable exportable, XElement element)
        {
            var array = exportable as IXmlArray;
            if (array == null)
                return;

            element.Elements("item").Remove();

            var nodes = array.Nodes().Where(x => x != null).ToList();
            if (nodes.Any())
            {
                foreach (var node in nodes)
                {
                    var type = node.GetType();
                    var elem = new XElement("item");
                    element.Add(elem);
                    elem.SetAttributeValue("type", type.FullName + ", " + type.Assembly.GetName().Name);
                    node.Export(elem);
                }
            }
        }

    }
}
