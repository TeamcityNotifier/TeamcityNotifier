using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace TeamcityNotifier
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    using DataAbstraction;

    internal class Project : IProject
    {
        private readonly string url;

        private readonly ObservableCollection<IBuildDefinition> buildDefinitions;

        private readonly ObservableCollection<IProject> childProjects;

        private Status status;

        private string id;

        private string name;

        private string description;

        private string parentId;

        public event PropertyChangedEventHandler PropertyChanged;

        public Project(string url)
        {
            this.url = url;
            this.childProjects = new ObservableCollection<IProject>();
            this.childProjects.CollectionChanged += ChildProjectsOnCollectionChanged;
            
            this.buildDefinitions = new ObservableCollection<IBuildDefinition>();
            this.buildDefinitions.CollectionChanged += BuildDefinitionsOnCollectionChanged;
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
                return !string.IsNullOrEmpty(ParentId);
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
        
        private void ChildProjectsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            if (notifyCollectionChangedEventArgs.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var item in notifyCollectionChangedEventArgs.NewItems.OfType<IProject>())
                {
                    item.PropertyChanged += UpdateStatus;
                }
            }
            else
            {
                foreach (var item in notifyCollectionChangedEventArgs.NewItems.OfType<IProject>())
                {
                    item.PropertyChanged -= UpdateStatus;
                }
            }
        }
        
        private void BuildDefinitionsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            if (notifyCollectionChangedEventArgs.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var item in notifyCollectionChangedEventArgs.NewItems.OfType<IBuildDefinition>())
                {
                    item.PropertyChanged += UpdateStatus;
                }
            }
            else
            {
                foreach (var item in notifyCollectionChangedEventArgs.NewItems.OfType<IBuildDefinition>())
                {
                    item.PropertyChanged -= UpdateStatus;
                }
            }
        }

        private void UpdateStatus(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName == "Status")
            {
                UpdateStatus();
            }
        }

        private void UpdateStatus()
        {
            var status = Status.Success;

            if (this.BuildDefinitions.Any(x => x.Status == Status.Failure) ||
                this.ChildProjects.Any(x => x.Status == Status.Failure))
            {
                status = Status.Failure;
            }
            if (this.BuildDefinitions.Any(x => x.Status == Status.Error) ||
                this.ChildProjects.Any(x => x.Status == Status.Error))
            {
                status = Status.Error;
            }

            this.Status = status;
        }
    }
}