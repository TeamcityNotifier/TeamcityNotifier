namespace TeamcityNotifier
{
    using System.Collections.Generic;

    public interface IFactory
    {
        IEnumerable<IServer> CreateServers();

        IProjectRepository GetProjectRepository(IServer server);
    }
}