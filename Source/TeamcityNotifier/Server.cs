namespace TeamCityRestClient
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Xml.Serialization;

    using xsdtest;

    public class Server : IServer
    {
        private readonly Uri baseUrl;
        private readonly HttpClient client;

        public Server(string baseUrl, string username, string password)
        {
            this.baseUrl = new Uri(baseUrl);

            var handler = new HttpClientHandler
            {
                Credentials = new NetworkCredential(username, password),
                UseCookies = true,
                PreAuthenticate = true

            };

            this.client = new HttpClient(handler);
        }

        public T Get<T>(string url)
        {
            var uri = new Uri(baseUrl, url);

            var asyncTask = this.client.GetStringAsync(uri);

            var serializer = new XmlSerializer(typeof(T));

            using (var reader = new StringReader(asyncTask.Result))
            {
                var objectDto = (T)serializer.Deserialize(reader);

                return objectDto;
            }
        }

        public IEnumerable<IProject> GetProjects()
        {
            throw  new NotImplementedException();
        }
    }
}
