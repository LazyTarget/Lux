using System;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using Lux.Config.Xml;
using Lux.Dependency;
using Lux.Xml;

namespace Lux
{
    public class FrameworkConfig : XmlConfigBase
    {
        private AssignableVariable<CultureInfo> _cultureInfo = new AssignableVariable<CultureInfo>();
        public CultureInfo CultureInfo
        {
            get { return _cultureInfo?.Value; }
            set { _cultureInfo.Value = value; }
        }


        private AssignableVariable<IDependencyContainer> _dependencyContainer =
            new AssignableVariable<IDependencyContainer>();

        public IDependencyContainer DependencyContainer
        {
            get { return _dependencyContainer?.Value; }
            set { _dependencyContainer.Value = value; }
        }

        public FrameworkConfig()
        {
            
        }


        public virtual void Apply()
        {
            if (_cultureInfo.Assigned)
                Framework.CultureInfo = CultureInfo;
            if (_dependencyContainer.Assigned)
                Framework.DependencyContainer = DependencyContainer;
        }

        public override void Configure(XElement element)
        {
            XElement elem;
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            elem = FindElement(element, nameof(CultureInfo));
            if (elem != null)
            {
                var name = elem.GetAttributeValue("value") ?? elem.Value;
                CultureInfo = CultureInfo.GetCultureInfo(name);
            }

            elem = FindElement(element, nameof(DependencyContainer));
            if (elem != null)
            {
                var typeStr = elem.GetAttributeValue("type");
                var type = Type.GetType(typeStr, true);
                if (!typeof (IDependencyContainer).IsAssignableFrom(type))
                    throw new InvalidCastException("Invalid type");

                object obj;
                if (Framework.TypeInstantiator != null)
                    obj = Framework.TypeInstantiator.Instantiate(type);
                else
                    obj = Activator.CreateInstance(type);

                var instance = (IDependencyContainer) obj;
                DependencyContainer = instance;
            }
        }

        public override void Export(XElement element)
        {
            
        }


        private XElement FindElement(XElement element, XName name)
        {
            var elem = element.Elements()
                .FirstOrDefault(
                    x =>
                        x.Name.LocalName.Equals(name.LocalName,
                            StringComparison.InvariantCultureIgnoreCase));
            return elem;
        }
    }
}