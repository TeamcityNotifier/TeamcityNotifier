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

        public IEnumerable<IProject> Projects
        {
            get
            {
                return this.projects;
            }
        }

        public void SetData(object obj)
        {
            var baseObject = (projects1) obj;

            foreach (var projectRef in baseObject.project)
            {
                var project = new Project(projectRef.href);
                project.PropertyChanged += ProjectOnPropertyChanged;

                this.projects.Add(project);

                if (!string.IsNullOrEmpty(project.ParentId))
                {
                    this.AddToNewParent(project);
                }
            }
        }

        private void ProjectOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            var changedProject = (Project)sender;

            if (propertyChangedEventArgs.PropertyName == "ParentId")
            {
                this.RemoveFromCurrentParent(changedProject);
                this.AddToNewParent(changedProject);
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