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

    /// <summary>
    /// Creates a collection of groups and items with hard-coded content.
    /// 
    /// DataService initializes with placeholder data rather than live production
    /// data so that sample data is provided at both design-time and run-time.
    /// </summary>
    public static class DataService
    {
        public const string PathFailedPicture = "Assets/Red.png";
        public const string PathSuccessfulPicture = "Assets/Green.png";

        public static ObservableCollection<ServerPMod> AllServers { get; private set; }


        public static IEnumerable<ServerPMod> GetServers(string uniqueId)
        {
            if (!uniqueId.Equals("AllServers"))
            {
                throw new ArgumentException("Only 'AllServers' is supported as a collection of groups");
            }

            return AllServers;
        }

        public static ServerPMod GetServer(string uniqueId)
        {
            return AllServers.SingleOrDefault(server => server.UniqueId.Equals(uniqueId));
        }

        public static ProjectPMod GetProject(string uniqueId)
        {
            var allProjects = new List<ProjectPMod>();

            var firstLevelProjects = AllServers.SelectMany(group => group.Projects).ToList();
            allProjects.AddRange(firstLevelProjects);

            var allSubProjects = GetSubProjects(firstLevelProjects);
            if (allSubProjects != null)
            {
                allProjects.AddRange(allSubProjects);
            }

            return allProjects.SingleOrDefault((item) => item.UniqueId.Equals(uniqueId));
        }

        private static IEnumerable<ProjectPMod> GetSubProjects(IEnumerable<ProjectPMod> projects)
        {
            var subProjects = new List<ProjectPMod>();

            foreach (var project in projects.Where(project => project.Projects != null))
            {
                subProjects.AddRange(project.Projects);
                var subSubProjects = GetSubProjects(project.Projects);
                if (subSubProjects != null)
                {
                    subProjects.AddRange(subSubProjects);
                }
            }

            return subProjects;
        }

        public static void LoadData()
        {
            AllServers = new ObservableCollection<ServerPMod>();
            var serverConfiguration = GetServerConfiguration();

            var service =
                new Service(new RestFactory(new List<IRestConfiguration> { serverConfiguration }, new WrapperFactory()));

            IEnumerable<IServer> servers = new List<IServer>();

            try
            {
                servers = service.GetServers();
            }
            catch (Exception)
            {
                return;
            }

            foreach (var server in servers)
            {
                var serverPMod = new ServerPMod(Guid.NewGuid().ToString(), server.Name, PathSuccessfulPicture, string.Empty);

                foreach (var project in server.RootProject.ChildProjects)
                {
                    serverPMod.Projects.Add(CreateProjectPMod(project, serverPMod));
                }

                AllServers.Add(serverPMod);
            }
        }

        public static ServerConfiguration GetServerConfiguration()
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            if (localSettings.Values.ContainsKey("BaseUrl") 
                && localSettings.Values.ContainsKey("UserName") 
                && localSettings.Values.ContainsKey("Password") 
                && localSettings.Values.ContainsKey("ServerName")
                && localSettings.Values.ContainsKey("ServerOnOff"))
            {
                var baseUrl = localSettings.Values["BaseUrl"].ToString();
                var userName = localSettings.Values["UserName"].ToString();
                var password = localSettings.Values["Password"].ToString();
                var serverName = localSettings.Values["ServerName"].ToString();
                var serverOnOff = (bool)localSettings.Values["ServerOnOff"];

                return new ServerConfiguration(baseUrl, userName, password, serverName, serverOnOff);
            }

            return null;
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
