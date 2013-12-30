namespace TeamCityNotifierWindowsStore.DataModel
{
    using TeamcityNotifier;

    public class ServerConfigurationPMod : Common.BindableBase, IRestConfiguration
    {
        public ServerConfigurationPMod(string baseUrl, string userName, string password, string serverName, bool isServerOn)
        {
            this.BaseUrl = baseUrl;
            this.UserName = userName;
            this.Password = password;
            this.Name = serverName;
            this.IsServerOn = isServerOn;
        }

        private string baseUrl;

        public string BaseUrl
        {
            get { return this.baseUrl; }
            set { this.SetProperty(ref this.baseUrl, value); }
        }

        private string userName;

        public string UserName
        {
            get { return this.userName; }
            set { this.SetProperty(ref this.userName, value); }
        }

        private string password;

        public string Password
        {
            get { return this.password; }
            set { this.SetProperty(ref this.password, value); }
        }

        private string name;

        public string Name
        {
            get { return this.name; }
            set { this.SetProperty(ref this.name, value); }
        }

        private bool isServerOn;

        public bool IsServerOn
        {
            get { return this.isServerOn; }
            set { this.SetProperty(ref this.isServerOn, value); }
        }
    }
}