using System;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using Lux.Config.Xml;
using Lux.Dependency;
using Lux.Diagnostics;
using Lux.Serialization.Xml;
using Lux.Xml;

namespace Lux
{
    public class FrameworkConfig : XmlConfigBase
    {
        /// <summary>
        /// Determinds whether Framework.cs should invoke LoadFromConfig.cs when static constructor is called 
        /// </summary>
        public static bool AutoLoadConfig = true;


        public FrameworkConfig()
        {

        }

        private Assignable<CultureInfo> _cultureInfo = new Assignable<CultureInfo>();
        public CultureInfo CultureInfo
        {
            get { return _cultureInfo?.Value; }
            set { _cultureInfo.Value = value; }
        }


        private Assignable<IDependencyContainer> _dependencyContainer =
            new Assignable<IDependencyContainer>();

        public IDependencyContainer DependencyContainer
        {
            get { return _dependencyContainer?.Value; }
            set { _dependencyContainer.Value = value; }
        }



        private Assignable<ILogFactory> _logFactory
            = new Assignable<ILogFactory>();

        public ILogFactory LogFactory
        {
            get { return _logFactory?.Value; }
            set { _logFactory.Value = value; }
        }


        public virtual void Apply()
        {
            if (_cultureInfo.Assigned)
                Framework.CultureInfo = CultureInfo;
            if (_logFactory.Assigned)
                Framework.LogFactory = LogFactory;
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

            elem = FindElement(element, nameof(LogFactory));
            if (elem != null)
            {
                var typeStr = elem.GetAttributeValue("type");
                var type = Type.GetType(typeStr, true);
                if (!typeof(ILogFactory).IsAssignableFrom(type))
                    throw new InvalidCastException("Invalid type");

                object obj;
                if (Framework.TypeInstantiator != null)
                    obj = Framework.TypeInstantiator.Instantiate(type);
                else
                    obj = Activator.CreateInstance(type);

                var instance = (ILogFactory)obj;
                LogFactory = instance;
            }
        }

        public override void Export(XElement element)
        {
            Type type;
            XElement elem;
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            type = LogFactory?.GetType();
            if (type != null)
            {
                elem = element.GetOrCreateElement(nameof(LogFactory));

                var xmlExportable = LogFactory as IXmlExportable;
                if (xmlExportable != null)
                {
                    xmlExportable.Export(elem);
                }
                else
                {
                    var typeString = type.FullName +
                                            (type.Assembly.IsFullyTrusted
                                                ? ""
                                                : ", " + type.Assembly.GetName().Name);
                    elem.SetAttr("type", typeString);
                }
            }
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