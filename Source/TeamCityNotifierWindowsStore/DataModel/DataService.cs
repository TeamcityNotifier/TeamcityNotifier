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

        public static string BaseUrlKey = "BaseUrlKey";

        public static string UserNameKey = "UserNameKey";

        public static string PasswordKey = "Password";

        public static string ServerNameKey = "ServerName";

        public static string IsServerOnKey = "IsServerOn";

        public static string ServerContainerKeyPrefix = "Server ";

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
            var serverConfigurations = GetServerConfigurations();

            var service = new Service(new RestFactory(serverConfigurations , new WrapperFactory()));

            IEnumerable<IServer> servers = new List<IServer>();

            try
            {
                servers = service.GetServers();
            }
            catch (Exception)
            {
                return;
            }

            for (int i = 0; i < servers.Count(); i++)
            {
                var server = servers.ToList()[i];

                if (serverConfigurations[i].IsServerOn)
                {
                    var serverPMod = new ServerPMod(Guid.NewGuid().ToString(), server.Name, PathSuccessfulPicture, string.Empty);

                    foreach (var project in server.RootProject.ChildProjects)
                    {
                        serverPMod.Projects.Add(CreateProjectPMod(project, serverPMod));
                    }

                    AllServers.Add(serverPMod);
                }
            }
        }

        public static ObservableCollection<ServerConfigurationPMod> GetServerConfigurations()
        {
            var serverConfigurations = new ObservableCollection<ServerConfigurationPMod>();

            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

            for (int i = 0; i < localSettings.Containers.Count; i++)
            {
                var serverContainerKey = ServerContainerKeyPrefix + i;

                if (localSettings.Containers.ContainsKey(serverContainerKey))
                {
                    string baseUrl = string.Empty;
                    string userName = string.Empty;
                    string password = string.Empty;
                    string serverName = string.Empty;
                    bool isServerOn = false;

                    var container = localSettings.Containers[serverContainerKey];


                    if (container.Values.ContainsKey(BaseUrlKey))
                    {
                        baseUrl = container.Values[BaseUrlKey].ToString();
                    }

                    if (container.Values.ContainsKey(UserNameKey))
                    {
                        userName = container.Values[UserNameKey].ToString();
                    }
                        
                    if (container.Values.ContainsKey(PasswordKey))
                    {
                        password = container.Values[PasswordKey].ToString();
                    }
                        
                    if (container.Values.ContainsKey(ServerNameKey))
                    {
                        serverName = container.Values[ServerNameKey].ToString(); 
                    }
                        
                    if(container.Values.ContainsKey(IsServerOnKey))
                    {
                        isServerOn = (bool)container.Values[IsServerOnKey];
                    }
                        
                    serverConfigurations.Add(
                        new ServerConfigurationPMod(baseUrl, userName, password, serverName, isServerOn));
                }
            }

            if (serverConfigurations.Count < 3)
            {
                var emptyServerConfigurations = 3 - serverConfigurations.Count;

                for (int i = 0; i < emptyServerConfigurations; i++)
                {
                    serverConfigurations.Add(new ServerConfigurationPMod(string.Empty, string.Empty, string.Empty, string.Empty, false));
                }
            }

            return serverConfigurations;
        }

        private static ProjectPMod CreateProjectPMod(IProject project, ServerEntityBase serverServerEntity)
        {
            var projectPMod = new ProjectPMod(
                Guid.NewGuid().ToString(),
                project.Name,
                PathSuccessfulPicture,
                project.Description,
                string.Empty,
                serverServerEntity);

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
                                    serverServerEntity.SetImage(PathFailedPicture);
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
