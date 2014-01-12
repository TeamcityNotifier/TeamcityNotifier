namespace TeamCityNotifierWindowsStore.DataModel
{
    using TeamcityNotifier;

    public class ServerConfiguration : Common.BindableBase, IRestConfiguration
    {
        private string baseUrl;

        private string password;

        private string name;

        private string userName;

        private bool isServerOn;

        public ServerConfiguration(string baseUrl, string userName, string password, string serverName, bool isServerOn)
        {
            this.BaseUrl = baseUrl;
            this.UserName = userName;
            this.Password = password;
            this.Name = serverName;
            this.IsServerOn = isServerOn;
        }

        public string Name
        {
            get { return this.name; }
            set { this.SetProperty(ref this.name, value); }
        }

        public string Password
        {
            get { return this.password; }
            set { this.SetProperty(ref this.password, value); }
        }

        public string BaseUrl
        {
            get { return this.baseUrl; }
            set { this.SetProperty(ref this.baseUrl, value); }
        }

        public string UserName
        {
            get { return this.userName; }
            set { this.SetProperty(ref this.userName, value); }
        }

        public bool IsServerOn
        {
            get { return this.isServerOn; }
            set { this.SetProperty(ref this.isServerOn, value); }
        }
    }
}