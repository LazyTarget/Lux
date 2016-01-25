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




        public override void Configure(IXmlObject obj, XElement source)
        {
            var array = obj as IXmlArray;
            if (array == null)
                return;

            // Clear
            array.ClearItems();

            var itemElems = source.Elements("item").ToList();
            if (itemElems.Any())
            {
                foreach (var elem in itemElems)
                {
                    if (elem == null)
                        continue;
                    try
                    {
                        // Append items
                        var node = XmlInstantiator.InstantiateNode(elem, array);
                        array.AddItem(node);
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
            }
        }



        public override void Export(IXmlObject obj, XElement target)
        {
            var array = obj as IXmlArray;
            if (array == null)
                return;
            
            // Clear
            target.Elements("item").Remove();

            var nodes = array.Items().ToList();
            if (nodes.Any())
            {
                foreach (var subNode in nodes)
                {
                    if (subNode == null)
                        continue;

                    var type = subNode.GetType();
                    var typeString = type.FullName + ", " + type.Assembly.GetName().Name;

                    // Append items
                    var elem = new XElement("item");
                    target.Add(elem);
                    elem.SetAttributeValue("type", typeString);
                    //node.Export(elem);

                    var subObj = subNode as XmlObject;
                    if (subObj != null)
                    {
                        var pattern = (obj as XmlObject)?.Pattern ?? subObj.Pattern;
                        pattern.Export(subObj, elem);
                    }
                }
            }
        }

    }
}
