namespace TeamCityRestClient
{
    using System.Collections.Generic;
    using System.Linq;

    using xsdtest;

  
    public class Project : IProject
    {
        private readonly Server server;
        private readonly string url;

        private project project;

        public Project(Server server, string url)
        {
            this.server = server;
            this.url = url;
        }

        public string Name
        {
            get
            {
                if (project == null)
                {
                    this.project = this.Get();
                }

                return this.project.name;
            }
        }

        public string Description
        {
            get
            {
                if (project == null)
                {
                    this.project = this.Get();
                }

                return this.project.description;
            }
        }

        private project Get()
        {
            return this.server.Get<project>(url);
        }
    }
}