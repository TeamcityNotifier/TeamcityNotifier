namespace TeamcityNotifier
{
    using System.Collections.Generic;

    public class Service : IService
    {
        private IRestFactory restFactory;

        public Service(IRestFactory restFactory)
        {
            this.restFactory = restFactory;
        }

        public IEnumerable<IServer> GetServers()
        {
            return this.restFactory.CreateServers();
        }
    }
}