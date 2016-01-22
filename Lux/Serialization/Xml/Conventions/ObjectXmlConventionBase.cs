using System;
using System.Linq;
using System.Xml.Linq;

namespace Lux.Serialization.Xml
{
    public abstract class ObjectXmlConventionBase : XmlConventionBase
    {
        protected ObjectXmlConventionBase()
        {

        }

        public override void Configure(IXmlConfigurable configurable, XElement element)
        {
            var obj = configurable as IXmlObject;
            if (obj != null)
                ConfigureObject(obj, element);
        }

        protected abstract void ConfigureObject(IXmlObject obj, XElement element);



        public override void Export(IXmlExportable exportable, XElement element)
        {
            var obj = exportable as IXmlObject;
            if (obj != null)
                ExportObject(obj, element);
        }

        protected abstract void ExportObject(IXmlObject obj, XElement element);

    }
}
