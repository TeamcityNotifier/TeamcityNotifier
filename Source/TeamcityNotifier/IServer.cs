using System.ComponentModel;

namespace TeamcityNotifier
{
    using TeamcityNotifier.RestObject;
    using TeamcityNotifier.Wrapper;

    public interface IServer : INotifyPropertyChanged
    {
        string Name { get; }

        IUri Uri { get; }

        IRestConsumer RestConsumer { get; }

        Status Status { get; }

        IProjectRepository ProjectRepository { get; }

        IProject RootProject { get; }
    }
}