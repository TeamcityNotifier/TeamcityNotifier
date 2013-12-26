namespace TeamCityNotifierWindowsStore.DataModel
{
    using TeamCityNotifierWindowsStore.Data;

    public class BuildDefinitionPMod : SampleDataCommon
    {
        public BuildDefinitionPMod(string uniqueId, string title, string subtitle, 
            string imagePath, string description, string buildRepositoryUrl)
            : base(uniqueId, title, subtitle, imagePath, description)
        {
            this.BuildRepositoryUrl = buildRepositoryUrl;
        }

        private string buildRepositoryUrl;
        public string BuildRepositoryUrl
        {
            get { return this.buildRepositoryUrl; }
            set { this.SetProperty(ref this.buildRepositoryUrl, value); }
        }
    }
}
