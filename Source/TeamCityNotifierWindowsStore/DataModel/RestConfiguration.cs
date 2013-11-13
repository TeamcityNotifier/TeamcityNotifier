namespace TeamCityNotifierWindowsStore.Data
{
    using TeamcityNotifier;

    public class RestConfiguration : IRestConfiguration
    {
        public RestConfiguration(string baseUrl, string userName, string password, string serverName)
        {
            this.BaseUrl = baseUrl;
            this.UserName = userName;
            this.Password = password;
            this.Name = serverName;
        }

        public string BaseUrl { get; private set; }

        public string UserName { get; private set; }

        public string Password { get; private set; }

        public string Name { get; private set; }
    }
}