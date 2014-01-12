namespace TeamcityNotifier
{
    using System.Collections.ObjectModel;

    public interface IService
    {
        ObservableCollection<IServer> GetServers();

        void StartPeriodicallyUpdating(ObservableCollection<IServer> servers);

        void StartPeriodicallyUpdating(IServer servers);
    }
}