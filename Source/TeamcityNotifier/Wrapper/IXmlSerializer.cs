namespace TeamcityNotifier.Wrapper
{
    using System.Xml.Serialization;

    public interface IXmlSerializer
    {
        object Deserialize(IStringReader reader);

        XmlSerializer ToXmlSerializer();
    }
}