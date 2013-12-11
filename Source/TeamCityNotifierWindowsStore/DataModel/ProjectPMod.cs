namespace TeamCityNotifierWindowsStore.Data
{
    using System;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Generic item data model.
    /// </summary>
    public class ProjectPMod : SampleDataCommon
    {
        public ProjectPMod(String uniqueId, String title, String subtitle, String imagePath, String description, String content, SampleDataCommon group)
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

        private SampleDataCommon _group;
        public SampleDataCommon Group
        {
            get { return this._group; }
            set { this.SetProperty(ref this._group, value); }
        }

        private ObservableCollection<ProjectPMod> _items = new ObservableCollection<ProjectPMod>();
        public ObservableCollection<ProjectPMod> Items
        {
            get { return this._items; }
        }

        private ObservableCollection<ProjectPMod> _topItem = new ObservableCollection<ProjectPMod>();
        public ObservableCollection<ProjectPMod> TopItems
        {
            get { return this._topItem; }
        }
    }
}