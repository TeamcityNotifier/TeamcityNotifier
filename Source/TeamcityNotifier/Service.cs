namespace TeamcityNotifier
{
    using System.Collections.Generic;

    public class Service : IService
    {
        private readonly IRestFactory restFactory;

        private readonly List<IUpdater> updaters;

        public Service(IRestFactory restFactory)
        {
            this.restFactory = restFactory;
            this.updaters = new List<IUpdater>();
        }

        public IEnumerable<IServer> GetServers()
        {
            return this.restFactory.CreateServers();
        }

        public void StartPeriodicallyUpdating(IEnumerable<IServer> servers)
        {
            foreach (var server in servers)
            {
                this.StartPeriodicallyUpdating(server);
            }  
        }

        public void StartPeriodicallyUpdating(IServer server)
        {
            var updater = new Updater(server.RestConsumer);
            this.updaters.Add(updater);

            foreach (var project in server.BuildRepository.Projects)
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