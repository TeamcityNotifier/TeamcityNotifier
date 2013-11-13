﻿namespace TeamcityNotifier
{
    using System;
    using System.Collections.Generic;

    using TeamcityNotifier.Wrapper;

    public class Server : IServer
    {
        public Server(IWrapperFactory factory, IRestConfiguration configuration)
        {
            this.UserName = configuration.UserName;
            this.Password = configuration.Password;
            this.Name = configuration.Name;
            this.Uri = factory.CreateUri(configuration.BaseUrl);
        }

        public IEnumerable<IProject> Projects { get; internal set; }

        public string Name { get; internal set; }

        public string UserName { get; private set; }

        public string Password { get; private set; }

        public IUri Uri { get; private set; }
    }
}
