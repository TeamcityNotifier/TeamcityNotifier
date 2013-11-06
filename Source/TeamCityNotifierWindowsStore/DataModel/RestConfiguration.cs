namespace TeamCityNotifierWindowsStore.Data
{
    using TeamcityNotifier;

    public class RestConfiguration : IRestConfiguration
    {
        public RestConfiguration(string baseUrl, string userName, string password)
        {
            this.BaseUrl = baseUrl;
            this.UserName = userName;
            this.Password = password;
        }

        public string BaseUrl { get; private set; }

        public string UserName { get; private set; }

        public string Password { get; private set; }
    }
}