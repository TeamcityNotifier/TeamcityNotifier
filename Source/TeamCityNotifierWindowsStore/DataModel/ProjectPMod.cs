namespace TeamCityNotifierWindowsStore.DataModel
{
    using System.Collections.ObjectModel;

    using TeamcityNotifier;

    /// <summary>
    /// Generic item data model.
    /// </summary>
    public class ProjectPMod : ServerEntityBase
    {
        private string content = string.Empty;

        private ServerEntityBase parent;

        public ProjectPMod(
            string uniqueId, string title, string description, string content, ServerEntityBase parent, Status status)
            : base(uniqueId, title, description, status)
        {
            this.BuildDefinitions = new ObservableCollection<BuildDefinitionPMod>();
            this.TopProjects = new ObservableCollection<ProjectPMod>();
            this.Projects = new ObservableCollection<ProjectPMod>();
            this.content = content;
            this.parent = parent;
        }

        public string Content
        {
            get { return this.content; }
            set { this.SetProperty(ref this.content, value); }
        }

        public ServerEntityBase Parent
        {
            get { return this.parent; }
            set { this.SetProperty(ref this.parent, value); }
        }

        public ObservableCollection<ProjectPMod> Projects { get; private set; }

        public ObservableCollection<ProjectPMod> TopProjects { get; private set; }

        public ObservableCollection<BuildDefinitionPMod> BuildDefinitions { get; private set; }
    }
}