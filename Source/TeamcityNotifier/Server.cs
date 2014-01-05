﻿using System.ComponentModel;

namespace TeamcityNotifier
{
    using System.Collections.Generic;
    using System.Linq;

    using TeamcityNotifier.Wrapper;

    internal class Server : IServer
    {
        private Status status;

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

        public IEnumerable<IProject> Projects { get; internal set; }

        public IProject RootProject
        {
            get
            {
                return Projects.FirstOrDefault(x => !x.HasParent);
            }
        }

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

        public string Name { get; private set; }

        public string UserName { get; private set; }

        public string Password { get; private set; }

        public IUri Uri { get; private set; }

        public IRestConsumer RestConsumer { get; private set; }

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
