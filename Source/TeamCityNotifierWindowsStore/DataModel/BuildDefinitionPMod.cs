namespace TeamCityNotifierWindowsStore.DataModel
{
    public class BuildDefinitionPMod : PModBase
    {
        private string buildRepositoryUrl;

        public BuildDefinitionPMod(
            string uniqueId, string title, string imagePath, string description, string buildRepositoryUrl)
            : base(uniqueId, title, imagePath, description)
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
