namespace TeamcityNotifier.RestObject
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    using DataAbstraction;

    using TeamcityNotifier.RestObject;

    internal class Project : IProject
    {
        private readonly string url;

        private readonly List<IBuildDefinition> buildDefinitions;

        private readonly List<IProject> childProjects;

        private Status status;

        private string id;

        private string name;

        private string description;

        private string parentId;

        public event PropertyChangedEventHandler PropertyChanged;

        public Project(string url)
        {
            this.url = url;
            this.childProjects = new List<IProject>();
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

        public bool HasParent
        {
            get
            {
                return !string.IsNullOrEmpty(this.ParentId);
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
            childProject.PropertyChanged += this.UpdateStatus;

            this.OnPropertyChanged("ChildProjects");
        }

        public void RemoveChild(IProject childProject)
        {
            this.childProjects.Remove(childProject);
            childProject.PropertyChanged -= this.UpdateStatus;

            this.OnPropertyChanged("ChildProjects");
        }

        public void SetData(object obj)
        {
            var baseObject = (project) obj;

            this.Id = baseObject.id;
            this.Name = baseObject.name;
            this.Description = baseObject.description;
            this.ParentId = baseObject.parentProject.id;

            this.ClearBuildDefinitions();
            this.FillBuildDefinitions(baseObject.buildTypes);
        }

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void ClearBuildDefinitions()
        {
            foreach (var buildDefinition in this.buildDefinitions)
            {
                buildDefinition.PropertyChanged -= this.UpdateStatus;
            }

            this.buildDefinitions.Clear();

            this.OnPropertyChanged("BuildDefinitions");
        }

        private void FillBuildDefinitions(IEnumerable<buildTyperef> buildTypeRefs)
        {
            foreach (var buildTypeRef in buildTypeRefs)
            {
                var buildDefinition = new BuildDefinition(buildTypeRef.href);
                this.buildDefinitions.Add(buildDefinition);

                buildDefinition.PropertyChanged += this.UpdateStatus;
            }

            this.OnPropertyChanged("BuildDefinitions");
        }

        private void UpdateStatus(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName == "Status")
            {
                this.UpdateStatus();
            }
        }

        private void UpdateStatus()
        {
            var newStatus = Status.Success;

            if (this.BuildDefinitions.Any(x => x.Status == Status.Failure) ||
                this.ChildProjects.Any(x => x.Status == Status.Failure))
            {
                newStatus = Status.Failure;
            }
            if (this.BuildDefinitions.Any(x => x.Status == Status.Error) ||
                this.ChildProjects.Any(x => x.Status == Status.Error))
            {
                newStatus = Status.Error;
            }

            this.Status = newStatus;
        }
    }
}