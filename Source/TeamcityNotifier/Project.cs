namespace TeamcityNotifier
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    using DataAbstraction;

    public class Project : IProject
    {
        private readonly string url;

        private readonly List<IBuildDefinition> buildDefinitions;

        private readonly HashSet<IProject> childProjects;

        private string id;

        private string name;

        private string description;

        private string parentId;

        public event PropertyChangedEventHandler PropertyChanged;

        public Project(string url)
        {
            this.url = url;
            this.childProjects = new HashSet<IProject>();
            this.buildDefinitions = new List<IBuildDefinition>();
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
                return typeof(project);
            }
        }

        public IEnumerable<IRestObject> Dependencies
        {
            get
            {
                return this.buildDefinitions;
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

        public string ParentId
        {
            get
            {
                return this.parentId;
            }

            private set
            {
                if (this.parentId == value)
                {
                    return;
                }

                this.parentId = value;
                this.OnPropertyChanged("ParentId");
            }
        }

        public IEnumerable<IProject> ChildProjects
        {
            get
            {
                return this.childProjects;
            }
        }

        public IEnumerable<IBuildDefinition> BuildDefinitions
        {
            get
            {
                return this.buildDefinitions;
            }
        }

        public void AddChild(IProject childProject)
        {
            this.childProjects.Add(childProject);
        }

        public void RemoveChild(IProject childProject)
        {
            this.childProjects.Remove(childProject);
        }

        public void SetData(object obj)
        {
            var baseObject = (project) obj;

            this.Id = baseObject.id;
            this.Name = baseObject.name;
            this.Description = baseObject.description;
            this.ParentId = baseObject.parentProject.id;

            foreach (var buildTypeRef in baseObject.buildTypes)
            {
                this.buildDefinitions.Add(new BuildDefinition(buildTypeRef.href));
            }
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