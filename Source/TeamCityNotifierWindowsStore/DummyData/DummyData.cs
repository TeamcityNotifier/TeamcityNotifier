namespace TeamCityNotifierWindowsStore.DummyData
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;

    using TeamCityNotifierWindowsStore.Data;
    using TeamCityNotifierWindowsStore.DataModel;

    public class DummyData
    {
        private readonly ObservableCollection<ServerPMod> allGroups;

        public DummyData()
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
                DataService.PathSuccessfulPicture,
                "description build definition",
                "url build defintion");
        }

        public static ProjectPMod GetDummyProjectPMod(PModBase serverPMod)
        {
            var projectPMod = new ProjectPMod(
                Guid.NewGuid().ToString(),
                "title project",
                DataService.PathSuccessfulPicture,
                "description project",
                "url project",
                serverPMod);
            
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
                DataService.PathSuccessfulPicture, 
                "description server");

            serverPMod.Projects.Add(GetDummyProjectPMod(serverPMod));
            serverPMod.Projects.Add(GetDummyProjectPMod(serverPMod));
            serverPMod.Projects.Add(GetDummyProjectPMod(serverPMod));
            serverPMod.Projects.First().Projects.Add(GetDummyProjectPMod(serverPMod.Projects.First()));

            return serverPMod;
        }
    }
}