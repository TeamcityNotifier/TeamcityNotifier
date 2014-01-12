namespace TeamCityNotifierWindowsStore.DummyData
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Net.Http;

    using TeamcityNotifier;

    using TeamCityNotifierWindowsStore.DataModel;

    using TeamcityNotifier.RestObject;
    using TeamcityNotifier.Wrapper;

    public class DummyServerData
    {
        private readonly ObservableCollection<IServer> allGroups;

        public DummyServerData()
        {
            this.allGroups = new ObservableCollection<IServer> {  };
        }

        public ObservableCollection<IServer> AllGroups
        {
            get
            {
                return this.allGroups;
            }
        }

//        public static IBuildDefinition GetDummyBuildDefinition()
//        {
//            return new BuildDefinition("buildDefinitionUrl");
//        }

//        public static IProject GetDummyProject()
//        {
//            var project = new Project("projectUrl", n);
//            
//            project.BuildDefinitions.Add(GetDummyBuildDefinition());
//            project.BuildDefinitions.Add(GetDummyBuildDefinition());
//            project.BuildDefinitions.Add(GetDummyBuildDefinition());
//
//            return project;
//        }

//        public static IServer GetDummyServer()
//        {
//            var server  = new Server("serverName", new UriWrapper("serverUrl"), 
//                new RestConsumer(new UriWrapper("uri"), new HttpClient(), new WrapperFactory()));
//
//            server.RootProject.ChildProjects.Add(GetDummyProject());
//            server.RootProject.ChildProjects.Add(GetDummyProject());
//            server.RootProject.ChildProjects.Add(GetDummyProject());
//            server.RootProject.ChildProjects.First().ChildProjects.Add(GetDummyProject());
//
//            return server;
//        }
    }
}