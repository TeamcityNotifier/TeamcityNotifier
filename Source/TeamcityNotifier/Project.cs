namespace TeamcityNotifier
{
    using xsdtest;

    public class Project : IProject
    {
        private readonly project project;

        public Project(project project)
        {
            this.project = project;
        }

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

    }
}