namespace TeamcityNotifier
{
    using System.Collections.Generic;
    using System.Linq;

    using TeamcityNotifier.Wrapper;

    public class RestFactory : IFactory
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
                var server = new Server(wrapperFactory, configuration);

                server.Projects = this.GetProjectRepository(server).Projects;

                servers.Add(server);
            }

            return servers;
        }

        public IProjectRepository GetProjectRepository(IServer server)
        {
            var restConsumer = server.RestConsumer;

            var projectRepository = new ProjectRepository(HttpRelativeUrl.PROJECT_URL);

            restConsumer.Load(projectRepository);

            foreach (var project in projectRepository.Projects)
            {
                LoadBuilds(server, project);
                foreach (var childProject in project.ChildProjects)
                {
                    LoadBuilds(server, childProject);
                }
            }

            return projectRepository;
        }

        private void LoadBuilds(IServer server, IProject project)
        {
            foreach (var buildDefinition in project.BuildDefinitions)
            {
                buildDefinition.LastBuild = GetBuildRepository(server, buildDefinition).Builds.FirstOrDefault();
            }
        }

        public IBuildRepository GetBuildRepository(IServer server, IBuildDefinition buildDefinition)
        {
            var restConsumer = server.RestConsumer;

            var buildRepository = new BuildRepository(buildDefinition.BuildRepositoryUrl);

            restConsumer.Load(buildRepository);

            return buildRepository;
        }
    }
}
