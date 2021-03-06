﻿using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Lux.Serialization.Xml
{
    public class XmlPattern : IXmlPattern
    {
        //public static IXmlPattern Default               => new DefaultXmlPattern();
        public static IXmlPattern Default               = new DefaultXmlPattern();
        public static readonly IXmlPattern Instance     = Default;

        static XmlPattern()
        {

        }

        
        private IList<XmlConventionBase> _conventions;

        public XmlPattern()
        {
            _conventions = new List<XmlConventionBase>();
        }

        public IList<XmlConventionBase> Conventions
        {
            get { return _conventions; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));
                _conventions = value;
            }
        }

        public virtual void Configure(IXmlObject obj, XElement source)
        {
            foreach (var convention in Conventions)
            {
                convention.Configure(obj, source);
            }

            //obj.Configure(target);
        }

        public virtual void Export(IXmlObject obj, XElement target)
        {
            foreach (var convention in Conventions)
            {
                convention.Export(obj, target);
            }

            //obj.Export(target);
        }

    }
}
