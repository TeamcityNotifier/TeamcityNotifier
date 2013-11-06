namespace TeamcityNotifier.Wrapper
{
    using System.Xml.Serialization;

    internal class XmlSerializerWrapper : IXmlSerializer
    {
        private readonly XmlSerializer xmlSerializer;

        public XmlSerializerWrapper(XmlSerializer xmlSerializer)
        {
            this.xmlSerializer = xmlSerializer;
        }

        public object Deserialize(IStringReader reader)
        {
            return xmlSerializer.Deserialize(reader.ToStringReader());
        }

        public XmlSerializer ToXmlSerializer()
        {
            return xmlSerializer;
        }
    }
}