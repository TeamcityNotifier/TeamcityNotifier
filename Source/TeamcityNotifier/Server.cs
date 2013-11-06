namespace TeamcityNotifier
{
    using System;
    using System.Collections.Generic;

    using TeamcityNotifier.Wrapper;

    public class Server : IServer
    {
        public Server(IUri uri, string userName, string password)
        {
            this.UserName = userName;
            this.Password = password;
            this.Uri = uri;
        }

        public IEnumerable<IProject> Projects { get; internal set; }

        public string Name { get; internal set; }

        public string UserName { get; private set; }

        public string Password { get; private set; }

        public IUri Uri { get; private set; }
    }
}
