using System;
using System.Linq;
using System.Xml.Linq;

namespace Lux.Serialization.Xml
{
    public class DefaultArrayXmlConvention : XmlConventionBase
    {
        public DefaultArrayXmlConvention()
        {

        }




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
            array.ClearItems();

            var itemElems = element.Elements("item").Where(x => x != null).ToList();
            if (itemElems.Any())
            {
                foreach (var elem in itemElems)
                {
                    try
                    {
                        // Append items
                        var node = XmlInstantiator.InstantiateNode(array, elem);
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

            // Clear
            element.Elements("item").Remove();

            var nodes = array.Items().Where(x => x != null).ToList();
            if (nodes.Any())
            {
                foreach (var node in nodes)
                {
                    if (node == null)
                        continue;

                    var type = node.GetType();
                    var typeString = type.FullName + ", " + type.Assembly.GetName().Name;

                    // Append items
                    var elem = new XElement("item");
                    element.Add(elem);
                    elem.SetAttributeValue("type", typeString);
                    node.Export(elem);
                }
            }
        }

    }
}
