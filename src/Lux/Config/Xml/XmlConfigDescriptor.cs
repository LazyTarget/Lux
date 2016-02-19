using System;
using System.Collections.Generic;
using Lux.Data;
using Lux.Extensions;

namespace Lux.Config.Xml
{
    public class XmlConfigDescriptor : IConfigDescriptor
    {
        public XmlConfigDescriptor()
        {
            Properties = new Dictionary<string, object>();
            DataStore = new ConfigXmlDataStore();
            Parser = new XmlConfigParser();
        }

        public IDictionary<string, object> Properties { get; set; }
        public IDataStore<IConfigDescriptor> DataStore { get; set; }
        public IConfigParser Parser { get; set; }


        public Uri Uri
        {
            get { return Properties.GetOrDefault(nameof(Uri)).CastTo<Uri>(); }
            set { Properties[nameof(Uri)] = value; }
        }

        public string RootElementPath
        {
            get { return Properties.GetOrDefault(nameof(RootElementPath)).CastTo<string>(); }
            set { Properties[nameof(RootElementPath)] = value; }
        }

    }
}