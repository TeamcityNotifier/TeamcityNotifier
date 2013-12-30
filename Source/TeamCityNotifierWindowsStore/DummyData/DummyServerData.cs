namespace TeamCityNotifierWindowsStore.DummyData
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;

    using TeamcityNotifier;

    using TeamCityNotifierWindowsStore.DataModel;

    public class DummyServerData
    {
        private readonly ObservableCollection<ServerPMod> allGroups;

        public DummyServerData()
        {
            this.allGroups = new ObservableCollection<ServerPMod> { GetDummyServerPMod() };
        }

        public ObservableCollection<ServerPMod> AllGroups
        {
            get
            {
                return this.allGroups;
            }
        }

        public static BuildDefinitionPMod GetDummyBuildDefinitionPMod()
        {
            return new BuildDefinitionPMod(
                Guid.NewGuid().ToString(),
                "title build definition",
                "description build definition",
                "url build defintion", 
                Status.Success);
        }

        public static ProjectPMod GetDummyProjectPMod(ServerEntityBase serverServerEntity)
        {
            var projectPMod = new ProjectPMod(
                Guid.NewGuid().ToString(),
                "title project",
                "description project",
                "url project",
                serverServerEntity, 
                Status.Success);
            
            projectPMod.BuildDefinitions.Add(GetDummyBuildDefinitionPMod());
            projectPMod.BuildDefinitions.Add(GetDummyBuildDefinitionPMod());
            projectPMod.BuildDefinitions.Add(GetDummyBuildDefinitionPMod());

            return projectPMod;
        }

        public static ServerPMod GetDummyServerPMod()
        {
            var serverPMod  = new ServerPMod(
                Guid.NewGuid().ToString(), 
                "title server", 
                "description server", 
                Status.Success);

            serverPMod.Projects.Add(GetDummyProjectPMod(serverPMod));
            serverPMod.Projects.Add(GetDummyProjectPMod(serverPMod));
            serverPMod.Projects.Add(GetDummyProjectPMod(serverPMod));
            serverPMod.Projects.First().Projects.Add(GetDummyProjectPMod(serverPMod.Projects.First()));

            return serverPMod;
        }
    }
}