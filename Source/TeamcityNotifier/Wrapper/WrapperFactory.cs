namespace TeamcityNotifier.Wrapper
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Xml.Serialization;

    public class WrapperFactory :IWrapperFactory
    {
        public IXmlSerializer CreateXmlSerializer(Type type)
        {
            return new XmlSerializerWrapper(new XmlSerializer(type));
        }

        public IHttpClient CreateHttpClientHandler(string username, string password)
        {
            var handler = new HttpClientHandler
            {
                Credentials = new NetworkCredential(username, password),
                UseCookies = true,
                PreAuthenticate = true

            };

            return new HttpClientWrapper(new  HttpClient(handler));
        }

        public IStringReader CreateStringReader(string str)
        {
            return new StringReaderWrapper(new StringReader(str));
        }

        public IUri CreateUri(string baseUri)
        {
            return new UriWrapper(new Uri(baseUri));
        }

        public IUri CreateUri(IUri baseUri, string relativeUri)
        {
            return new UriWrapper(new Uri(baseUri.ToUri(), relativeUri));
        }
    }
}