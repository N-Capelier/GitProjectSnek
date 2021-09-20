using System.Xml.Serialization;
using System.IO;

namespace Saving
{
    public static class Serializer
    {

        public static string Serialize<T>(this T _elementToSerialize)
        {
            XmlSerializer _xml = new XmlSerializer(typeof(T));
            StringWriter _writer = new StringWriter();
            _xml.Serialize(_writer, _elementToSerialize);
            return _writer.ToString();
        }

        public static T Deserialize<T>(this string _serializedElement)
        {
            XmlSerializer _xml = new XmlSerializer(typeof(T));
            StringReader _reader = new StringReader(_serializedElement);
            return (T)_xml.Deserialize(_reader);
        }
    }
}