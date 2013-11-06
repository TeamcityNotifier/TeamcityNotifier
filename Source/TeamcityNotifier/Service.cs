namespace TeamcityNotifier
{
    using System.Collections.Generic;

    public class Service : IService
    {private IFactory factory;

        public Service(IFactory factory)
        {
            this.factory = factory;
        }

        public IEnumerable<IServer> GetServers()
        {
            return this.factory.CreateServers();
        }
    }
}