namespace TeamcityNotifier
{
    using System.Collections.Generic;

    using xsdtest;

    public class Project : IProject
    {
        private readonly project project;

        public Project(project project)
        {
            this.project = project;
        }

        public IEnumerable<IBuildDefinition> BuildDefinitions { get; internal set; }

        public string Name
        {
            get
            {
                return this.project.name;
            }
        }

        public string Description
        {
            get
            {
                return this.project.description;
            }
        }

        public string Href
        {
            get
            {
                return this.project.href;
            }
        }
    }
}