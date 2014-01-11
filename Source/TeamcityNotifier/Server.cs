using System.ComponentModel;

namespace TeamcityNotifier
{
    using System.Linq;

    using TeamcityNotifier.Wrapper;

    internal class Server : IServer
    {
        private Status status;

        private IProjectRepository buildRepository;

        public event PropertyChangedEventHandler PropertyChanged;

        public Server(IWrapperFactory factory, IRestConfiguration configuration)
        {
            this.UserName = configuration.UserName;
            this.Password = configuration.Password;
            this.Name = configuration.Name;
            this.Uri = factory.CreateUri(configuration.BaseUrl);

            this.RestConsumer = new RestConsumer(
                this.Uri,
                factory.CreateHttpClientHandler(this.UserName, this.Password),
                factory);
        }

        public string Name { get; private set; }

        public string UserName { get; private set; }

        public string Password { get; private set; }

        public IUri Uri { get; private set; }

        public IRestConsumer RestConsumer { get; private set; }

        public Status Status
        {
            get
            {
                return this.status;
            }
            private set
            {
                if (this.status == value)
                {
                    return;
                }

                this.status = value;
                this.OnPropertyChanged("Status");
            }
        }

        public IProjectRepository BuildRepository
        {
            get
            {
                return this.buildRepository;
            }
            internal set
            {
                if (this.buildRepository == value)
                {
                    return;
                }

                this.buildRepository = value;
                this.buildRepository.PropertyChanged += this.UpdateStatus;
                this.OnPropertyChanged("BuildRepository");
            }
        }

        public IProject RootProject
        {
            get
            {
                return this.BuildRepository.Projects.FirstOrDefault(x => !x.HasParent);
            }
        }

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void UpdateStatus(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName == "Status")
            {
                this.Status = BuildRepository.Status;
            }
        }
    }
}
