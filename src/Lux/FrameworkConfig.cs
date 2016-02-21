using System;
using System.Globalization;
using System.Xml.Linq;
using Lux.Config.Xml;
using Lux.Xml;

namespace Lux
{
    public class FrameworkConfig : XmlConfigBase
    {
        public CultureInfo CultureInfo { get; set; }



        public virtual void Apply()
        {
            Framework.CultureInfo = CultureInfo;
        }

        public override void Configure(XElement element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            var elem = element.Element("cultureInfo");
            if (elem != null)
            {
                var name = elem.GetAttributeValue("value") ?? elem.Value;
                CultureInfo = CultureInfo.GetCultureInfo(name);
            }
        }

        public override void Export(XElement element)
        {
                
        }
    }
}