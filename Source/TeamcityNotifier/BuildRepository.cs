namespace TeamcityNotifier
{
    using System;
    using System.Collections.Generic;

    using DataAbstraction;

    public class BuildRepository : IBuildRepository
    {
        private readonly string url;

        private readonly List<IBuild> builds; 

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

            foreach (var build in baseObject.build)
            {
                this.builds.Add(new Build(build.href));
            }
        }
    }
}