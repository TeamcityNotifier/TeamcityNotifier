namespace TeamcityNotifier
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    using DataAbstraction;

    internal class ProjectRepository : IProjectRepository
    {
        private readonly string url;

        private readonly List<IProject> projects;

        private Status status;

        public event PropertyChangedEventHandler PropertyChanged;

        public ProjectRepository(string url)
        {
            this.url = url;
            this.projects = new List<IProject>();
        }

        public string Url
        {
            get
            {
                return url;
            }
        }

        public Type BaseType
        {
            get
            {
                return typeof(projects1);
            }
        }

        public IEnumerable<IRestObject> Dependencies
        {
            get
            {
                return this.Projects;
            }
        }

        public Status Status
        {
            get
            {
                return this.status;
            }
            set
            {
                if (this.status == value)
                {
                    return;
                }

                this.status = value;
                this.OnPropertyChanged("Status");
            }
        }

        public IEnumerable<IProject> Projects
        {
            get
            {
                return this.projects;
            }
        }

        public void SetData(object obj)
        {
            var baseObject = (projects1)obj;

            this.ClearProjects();

            this.FillProjects(baseObject.project);
        }

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void ClearProjects()
        {
            foreach (var project in this.projects)
            {
                project.PropertyChanged -= this.ProjectOnPropertyChanged;
            }

            this.projects.Clear();

            this.OnPropertyChanged("Projects");
        }

        private void FillProjects(IEnumerable<projectref> projectRefs)
        {
            foreach (var projectRef in projectRefs)
            {
                var project = new Project(projectRef.href);
                this.projects.Add(project);

                project.PropertyChanged += this.ProjectOnPropertyChanged;
            }

            this.OnPropertyChanged("Projects");
        }

        private void ProjectOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            var changedProject = (Project)sender;

            if (propertyChangedEventArgs.PropertyName == "ParentId")
            {
                this.RemoveFromCurrentParent(changedProject);
                this.AddToNewParent(changedProject);
            }
            else if (propertyChangedEventArgs.PropertyName == "Status")
            {
                this.Status = changedProject.Status;
            }
        }

        private void RemoveFromCurrentParent(Project changedProject)
        {
            var currentParent = this.Projects.FirstOrDefault(project => project.ChildProjects.Contains(changedProject));

            if (currentParent != null)
            {
                currentParent.RemoveChild(changedProject);
            }
        }

        private void AddToNewParent(Project changedProject)
        {
            var parentProject = this.Projects.FirstOrDefault(project => project.Id == changedProject.ParentId);
            parentProject.AddChild(changedProject);
        }
    }
}