namespace TeamcityNotifier
{
    using System.Collections.Generic;

    public interface IFactory
    {
        IEnumerable<IServer> CreateServers();

        IEnumerable<IProject> CreateProjects(IServer server);
    }
}