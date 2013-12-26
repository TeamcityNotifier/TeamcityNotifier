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
    using TeamCityNotifierWindowsStore.DataModel;

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
        static int serverCount = 1;
        static int projectCount = 10;
        private static bool succeedToggler = false;

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

            var firstLevelProjects = dataSourceService.AllGroups.SelectMany(group => group.Items).ToList();
            var temp = firstLevelProjects.SelectMany(items => items.Items).Union(firstLevelProjects);

            var matches = temp.Where((item) => item.UniqueId.Equals(uniqueId));
            
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public DataSourceService()
        {
            var configuration = new RestConfiguration("https://teamcity.bbv.ch/", "teamcitynotifier_test", "j9nufrE6", "My first Server");
            var configuration2 = new RestConfiguration("https://teamcity.bbv.ch/", "teamcitynotifier_test", "j9nufrE6", "My first Server");

            var service = new Service(new RestFactory(new List<IRestConfiguration> {configuration, configuration2}, new WrapperFactory()));

            foreach (var server in service.GetServers())
            {
                var serverPMod = new ServerPMod(
                    serverCount.ToString(),
                    server.Name + serverCount,
                    "my server sub title",
                    "Assets/Green.png",
                    "my server description");

                serverCount++;

                foreach (var project in server.RootProject.ChildProjects)
                {
                     serverPMod.Items.Add(CreateProjectPMod(project, serverPMod));
                }

                this.AllGroups.Add(serverPMod);
            }
        }

        private static ProjectPMod CreateProjectPMod(IProject project, SampleDataCommon serverPMod)
        {
            ProjectPMod projectPMod;

            if (succeedToggler)
            {
                projectPMod = new ProjectPMod(
                    projectCount.ToString(),
                    project.Name + projectCount,
                    "my project sub title",
                    "Assets/Green.png",
                    project.Description,
                    "my project content",
                    serverPMod,
                    succeedToggler);

                foreach (var buildDefinition in project.BuildDefinitions)
                {
                    projectPMod.BuildDefinitions.Add(new BuildDefinitionPMod(
                        buildDefinition.Id, 
                        buildDefinition.Name, 
                        "subtitel builddefinition",
                        "Assets/Green.png",
                        buildDefinition.Description,
                        buildDefinition.Url));
                }

                succeedToggler = false;
            }
            else
            {
                projectPMod = new ProjectPMod(
                    projectCount.ToString(),
                    project.Name + projectCount,
                    "my project sub title",
                    "Assets/Red.png",
                    project.Description,
                    "my project content",
                    serverPMod,
                    succeedToggler);

                foreach (var buildDefinition in project.BuildDefinitions)
                {
                    projectPMod.BuildDefinitions.Add(new BuildDefinitionPMod(
                        buildDefinition.Id,
                        buildDefinition.Name,
                        "subtitel builddefinition",
                        "Assets/Green.png",
                        buildDefinition.Description, 
                        buildDefinition.Url));
                }

                succeedToggler = true;
            }

            projectCount++;

            foreach (var childProject in project.ChildProjects)
            {
                projectPMod.Items.Add(CreateProjectPMod(childProject, projectPMod));
            }

            return projectPMod;
        }
    }
}
