namespace TeamCityRestClient
{
    using System.Collections.Generic;

    public class Service : IService
    {
        private IConfiguration configuration;

        public Service(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public IEnumerable<IServer> GetServers()
        {
            throw new System.NotImplementedException();
        }
    }
}