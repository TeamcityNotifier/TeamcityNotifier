namespace TeamCityNotifierWindowsStore.DataModel
{
    using TeamcityNotifier;

    public class BuildDefinitionPMod : ServerEntityBase
    {
        private string buildRepositoryUrl;

        public BuildDefinitionPMod(
            string uniqueId, string title, string description, string buildRepositoryUrl, Status status)
            : base(uniqueId, title, description, status)
        {
            this.BuildRepositoryUrl = buildRepositoryUrl;
        }

        public string BuildRepositoryUrl
        {
            get { return this.buildRepositoryUrl; }
            set { this.SetProperty(ref this.buildRepositoryUrl, value); }
        }
    }
}
