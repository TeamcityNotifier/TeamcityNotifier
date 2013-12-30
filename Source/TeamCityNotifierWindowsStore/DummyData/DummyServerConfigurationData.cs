namespace TeamCityNotifierWindowsStore.DummyData
{
    using System.Collections.ObjectModel;

    using TeamCityNotifierWindowsStore.DataModel;

    public class DummyServerConfigurationData
    {
        private readonly ObservableCollection<ServerConfigurationPMod> serverConfigurationPMods;

        public DummyServerConfigurationData()
        {
            this.serverConfigurationPMods = new ObservableCollection<ServerConfigurationPMod>();

            for (int i = 0; i < 3; i++)
            {
                this.serverConfigurationPMods.Add(new ServerConfigurationPMod(
                    "https://teamcity.bbv.ch/",
                    "teamcitynotifier_test",
                    "j9nufrE6",
                    "My first Server",
                    true));
            }
        }

        public ObservableCollection<ServerConfigurationPMod> ServerConfigurationPMods
        {
            get
            {
                return this.serverConfigurationPMods;
            }
        }
    }
}