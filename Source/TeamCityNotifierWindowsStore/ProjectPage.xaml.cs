// The Item Detail Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234232

namespace TeamCityNotifierWindowsStore
{
    using System;
    using System.Collections.Generic;

    using TeamCityNotifierWindowsStore.Common;
    using TeamCityNotifierWindowsStore.DataModel;

    using Windows.UI.Xaml.Controls;

    /// <summary>
    /// A page that displays details for a single item within a Parent while allowing gestures to
    /// flip through other items belonging to the same Parent.
    /// </summary>
    public sealed partial class ItemDetailPage : TeamCityNotifierWindowsStore.Common.LayoutAwarePage
    {
        public ItemDetailPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            // Allow saved page state to override the initial item to display
            if (pageState != null && pageState.ContainsKey("SelectedItem"))
            {
                navigationParameter = pageState["SelectedItem"];
            }

            // TODO: Create an appropriate data model for your problem domain to replace the sample data
            var project = DataService.GetProject((String)navigationParameter);
            this.DefaultViewModel["Parent"] = project.Parent;
            if (project.Parent is ServerPMod)
            {
               this.DefaultViewModel["Projects"] = ((ServerPMod)project.Parent).Projects; 
            }
            else if (project.Parent is ProjectPMod)
            {
                this.DefaultViewModel["Projects"] = ((ProjectPMod)project.Parent).Projects; 
            }

            this.DefaultViewModel["SubProjects"] = project.Projects;

            this.DefaultViewModel["BuildDefinitions"] = project.BuildDefinitions;

            this.flipView.SelectedItem = project;
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
            var selectedItem = (ProjectPMod)this.flipView.SelectedItem;
            pageState["SelectedItem"] = selectedItem.UniqueId;
        }

        /// <summary>
        /// Invoked when an item is clicked.
        /// </summary>
        /// <param name="sender">The GridView (or ListView when the application is snapped)
        /// displaying the item clicked.</param>
        /// <param name="e">Event data that describes the item clicked.</param>
        void ItemView_ProjectItemClick(object sender, ItemClickEventArgs e)
        {
            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter
            var itemId = ((ProjectPMod)e.ClickedItem).UniqueId;
            this.Frame.Navigate(typeof(ItemDetailPage), itemId);
        }

        private void ItemView_BuildDefinitionItemClick(object sender, ItemClickEventArgs e)
        {
            var buildRepositoryUrl = ((BuildDefinitionPMod)e.ClickedItem).BuildRepositoryUrl;

            this.Frame.Navigate(typeof(BuildDefinitionPage), buildRepositoryUrl);
        }

        private void FlipView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedProject = (ProjectPMod)((FlipView)sender).SelectedItem;

            if (selectedProject.Parent is ServerPMod)
            {
                this.DefaultViewModel["Projects"] = ((ServerPMod)selectedProject.Parent).Projects; 
            }
            else if (selectedProject.Parent is ProjectPMod)
            {
                this.DefaultViewModel["Projects"] = ((ProjectPMod)selectedProject.Parent).Projects; 
            }

            this.DefaultViewModel["SubItems"] = selectedProject.Projects;

            this.DefaultViewModel["BuildDefinitions"] = selectedProject.BuildDefinitions;
        }
    }
}
