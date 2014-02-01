namespace TeamCityNotifierWindowsStore.DummyData
{
    using System;
    using System.ComponentModel;

    using TeamcityNotifier;
    using TeamcityNotifier.RestObject;
    using TeamcityNotifier.Wrapper;

    public class DummyServer : IServer
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Guid UniqueId { get; private set; }

        public string Name { get; set; }

        public IUri Uri { get; private set; }

        public IRestConsumer RestConsumer { get; private set; }

        public Status Status { get; set; }

        public IProjectRepository ProjectRepository { get; private set; }

        public IProject RootProject { get; set; }
    }
}