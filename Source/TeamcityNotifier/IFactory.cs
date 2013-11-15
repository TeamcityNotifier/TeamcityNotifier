namespace TeamcityNotifier
{
    using System.Collections.Generic;

    using DataAbstraction;

    public interface IFactory
    {
        IEnumerable<IServer> CreateServers();

        IEnumerable<IProject> CreateProjects(IServer server);

        IEnumerable<IBuildDefinition> CreateBuildDefinitions(IServer server, IProject project);
    }
}