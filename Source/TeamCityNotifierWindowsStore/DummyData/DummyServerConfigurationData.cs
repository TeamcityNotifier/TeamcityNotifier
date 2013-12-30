namespace TeamCityNotifierWindowsStore.DummyData
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;

    using TeamCityNotifierWindowsStore.DataModel;

    public class DummyServerConfigurationData
    {
        private readonly ServerConfiguration serverConfiguration;

        public DummyServerConfigurationData()
        {
            this.serverConfiguration = new ServerConfiguration(
                "https://teamcity.bbv.ch/", 
                "teamcitynotifier_test", 
                "j9nufrE6", 
                "My first Server", 
                true);
        }

        public ServerConfiguration ServerConfiguration
        {
            get
            {
                return this.serverConfiguration;
            }
        }
    }
}