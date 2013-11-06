namespace TeamCityRestClient
{
    public interface IConfiguration
    {
        string BaseUrl { get; }

        string UserName { get; }

        string Password { get; }
    }
}