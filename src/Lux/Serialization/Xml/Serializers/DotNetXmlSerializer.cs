using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Lux.Serialization.Xml
{
    /// <summary>
    /// Wrapper for System.Xml.Serialization.XmlSerializer.
    /// </summary>
    public class DotNetXmlSerializer : ISerializer, IDeserializer
    {
        /// <summary>
        /// Default constructor, does not specify namespace
        /// </summary>
        public DotNetXmlSerializer()
        {
            ContentType = "application/xml";
            Encoding = Encoding.UTF8;
        }

        /// <summary>
        /// Specify the namespaced to be used when serializing
        /// </summary>
        /// <param name="namespace">XML namespace</param>
        public DotNetXmlSerializer(string @namespace)
            : this()
        {
            Namespace = @namespace;
        }

        /// <summary>
        /// Name of the root element to use when serializing
        /// </summary>
        public string RootElement { get; set; }

        /// <summary>
        /// XML namespace to use when serializing
        /// </summary>
        public string Namespace { get; set; }

        /// <summary>
        /// Format string to use when serializing dates
        /// </summary>
        public string DateFormat { get; set; }

        /// <summary>
        /// Content type for serialized content
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Encoding for serialized content
        /// </summary>
        public Encoding Encoding { get; set; }


        /// <summary>
        /// Serialize the object as XML
        /// </summary>
        /// <param name="obj">Object to serialize</param>
        /// <returns>XML as string</returns>
        public string Serialize(object obj)
        {
            var ns = new XmlSerializerNamespaces();
            ns.Add(string.Empty, Namespace);

            var serializer = new System.Xml.Serialization.XmlSerializer(obj.GetType());
            var writer = new EncodingStringWriter(Encoding);
            serializer.Serialize(writer, obj, ns);
            var xml = writer.ToString();
            return xml;
        }
        
        public object Deserialize(object input, Type type)
        {
            var xml = (input ?? "").ToString();
            if (string.IsNullOrEmpty(xml))
                return null;

            var bytes = Encoding.GetBytes(xml);
            using (var stream = new MemoryStream(bytes))
            {
                var serializer = new System.Xml.Serialization.XmlSerializer(type);
                var obj = serializer.Deserialize(stream);
                return obj;
            }
        }

        public T Deserialize<T>(object input)
        {
            var type = typeof (T);
            var obj = Deserialize(input, type);
            var result = (T) obj;
            return result;
        }


        /// <summary>
        /// Need to subclass StringWriter in order to override Encoding
        /// </summary>
        private class EncodingStringWriter : StringWriter
        {
            public EncodingStringWriter(Encoding encoding)
            {
                Encoding = encoding;
            }

            public override Encoding Encoding { get; }
        }

    }
}