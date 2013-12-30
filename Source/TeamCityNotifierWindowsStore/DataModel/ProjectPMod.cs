namespace TeamCityNotifierWindowsStore.DataModel
{
    using System;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Generic item data model.
    /// </summary>
    public class ProjectPMod : ServerEntityBase
    {
        public ProjectPMod(
            String uniqueId, String title, String imagePath, String description, String content, ServerEntityBase parent)
            : base(uniqueId, title, imagePath, description)
        {
            BuildDefinitions = new ObservableCollection<BuildDefinitionPMod>();
            TopProjects = new ObservableCollection<ProjectPMod>();
            Projects = new ObservableCollection<ProjectPMod>();
            this.content = content;
            this.parent = parent;
        }

        private string content = string.Empty;
        public string Content
        {
            get { return this.content; }
            set { this.SetProperty(ref this.content, value); }
        }

        private ServerEntityBase parent;
        public ServerEntityBase Parent
        {
            get { return this.parent; }
            set { this.SetProperty(ref this.parent, value); }
        }

        public ObservableCollection<ProjectPMod> Projects { get; private set; }

        public ObservableCollection<ProjectPMod> TopProjects { get; private set; }

        public ObservableCollection<BuildDefinitionPMod> BuildDefinitions { get; private set; }

        private bool buildSuccessful;
        public bool BuildSuccessful
        {
            get { return this.buildSuccessful; }
            set { this.SetProperty(ref this.buildSuccessful, value); }
        }
    }
}