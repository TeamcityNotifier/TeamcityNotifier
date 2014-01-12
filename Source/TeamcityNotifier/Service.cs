namespace TeamcityNotifier
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public class Service : IService
    {
        private readonly IRestFactory restFactory;

        private readonly INetworkFactory networkFactory;

        private readonly List<IUpdater> updaters;

        public Service(IRestFactory restFactory, INetworkFactory networkFactory)
        {
            this.restFactory = restFactory;
            this.networkFactory = networkFactory;
            this.updaters = new List<IUpdater>();
        }

        public ObservableCollection<IServer> GetServers()
        {
            return this.restFactory.CreateServers();
        }

        public void StartPeriodicallyUpdating(ObservableCollection<IServer> servers)
        {
            foreach (var server in servers)
            {
                this.StartPeriodicallyUpdating(server);
            }  
        }

        public void StartPeriodicallyUpdating(IServer server)
        {
            var updater = networkFactory.CreateUpdater(server.RestConsumer);
            this.updaters.Add(updater);

            foreach (var project in server.ProjectRepository.Projects)
            {
                foreach (var buildDefinition in project.BuildDefinitions)
                {
                    updater.Register(buildDefinition.BuildRepository);
                }
            }

            updater.Start();
        }
    }
}