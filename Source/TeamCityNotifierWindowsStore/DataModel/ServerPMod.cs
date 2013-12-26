namespace TeamCityNotifierWindowsStore.DataModel
{
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;

    /// <summary>
    /// Generic Parent data model.
    /// </summary>
    public class ServerPMod : PModBase
    {
        public ServerPMod(string uniqueId, string title, string imagePath, string description)
            : base(uniqueId, title, imagePath, description)
        {
            this.TopProjects = new ObservableCollection<ProjectPMod>();
            this.Projects = new ObservableCollection<ProjectPMod>();
            this.Projects.CollectionChanged += this.ItemsCollectionChanged;
        }

        public ObservableCollection<ProjectPMod> Projects { get; private set; }

        public ObservableCollection<ProjectPMod> TopProjects { get; private set; }

        private void ItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // Provides a subset of the full items collection to bind to from a ServerPage
            // for two reasons: GridView will not virtualize large items collections, and it
            // improves the user experience when browsing through groups with large numbers of
            // items.
            //
            // A maximum of 12 items are displayed because it results in filled grid columns
            // whether there are 1, 2, 3, 4, or 6 rows displayed
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewStartingIndex < 12)
                    {
                        this.TopProjects.Insert(e.NewStartingIndex,this.Projects[e.NewStartingIndex]);
                        if (this.TopProjects.Count > 12)
                        {
                            this.TopProjects.RemoveAt(12);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Move:
                    if (e.OldStartingIndex < 12 && e.NewStartingIndex < 12)
                    {
                        this.TopProjects.Move(e.OldStartingIndex, e.NewStartingIndex);
                    }
                    else if (e.OldStartingIndex < 12)
                    {
                        this.TopProjects.RemoveAt(e.OldStartingIndex);
                        this.TopProjects.Add(this.Projects[11]);
                    }
                    else if (e.NewStartingIndex < 12)
                    {
                        this.TopProjects.Insert(e.NewStartingIndex, this.Projects[e.NewStartingIndex]);
                        this.TopProjects.RemoveAt(12);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (e.OldStartingIndex < 12)
                    {
                        this.TopProjects.RemoveAt(e.OldStartingIndex);
                        if (this.Projects.Count >= 12)
                        {
                            this.TopProjects.Add(this.Projects[11]);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    if (e.OldStartingIndex < 12)
                    {
                        this.TopProjects[e.OldStartingIndex] = this.Projects[e.OldStartingIndex];
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    this.TopProjects.Clear();
                    while (this.TopProjects.Count < this.Projects.Count && this.TopProjects.Count < 12)
                    {
                        this.TopProjects.Add(this.Projects[this.TopProjects.Count]);
                    }

                    break;
            }
        }
    }
}