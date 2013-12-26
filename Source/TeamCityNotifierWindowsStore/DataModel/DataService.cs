// The data model defined by this file serves as a representative example of a strongly-typed
// model that supports notification when members are added, removed, or modified.  The property
// names chosen coincide with data bindings in the standard item templates.
//
// Applications may use this model as a starting point and build on it, or discard it entirely and
// replace it with something appropriate to their needs.
namespace TeamCityNotifierWindowsStore.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using TeamcityNotifier;
    using TeamcityNotifier.Wrapper;

    using TeamCityNotifierWindowsStore.Data;

    /// <summary>
    /// Creates a collection of groups and items with hard-coded content.
    /// 
    /// DataService initializes with placeholder data rather than live production
    /// data so that sample data is provided at both design-time and run-time.
    /// </summary>
    public sealed class DataService
    {
        public const string PathFailedPicture = "Assets/Red.png";
        public const string PathSuccessfulPicture = "Assets/Green.png";

        private static readonly DataService dataService = new DataService();

        private readonly ObservableCollection<ServerPMod> allGroups = new ObservableCollection<ServerPMod>();

        public ObservableCollection<ServerPMod> AllGroups
        {
            get { return this.allGroups; }
        }

        public static IEnumerable<ServerPMod> GetServers(string uniqueId)
        {
            if (!uniqueId.Equals("AllGroups"))
            {
                throw new ArgumentException("Only 'AllGroups' is supported as a collection of groups");
            }

            return dataService.AllGroups;
        }

        public static ServerPMod GetServer(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            var matches = dataService.AllGroups.Where((group) => group.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public static ProjectPMod GetProject(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            var firstLevelProjects = dataService.AllGroups.SelectMany(group => group.Projects).ToList();
            var temp = firstLevelProjects.SelectMany(items => items.Projects).Union(firstLevelProjects);

            var matches = temp.Where((item) => item.UniqueId.Equals(uniqueId));
            
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public DataService()
        {
            var configuration = new RestConfigurationPMod("https://teamcity.bbv.ch/", "teamcitynotifier_test", "j9nufrE6", "My first Server");
            var configuration2 = new RestConfigurationPMod("https://teamcity.bbv.ch/", "teamcitynotifier_test", "j9nufrE6", "My first Server");

            var service = new Service(new RestFactory(new List<IRestConfiguration> {configuration, configuration2}, new WrapperFactory()));

            foreach (var server in service.GetServers())
            {
                var serverPMod = new ServerPMod(
                    Guid.NewGuid().ToString(),
                    server.Name,
                    PathSuccessfulPicture,
                    string.Empty);

                foreach (var project in server.RootProject.ChildProjects)
                {
                     serverPMod.Projects.Add(CreateProjectPMod(project, serverPMod));
                }

                this.AllGroups.Add(serverPMod);
            }
        }

        private static ProjectPMod CreateProjectPMod(IProject project, PModBase serverPMod)
        {
            var projectPMod = new ProjectPMod(
                Guid.NewGuid().ToString(),
                project.Name,
                PathSuccessfulPicture,
                project.Description,
                string.Empty,
                serverPMod);

                foreach (var buildDefinition in project.BuildDefinitions)
                {
                    var buildDefinitionPMod = new BuildDefinitionPMod(
                        buildDefinition.Id,
                        buildDefinition.Name,
                        PathSuccessfulPicture,
                        buildDefinition.Description,
                        buildDefinition.Url);

                    if (buildDefinition.LastBuild != null)
                    {
                        switch (buildDefinition.LastBuild.Status)
                        {
                            case Status.Success:
                                {
                                    break;
                                }
                            
                            case Status.Error:
                            case Status.Failure:
                                {
                                    serverPMod.SetImage(PathFailedPicture);
                                    projectPMod.SetImage(PathFailedPicture);
                                    buildDefinitionPMod.SetImage(PathFailedPicture);
                                    break;
                                }

                            default:
                                {
                                    throw new Exception("Build has an unknown state");
                                }
                        }
                    }

                    projectPMod.BuildDefinitions.Add(buildDefinitionPMod);
                }

            foreach (var childProject in project.ChildProjects)
            {
                projectPMod.Projects.Add(CreateProjectPMod(childProject, projectPMod));
            }

            return projectPMod;
        }
    }
}
