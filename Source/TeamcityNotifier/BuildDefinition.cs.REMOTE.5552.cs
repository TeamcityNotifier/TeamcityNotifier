﻿namespace TeamcityNotifier
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    using DataAbstraction;

    internal class BuildDefinition : IBuildDefinition
    {
        private readonly string url;

        private IBuild lastBuild;

        private IBuildRepository buildRepository;

        private string description;

        private string name;

        private string id;

        private Status status;

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

        public IEnumerable<IRestObject> Dependencies
        {
            get
            {
                return new List<IRestObject> { this.BuildRepository };
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

        public IBuildRepository BuildRepository
        {
            get
            {
                return this.buildRepository;
            }
            private set
            {
                if (this.buildRepository == value)
                {
                    return;
                }

                this.buildRepository = value;
                this.OnPropertyChanged("BuildRepository");
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
                this.Status = value.Status;
                this.OnPropertyChanged("LastBuild");
            }
        }

        public void SetData(object obj)
        {
            var baseObject = (buildType) obj;

            this.Id = baseObject.id;
            this.Name = baseObject.name;
            this.Description = baseObject.description;
            this.BuildRepository = new BuildRepository(baseObject.builds.href + "?locator=count:1");
            
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