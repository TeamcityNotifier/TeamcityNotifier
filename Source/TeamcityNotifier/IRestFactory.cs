namespace TeamcityNotifier
{
    using System.Collections.ObjectModel;

    using TeamcityNotifier.RestObject;

    public interface IRestFactory
    {
        ObservableCollection<IServer> CreateServers();

        IProjectRepository GetProjectRepository(IServer server);
    }
}