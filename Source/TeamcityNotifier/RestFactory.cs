namespace TeamcityNotifier
{
    using System.Collections.Generic;
    using System.Linq;

    using TeamcityNotifier.Wrapper;

    public class RestFactory : IRestFactory
    {
        private readonly IEnumerable<IRestConfiguration> configurations;
        private readonly IWrapperFactory wrapperFactory;

        public RestFactory(IEnumerable<IRestConfiguration> configurations, IWrapperFactory wrapperFactory)
        {
            this.configurations = configurations;
            this.wrapperFactory = wrapperFactory;
        }

        public IEnumerable<IServer> CreateServers()
        {
            var servers = new List<IServer>();

            foreach (var configuration in this.configurations)
            {
                if (IsValid(configuration))
                {
                    var server = new Server(wrapperFactory, configuration);

                    server.Projects = this.GetProjectRepository(server).Projects;

                    servers.Add(server);
                }
            }

            return servers;
        }

        public IProjectRepository GetProjectRepository(IServer server)
        {
            var restConsumer = server.RestConsumer;

            var projectRepository = new ProjectRepository(HttpRelativeUrl.PROJECT_URL);

            restConsumer.Load(projectRepository);

            return projectRepository;
        }

        private bool IsValid(IRestConfiguration configuration)
        {
            return configuration.BaseUrl != string.Empty
                && configuration.UserName != string.Empty
                && configuration.Password != string.Empty;
        }

    }
}
