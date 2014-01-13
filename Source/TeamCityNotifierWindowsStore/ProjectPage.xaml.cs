// The Item Detail Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234232

namespace TeamCityNotifierWindowsStore
{
    using System;
    using System.Collections.Generic;

    using TeamCityNotifierWindowsStore.Common;
    using TeamCityNotifierWindowsStore.DataModel;

    using TeamcityNotifier;
    using TeamcityNotifier.RestObject;

    using Windows.UI.Xaml.Controls;

    /// <summary>
    /// A page that displays details for a single item within a Parent while allowing gestures to
    /// flip through other items belonging to the same Parent.
    /// </summary>
    public sealed partial class ProjectPage : TeamCityNotifierWindowsStore.Common.LayoutAwarePage
    {
        private string navigationParameter;

        public ProjectPage()
        {
            this.InitializeComponent();
            this.AddServerSettingsToServerPane();
        }

        public override void ReloadData()
        {
            base.ReloadData();
            var project = DataService.GetProject(this.navigationParameter);
            if (project != null)
            {
                this.SetData(project);
                this.flipView.SelectedItem = project;
            }
            else
            {
                this.Frame.Navigate(typeof(ServerPage), "AllServers");
            }
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

            this.navigationParameter = navigationParameter.ToString();
            var project = DataService.GetProject(this.navigationParameter);

            if (project != null)
            {
                this.SetData(project);
                this.flipView.SelectedItem = project;
            }
            else
            {
                this.Frame.Navigate(typeof(ServerPage), "AllServers");
            }
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
            var selectedItem = (IProject)this.flipView.SelectedItem;
            if (selectedItem != null)
            {
               pageState["SelectedItem"] = selectedItem.UniqueId; 
            }
        }

        private void SetData(IProject project)
        {
            var parent = (IProject)project.Parent;

            if (parent.HasParent)
            {
                this.DefaultViewModel["Parent"] = parent;
                this.DefaultViewModel["Projects"] = parent.ChildProjects;
            }
            else
            {
                this.DefaultViewModel["Parent"] = parent.Parent;
                this.DefaultViewModel["Projects"] = parent.ChildProjects;
            }

            this.DefaultViewModel["SubProjects"] = project.ChildProjects;

            this.DefaultViewModel["BuildDefinitions"] = project.BuildDefinitions;
        }

        /// <summary>
        /// Invoked when an item is clicked.
        /// </summary>
        /// <param name="sender">The GridView (or ListView when the application is snapped)
        /// displaying the item clicked.</param>
        /// <param name="e">Event data that describes the item clicked.</param>
        private void ItemView_ProjectItemClick(object sender, ItemClickEventArgs e)
        {
            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter
            var itemId = ((IProject)e.ClickedItem).UniqueId;
            this.Frame.Navigate(typeof(ProjectPage), itemId);
        }

        private void ItemView_BuildDefinitionItemClick(object sender, ItemClickEventArgs e)
        {
            var buildDefinition = ((IBuildDefinition)e.ClickedItem);
            var webUrl = buildDefinition.BuildRepository.LastBuild.WebUrl;
            this.Frame.Navigate(typeof(BuildDefinitionPage), webUrl);
        }

        private void FlipView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedProject = (IProject)((FlipView)sender).SelectedItem;
            this.SetData(selectedProject);
        }
    }
}
