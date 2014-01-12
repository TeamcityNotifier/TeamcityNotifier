using System.ComponentModel;

namespace TeamcityNotifier
{
    using System;
    using System.Linq;

    using TeamcityNotifier.RestObject;
    using TeamcityNotifier.Wrapper;

    internal class Server : IServer
    {
        private Status status;

        private IProjectRepository projectRepository;

        private bool isServerOn;

        public event PropertyChangedEventHandler PropertyChanged;

        public Server(string name, IUri uri, IRestConsumer restConsumer)
        {
            this.UniqueId = Guid.NewGuid();
            this.Name = name;
            this.Uri = uri;
            this.RestConsumer = restConsumer;
        }

        public Guid UniqueId { get; private set; }

        public string Name { get; private set; }

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

        public IProjectRepository ProjectRepository
        {
            get
            {
                return this.projectRepository;
            }

            set
            {
                if (this.projectRepository == value)
                {
                    return;
                }

                this.projectRepository = value;
                this.projectRepository.PropertyChanged += this.UpdateStatus;
                this.OnPropertyChanged("ProjectRepository");
            }
        }

        public IProject RootProject
        {
            get
            {
                var rootProject = this.ProjectRepository.Projects.FirstOrDefault(x => !x.HasParent);
                rootProject.Parent = this;
                return rootProject;
            }
        }

        public bool IsServerOn
        {
            get
            {
                return this.isServerOn;
            }
            private set
            {
                if (this.isServerOn == value)
                {
                    return;
                }

                this.isServerOn = value;
                this.OnPropertyChanged("IsServerOn");
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
                this.Status = this.ProjectRepository.Status;
            }
        }
    }
}
