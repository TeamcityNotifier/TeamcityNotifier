namespace TeamcityNotifier
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    using DataAbstraction;

    internal class BuildDefinition : IBuildDefinition
    {
        private readonly string url;

        private IBuild lastBuild;

        private string buildRepositoryUrl;

        private string description;

        private string name;

        private string id;

        public event PropertyChangedEventHandler PropertyChanged;

        public BuildDefinition(string url)
        {
            this.url = url;
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
                return typeof(buildType);
            }
        }

        public IEnumerable<IRestObject> Dependencies
        {
            get
            {
                return new List<IRestObject>();
            }
        }

        public string Id
        {
            get
            {
                return this.id;
            }
            private set
            {
                if (this.id == value)
                {
                    return;
                }

                this.id = value;
                this.OnPropertyChanged("Id");
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
            private set
            {
                if (this.name == value)
                {
                    return;
                }

                this.name = value;
                this.OnPropertyChanged("Name");
            }
        }

        public string Description
        {
            get
            {
                return this.description;
            }
            private set
            {
                if (this.description == value)
                {
                    return;
                }

                this.description = value;
                this.OnPropertyChanged("Description");
            }
        }

        public string BuildRepositoryUrl
        {
            get
            {
                return this.buildRepositoryUrl;
            }
            private set
            {
                if (this.buildRepositoryUrl == value)
                {
                    return;
                }

                this.buildRepositoryUrl = value;
                this.OnPropertyChanged("BuildRepositoryUrl");
            }
        }

        IBuild IBuildDefinition.LastBuild
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

        public void SetData(object obj)
        {
            var baseObject = (buildType) obj;

            this.Id = baseObject.id;
            this.Name = baseObject.name;
            this.Description = baseObject.description;
            this.BuildRepositoryUrl = baseObject.builds.href + "?locator=count:1";
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