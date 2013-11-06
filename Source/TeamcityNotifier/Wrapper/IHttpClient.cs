namespace TeamcityNotifier.Wrapper
{
    using System.Net.Http;
    using System.Threading.Tasks;

    public interface IHttpClient
    {
        Task<string> GetStringAsync(IUri uri);

        HttpClient ToHttpClient();
    }
}