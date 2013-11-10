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
                var server = new Server(wrapperFactory.CreateUri(configuration.BaseUrl), configuration.UserName, configuration.Password);
                server.Projects = this.CreateProjects(server);
                server.Name = configuration.BaseUrl;
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


            foreach (var restProject in restProjects.project)
            {
                var projectUrl = this.wrapperFactory.CreateUri(server.Uri, restProject.href);
                var project = this.Get<project>(projectUrl, httpClient);

                if (project != null)
                {
                    projects.Add(new Project(project));
                }
            }

            return projects;
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