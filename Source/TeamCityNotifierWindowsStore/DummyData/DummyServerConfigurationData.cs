namespace TeamCityNotifierWindowsStore.DummyData
{
    using System.Collections.ObjectModel;

    using TeamCityNotifierWindowsStore.DataModel;

    public class DummyServerConfigurationData
    {
        private readonly ObservableCollection<ServerConfiguration> serverConfigurations;

        public DummyServerConfigurationData()
        {
            this.serverConfigurations = new ObservableCollection<ServerConfiguration>();

            for (int i = 0; i < 3; i++)
            {
                this.serverConfigurations.Add(new ServerConfiguration(
                    "https://teamcity.bbv.ch/",
                    "teamcitynotifier_test",
                    "j9nufrE6",
                    "My first Server",
                    true));
            }
        }

        public ObservableCollection<ServerConfiguration> ServerConfigurations
        {
            get
            {
                return this.serverConfigurations;
            }
        }
    }
}