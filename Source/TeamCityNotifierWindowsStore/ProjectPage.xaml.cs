﻿using TeamCityNotifierWindowsStore.Data;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Item Detail Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234232

namespace TeamCityNotifierWindowsStore
{
    using TeamCityNotifierWindowsStore.Common;
    using TeamCityNotifierWindowsStore.DataModel;

    /// <summary>
    /// A page that displays details for a single item within a group while allowing gestures to
    /// flip through other items belonging to the same group.
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
            var project = DataSourceService.GetProject((String)navigationParameter);
            this.DefaultViewModel["Group"] = project.Group;
            if (project.Group is ServerPMod)
            {
               this.DefaultViewModel["Items"] = ((ServerPMod)project.Group).Items; 
            }
            else if (project.Group is ProjectPMod)
            {
                this.DefaultViewModel["Items"] = ((ProjectPMod)project.Group).Items; 
            }

            this.DefaultViewModel["SubItems"] = project.Items;

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

            if (selectedProject.Group is ServerPMod)
            {
                this.DefaultViewModel["Items"] = ((ServerPMod)selectedProject.Group).Items; 
            }
            else if (selectedProject.Group is ProjectPMod)
            {
                this.DefaultViewModel["Items"] = ((ProjectPMod)selectedProject.Group).Items; 
            }

            this.DefaultViewModel["SubItems"] = selectedProject.Items;

            this.DefaultViewModel["BuildDefinitions"] = selectedProject.BuildDefinitions;
        }
    }
}