namespace TeamCityRestClient
{
    using System.Collections.Generic;

    public interface IService
    {
        IEnumerable<IServer> GetServers();
    }
}