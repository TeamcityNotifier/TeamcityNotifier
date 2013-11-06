using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.ApplicationModel.Resources.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml.Data;

// The data model defined by this file serves as a representative example of a strongly-typed
// model that supports notification when members are added, removed, or modified.  The property
// names chosen coincide with data bindings in the standard item templates.
//
// Applications may use this model as a starting point and build on it, or discard it entirely and
// replace it with something appropriate to their needs.

namespace TeamCityNotifierWindowsStore.Data
{
    using TeamcityNotifier;
    using TeamcityNotifier.Wrapper;

    /// <summary>
    /// Creates a collection of groups and items with hard-coded content.
    /// 
    /// DataSourceService initializes with placeholder data rather than live production
    /// data so that sample data is provided at both design-time and run-time.
    /// </summary>
    public sealed class DataSourceService
    {
        private static DataSourceService dataSourceService = new DataSourceService();

        private ObservableCollection<ServerPMod> _allGroups = new ObservableCollection<ServerPMod>();
        public ObservableCollection<ServerPMod> AllGroups
        {
            get { return this._allGroups; }
        }

        public static IEnumerable<ServerPMod> GetGroups(string uniqueId)
        {
            if (!uniqueId.Equals("AllGroups")) throw new ArgumentException("Only 'AllGroups' is supported as a collection of groups");
            
            return dataSourceService.AllGroups;
        }

        public static ServerPMod GetServer(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            var matches = dataSourceService.AllGroups.Where((group) => group.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public static ProjectPMod GetProject(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            var matches = dataSourceService.AllGroups.SelectMany(group => group.Items).Where((item) => item.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public DataSourceService()
        {
            String ITEM_CONTENT = String.Format("Item Content: {0}\n\n{0}\n\n{0}\n\n{0}\n\n{0}\n\n{0}\n\n{0}",
                        "Curabitur class aliquam vestibulum nam curae maecenas sed integer cras phasellus suspendisse quisque donec dis praesent accumsan bibendum pellentesque condimentum adipiscing etiam consequat vivamus dictumst aliquam duis convallis scelerisque est parturient ullamcorper aliquet fusce suspendisse nunc hac eleifend amet blandit facilisi condimentum commodo scelerisque faucibus aenean ullamcorper ante mauris dignissim consectetuer nullam lorem vestibulum habitant conubia elementum pellentesque morbi facilisis arcu sollicitudin diam cubilia aptent vestibulum auctor eget dapibus pellentesque inceptos leo egestas interdum nulla consectetuer suspendisse adipiscing pellentesque proin lobortis sollicitudin augue elit mus congue fermentum parturient fringilla euismod feugiat");
            var configuration = new RestConfiguration("https://teamcity.bbv.ch/", "teamcitynotifier_test", "j9nufrE6");

            var service = new Service(new RestFactory(new List<IRestConfiguration> {configuration}, new WrapperFactory()));

            foreach (var server in service.GetServers())
            {
                var serverPMod = new ServerPMod(
                    server.Name,
                    server.Name,
                    "Group Subtitle: 1",
                    "Assets/Green.png",
                    "Group Description: Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus tempor scelerisque lorem in vehicula. Aliquam tincidunt, lacus ut sagittis tristique, turpis massa volutpat augue, eu rutrum ligula ante a ante");

                foreach (var project in server.Projects)
                {
                    serverPMod.Items.Add(new ProjectPMod(
                        project.Name,
                        project.Name,
                        "Project Item Subtitle: 1",
                        "Assets/Green.png",
                        project.Description,
                        ITEM_CONTENT,
                        serverPMod));
                }

                this.AllGroups.Add(serverPMod);
            }
        }
    }
}
