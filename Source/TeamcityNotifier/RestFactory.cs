namespace TeamcityNotifier
{
    using System.Collections.Generic;

    using Wrapper;

    using xsdtest;

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
                server.Projects = this.CreateProjects(server);
                servers.Add(server);
            }

            return servers;
        }

        public IEnumerable<IProject> CreateProjects(IServer server)
        {
            var httpClient = this.wrapperFactory.CreateHttpClientHandler(server.UserName, server.Password);
            var url = this.wrapperFactory.CreateUri(server.Uri, HttpRelativeUrl.PROJECT_URL);
            var restProjects = Get<projects1>(url, httpClient);
            var projects = new List<IProject>();

            foreach (var projectRef in restProjects.project)
            {
                var projectUrl = this.wrapperFactory.CreateUri(server.Uri, projectRef.href);
                var restProject = this.Get<project>(projectUrl, httpClient);

                if (restProject != null)
                {
                    var project = new Project(restProject);
                    project.BuildDefinitions = this.CreateBuildDefinitions(server, project);
                    
                    projects.Add(project);
                }
            }

            return projects;
        }

        public IEnumerable<IBuildDefinition> CreateBuildDefinitions(IServer server, IProject project)
        {
            var httpClient = this.wrapperFactory.CreateHttpClientHandler(server.UserName, server.Password);
            var buildTypeUrl = this.wrapperFactory.CreateUri(server.Uri, project.Href);
            var restProject = this.Get<project>(buildTypeUrl, httpClient); 

            var buildDefinitions = new List<IBuildDefinition>();

            foreach (var buildType in restProject.buildTypes)
            {
                buildTypeUrl = this.wrapperFactory.CreateUri(server.Uri, buildType.href);
                var buildTypeRest = Get<buildType>(buildTypeUrl, httpClient);

                if (buildTypeRest != null)
                {
                    buildDefinitions.Add(new BuildDefinition(buildTypeRest));
                }
            }

            return buildDefinitions;
        }

        private T Get<T>(IUri url, IHttpClient httpClient)
        {
            var asyncTask = httpClient.GetStringAsync(url);

            var serializer = this.wrapperFactory.CreateXmlSerializer(typeof(T));

            using (var reader = this.wrapperFactory.CreateStringReader(asyncTask.Result))
            {
                var objectDto = (T)serializer.Deserialize(reader);

                return objectDto;
            }
        }
    }
}