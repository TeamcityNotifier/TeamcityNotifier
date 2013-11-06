namespace TeamcityNotifier.Wrapper
{
    using System.Net.Http;
    using System.Threading.Tasks;

    internal class HttpClientWrapper : IHttpClient
    {
        private readonly HttpClient httpClient;

        public HttpClientWrapper(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public Task<string> GetStringAsync(IUri uri)
        {
            return httpClient.GetStringAsync(uri.ToUri());
        }

        public HttpClient ToHttpClient()
        {
            return httpClient;
        }
    }
}