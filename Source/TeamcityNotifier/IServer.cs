using System.ComponentModel;

namespace TeamcityNotifier
{
    using System.Collections.Generic;

    using TeamcityNotifier.Wrapper;

    public interface IServer : INotifyPropertyChanged
    {
        IEnumerable<IProject> Projects { get; }

        string Name { get; }

        string UserName { get; }

        string Password { get; }

        Status Status { get; }

        IProject RootProject { get; }

        IUri Uri { get; }

        IRestConsumer RestConsumer { get; }
    }
}