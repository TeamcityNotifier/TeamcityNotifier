using System.ComponentModel;

namespace TeamcityNotifier
{
    using TeamcityNotifier.Wrapper;

    public interface IServer : INotifyPropertyChanged
    {
        string Name { get; }

        string UserName { get; }

        string Password { get; }

        IUri Uri { get; }

        IRestConsumer RestConsumer { get; }

        Status Status { get; }

        IProjectRepository BuildRepository { get; }

        IProject RootProject { get; }
    }
}