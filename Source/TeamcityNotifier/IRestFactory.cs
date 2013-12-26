namespace TeamcityNotifier
{
    using System.Collections.Generic;

    public interface IRestFactory
    {
        IEnumerable<IServer> CreateServers();

        IProjectRepository GetProjectRepository(IServer server);
    }
}