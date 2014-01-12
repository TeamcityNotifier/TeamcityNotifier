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
    using TeamcityNotifier.RestObject;
    using TeamcityNotifier.Wrapper;

    /// <summary>
    /// Creates a collection of groups and items with hard-coded content.
    /// 
    /// DataService initializes with placeholder data rather than live production
    /// data so that sample data is provided at both design-time and run-time.
    /// </summary>
    public static class DataService
    {
        public static string BaseUrlKey = "BaseUrlKey";

        public static string UserNameKey = "UserNameKey";

        public static string PasswordKey = "Password";

        public static string ServerNameKey = "ServerName";

        public static string IsServerOnKey = "IsServerOn";

        public static string ServerContainerKeyPrefix = "Server ";

        public static ObservableCollection<IServer> AllServers { get; private set; }

        public static ObservableCollection<IServer> GetServers(string uniqueId)
        {
            if (!uniqueId.Equals("AllServers"))
            {
                throw new ArgumentException("Only 'AllServers' is supported as a collection of groups");
            }

            return AllServers;
        }

        public static IServer GetServer(string uniqueId)
        {
            return AllServers.SingleOrDefault(server => server.UniqueId.ToString().Equals(uniqueId));
        }

        public static IProject GetProject(string uniqueId)
        {
            var allProjects = new List<IProject>();

            var firstLevelProjects = AllServers.SelectMany(server => server.RootProject.ChildProjects).ToList();
            allProjects.AddRange(firstLevelProjects);

            var allSubProjects = GetSubProjects(firstLevelProjects);
            if (allSubProjects != null)
            {
                allProjects.AddRange(allSubProjects);
            }

            return allProjects.SingleOrDefault((item) => item.UniqueId.ToString().Equals(uniqueId));
        }

        public static void LoadData()
        {
            AllServers = new ObservableCollection<IServer>();
            var serverConfigurations = GetServerConfigurations();
            var networkFactory = new NetworkFactory();

            IService service = new Service(new RestFactory(serverConfigurations, new WrapperFactory(), networkFactory), networkFactory);

            AllServers = service.GetServers();
            service.StartPeriodicallyUpdating(AllServers);
        }

        private static IEnumerable<IProject> GetSubProjects(IEnumerable<IProject> projects)
        {
            var subProjects = new List<IProject>();

            foreach (var project in projects.Where(project => project.ChildProjects != null))
            {
                subProjects.AddRange(project.ChildProjects);
                var subSubProjects = GetSubProjects(project.ChildProjects);
                if (subSubProjects != null)
                {
                    subProjects.AddRange(subSubProjects);
                }
            }

            return subProjects;
        }

        public static ObservableCollection<ServerConfiguration> GetServerConfigurations()
        {
            var serverConfigurations = new ObservableCollection<ServerConfiguration>();

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
                        
                    if (container.Values.ContainsKey(IsServerOnKey))
                    {
                        isServerOn = (bool)container.Values[IsServerOnKey];
                    }
                        
                    serverConfigurations.Add(
                        new ServerConfiguration(baseUrl, userName, password, serverName, isServerOn));
                }
            }

            FillUpWithEmptyServersIfLessThenThree(serverConfigurations);

            return serverConfigurations;
        }

        private static void FillUpWithEmptyServersIfLessThenThree(ObservableCollection<ServerConfiguration> serverConfigurations)
        {
            if (serverConfigurations.Count < 3)
            {
                var emptyServerConfigurations = 3 - serverConfigurations.Count;

                for (int i = 0; i < emptyServerConfigurations; i++)
                {
                    serverConfigurations.Add(
                        new ServerConfiguration(string.Empty, string.Empty, string.Empty, string.Empty, false));
                }
            }
        }
    }
}
