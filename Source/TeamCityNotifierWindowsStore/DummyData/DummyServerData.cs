namespace TeamCityNotifierWindowsStore.DummyData
{
    using System.Collections.ObjectModel;
    using System.Linq;

    using TeamcityNotifier;
    using TeamcityNotifier.RestObject;

    public class DummyServerData
    {
        private readonly ObservableCollection<IServer> allGroups;

        private readonly ObservableCollection<IProject> allProjects; 

        public DummyServerData()
        {
            var dummyServer = GetDummyServer();
            this.allGroups = new ObservableCollection<IServer> { dummyServer, dummyServer };

            var dummyProject = GetDummyProject();
            this.allProjects = new ObservableCollection<IProject> { dummyProject, dummyProject };

        }

        public ObservableCollection<IServer> AllGroups
        {
            get
            {
                return this.allGroups;
            }
        }

        public static IServer GetDummyServer()
        {
            var server = new DummyServer { Name = "serverName", Status = Status.Success };
            server.RootProject = GetDummyProject();
            server.RootProject.AddChild(GetDummyProject());
            server.RootProject.AddChild(GetDummyProject());
            server.RootProject.AddChild(GetDummyProject());
            server.RootProject.ChildProjects.First().AddChild(GetDummyProject());
            return server;
        }

        public static IProject GetDummyProject()
        {
            var project = new DummyProject { Name = "projectName", Status = Status.Success, Description = "projectDescription" };
            project.BuildDefinitions.Add(GetDummyBuildDefinition());
            project.BuildDefinitions.Add(GetDummyBuildDefinition());
            project.BuildDefinitions.Add(GetDummyBuildDefinition());

            return project;
        }

        public static IBuildDefinition GetDummyBuildDefinition()
        {
            return new DummyBuildDefinition { Name = "buildDefinitionName", Description = "buildDefinitionDescription", Status = Status.Error };
        }
    }
}