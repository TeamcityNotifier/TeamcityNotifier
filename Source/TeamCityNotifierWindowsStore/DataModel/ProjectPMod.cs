namespace TeamCityNotifierWindowsStore.Data
{
    using System;

    /// <summary>
    /// Generic item data model.
    /// </summary>
    public class ProjectPMod : SampleDataCommon
    {
        public ProjectPMod(String uniqueId, String title, String subtitle, String imagePath, String description, String content, ServerPMod group)
            : base(uniqueId, title, subtitle, imagePath, description)
        {
            this._content = content;
            this._group = group;
        }

        private string _content = string.Empty;
        public string Content
        {
            get { return this._content; }
            set { this.SetProperty(ref this._content, value); }
        }

        private ServerPMod _group;
        public ServerPMod Group
        {
            get { return this._group; }
            set { this.SetProperty(ref this._group, value); }
        }
    }
}