﻿// The Parent Detail Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234229

namespace TeamCityNotifierWindowsStore
{
    using System;
    using System.Collections.Generic;

    using TeamCityNotifierWindowsStore.DataModel;

    using TeamcityNotifier.RestObject;

    using Windows.UI.Xaml.Controls;

    /// <summary>
    /// A page that displays an overview of a single group, including a preview of the items
    /// within the group.
    /// </summary>
    public sealed partial class ServerDetailPage : TeamCityNotifierWindowsStore.Common.LayoutAwarePage
    {
        private string navigationParameter;

        public ServerDetailPage()
        {
            this.InitializeComponent();
            this.AddServerSettingsToServerPane();
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
            // TODO: Create an appropriate data model for your problem domain to replace the sample data
            this.navigationParameter = navigationParameter.ToString();
            var server = DataService.GetServer(this.navigationParameter);
            this.DefaultViewModel["Server"] = server;
            this.DefaultViewModel["Projects"] = server.RootProject.ChildProjects;
        }

        /// <summary>
        /// Invoked when an item is clicked.
        /// </summary>
        /// <param name="sender">The GridView (or ListView when the application is snapped)
        /// displaying the item clicked.</param>
        /// <param name="e">Event data that describes the item clicked.</param>
        void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter
            var itemId = ((IProject)e.ClickedItem).UniqueId;
            this.Frame.Navigate(typeof(ProjectPage), itemId);
        }

        public override void ReloadData()
        {
            base.ReloadData();
            var server = DataService.GetServer(this.navigationParameter);
            if (server != null)
            {
                this.DefaultViewModel["Server"] = server;
                this.DefaultViewModel["Projects"] = server.RootProject.ChildProjects;
            }
            else
            {
                this.Frame.Navigate(typeof(ServerPage), "AllServers");
            }
        }
    }
}
