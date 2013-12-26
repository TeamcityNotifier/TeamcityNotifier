namespace TeamCityNotifierWindowsStore.DummyData
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;

    using TeamCityNotifierWindowsStore.Data;
    using TeamCityNotifierWindowsStore.DataModel;

    public class DummyData
    {
        public ObservableCollection<ServerPMod> AllGroups
        {
            get
            {
                return new ObservableCollection<ServerPMod> { this.GetDummyServerPMod() };
            }
        }

        public BuildDefinitionPMod GetDummyBuildDefinitionPMod()
        {
            return new BuildDefinitionPMod(
                Guid.NewGuid().ToString(),
                "title build definition",
                DataSourceService.PathSuccessfulPicture,
                "description build definition",
                "url build defintion");
        }

        public ProjectPMod GetDummyProjectPMod(SampleDataCommon serverPMod)
        {
            var projectPMod = new ProjectPMod(
                Guid.NewGuid().ToString(),
                "title project",
                DataSourceService.PathSuccessfulPicture,
                "description project",
                "url project",
                serverPMod);
            
            projectPMod.BuildDefinitions.Add(this.GetDummyBuildDefinitionPMod());
            projectPMod.BuildDefinitions.Add(this.GetDummyBuildDefinitionPMod());
            projectPMod.BuildDefinitions.Add(this.GetDummyBuildDefinitionPMod());

            return projectPMod;
        }

        public ServerPMod GetDummyServerPMod()
        {
            var serverPMod  = new ServerPMod(
                Guid.NewGuid().ToString(), 
                "title server", 
                DataSourceService.PathSuccessfulPicture, 
                "description server");

            serverPMod.Items.Add(this.GetDummyProjectPMod(serverPMod));
            serverPMod.Items.Add(this.GetDummyProjectPMod(serverPMod));
            serverPMod.Items.Add(this.GetDummyProjectPMod(serverPMod));
            serverPMod.Items.First().Items.Add(this.GetDummyProjectPMod(serverPMod.Items.First()));

            return serverPMod;
        }
    }
}