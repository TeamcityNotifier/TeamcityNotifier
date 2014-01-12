namespace TeamcityNotifier
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using TeamcityNotifier.RestObject;
    using TeamcityNotifier.Wrapper;

    public class RestFactory : IRestFactory
    {
        private readonly IEnumerable<IRestConfiguration> configurations;
        private readonly IWrapperFactory wrapperFactory;
        private readonly INetworkFactory networkFactory;

        public RestFactory(IEnumerable<IRestConfiguration> configurations, IWrapperFactory wrapperFactory, INetworkFactory networkFactory)
        {
            this.configurations = configurations;
            this.wrapperFactory = wrapperFactory;
            this.networkFactory = networkFactory;
        }

        public ObservableCollection<IServer> CreateServers()
        {
            var servers = new ObservableCollection<IServer>();

            foreach (var configuration in this.configurations)
            {
                try
                {
                    if (IsValid(configuration) && configuration.IsServerOn)
                    {
                        var serverBaseUri = wrapperFactory.CreateUri(configuration.BaseUrl);

                        var restConsumer = networkFactory.CreateRestConsumer(
                            serverBaseUri,
                            wrapperFactory.CreateHttpClientHandler(configuration.UserName, configuration.Password),
                            wrapperFactory);

                        var server = new Server(configuration.Name, serverBaseUri, restConsumer);

                        server.ProjectRepository = this.GetProjectRepository(server);

                        servers.Add(server);
                    }
                }
                catch
                {
                }
            }

            return servers;
        }

        public IProjectRepository GetProjectRepository(IServer server)
        {
            var projectRepository = new ProjectRepository(HttpRelativeUrl.PROJECT_URL);

            server.RestConsumer.Load(projectRepository);

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
