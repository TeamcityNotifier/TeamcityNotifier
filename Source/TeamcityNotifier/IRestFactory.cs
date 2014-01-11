namespace TeamcityNotifier
{
    using System.Collections.Generic;

    using TeamcityNotifier.RestObject;

    public interface IRestFactory
    {
        IEnumerable<IServer> CreateServers();

        IProjectRepository GetProjectRepository(IServer server);
    }
}