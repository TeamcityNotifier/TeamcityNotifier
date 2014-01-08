namespace TeamcityNotifier
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    using DataAbstraction;

    internal class BuildRepository : IBuildRepository
    {
        private readonly string url;

        private IBuild lastBuild;

        private readonly List<IBuild> builds;

        public event PropertyChangedEventHandler PropertyChanged;

        public BuildRepository(string url)
        {
            this.url = url;
            this.builds = new List<IBuild>();
        }

        public string Url
        {
            get
            {
                return this.url;
            }
        }

        public Type BaseType
        {
            get
            {
                return typeof(builds);
            }
        }

        public IEnumerable<IRestObject> Dependencies
        {
            get
            {
                return this.builds;
            }
        }

        public IBuild LastBuild
        {
            get
            {
                return this.lastBuild;
            }
            set
            {

                if (this.lastBuild == value)
                {
                    return;
                }

                this.lastBuild = value;
                this.OnPropertyChanged("LastBuild");
            }
        }

        public IEnumerable<IBuild> Builds
        {
            get
            {
                return this.builds;
            }
        } 

        public void SetData(object obj)
        {
            var baseObject = (builds)obj;

            this.builds.Clear();

            foreach (var build in baseObject.build)
            {
                this.builds.Add(new Build(build.href));
            }

            this.LastBuild = this.builds.FirstOrDefault();
        }

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}