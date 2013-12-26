namespace TeamcityNotifier
{
    using System.Collections.Generic;

    using TeamcityNotifier.Wrapper;

    public interface IServer
    {
        IEnumerable<IProject> Projects { get; }

        string Name { get; }

        string UserName { get; }

        string Password { get; }

        IProject RootProject { get; }

        IUri Uri { get; }

        IRestConsumer RestConsumer { get; }
    }
}