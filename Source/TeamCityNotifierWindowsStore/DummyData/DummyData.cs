namespace TeamCityNotifierWindowsStore.DummyData
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;

    using TeamCityNotifierWindowsStore.Data;
    using TeamCityNotifierWindowsStore.DataModel;

    public class DummyData
    {
        public DummyData()
        {
            this.allGroups = new ObservableCollection<ServerPMod> { GetDummyServerPMod() };
        }

        private readonly ObservableCollection<ServerPMod> allGroups;

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
                DataSourceService.PathSuccessfulPicture,
                "description build definition",
                "url build defintion");
        }

        public static ProjectPMod GetDummyProjectPMod(SampleDataCommon serverPMod)
        {
            var projectPMod = new ProjectPMod(
                Guid.NewGuid().ToString(),
                "title project",
                DataSourceService.PathSuccessfulPicture,
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
                DataSourceService.PathSuccessfulPicture, 
                "description server");

            serverPMod.Items.Add(GetDummyProjectPMod(serverPMod));
            serverPMod.Items.Add(GetDummyProjectPMod(serverPMod));
            serverPMod.Items.Add(GetDummyProjectPMod(serverPMod));
            serverPMod.Items.First().Items.Add(GetDummyProjectPMod(serverPMod.Items.First()));

            return serverPMod;
        }
    }
}