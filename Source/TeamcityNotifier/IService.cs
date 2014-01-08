namespace TeamcityNotifier
{
    using System.Collections.Generic;

    public interface IService
    {
        IEnumerable<IServer> GetServers();

        void StartPeriodicallyUpdating(IEnumerable<IServer> servers);

        void StartPeriodicallyUpdating(IServer servers);
    }
}