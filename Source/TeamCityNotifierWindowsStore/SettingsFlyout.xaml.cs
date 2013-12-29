//*********************************************************
//
// Copyright (c) Microsoft. All rights reserved.
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TeamCityNotifierWindowsStore
{
    using TeamCityNotifierWindowsStore.DataModel;

    using Windows.UI.ApplicationSettings;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls.Primitives;
    using Windows.UI.Xaml.Media.Animation;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsFlyout : TeamCityNotifierWindowsStore.Common.LayoutAwarePage
    {
        // The guidelines recommend using 100px offset for the content animation.
        const int ContentAnimationOffset = 100;

        public SettingsFlyout()
        {
            this.InitializeComponent();
            this.FlyoutContent.Transitions = new TransitionCollection();
            this.FlyoutContent.Transitions.Add(new EntranceThemeTransition()
            {
                FromHorizontalOffset = (SettingsPane.Edge == SettingsEdgeLocation.Right) ? ContentAnimationOffset : (ContentAnimationOffset * -1)
            });

            var allServers = DataService.GetServerConfiguration();
            this.scrollViewer.DataContext = allServers;
        }

        /// <summary>
        /// This is the click handler for the back button on the Flyout.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MySettingsBackClicked(object sender, RoutedEventArgs e)
        {
            // First close our Flyout.
            Popup parent = this.Parent as Popup;
            if (parent != null)
            {
                parent.IsOpen = false;
            }

            // If the app is not snapped, then the back button shows the Settings pane again.
            if (Windows.UI.ViewManagement.ApplicationView.Value != Windows.UI.ViewManagement.ApplicationViewState.Snapped)
            {
                SettingsPane.Show();
            }
        }

        /// <summary>
        /// This is the a common click handler for the buttons on the Flyout.  You would replace this with your own handler
        /// if you have a button or buttons on this page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void FlyoutButton_ClickSave(object sender, RoutedEventArgs e)
        {
            var baseUrl = this.TextBoxBaseUrl.Text;
            var userName = this.TextBoxUserName.Text;
            var password = this.TextBoxPassword.Text;
            var serverName = this.TextBoxServerName.Text;
            var serverOnOff = this.ToggleSwitchServerOnOff.IsOn;

            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values["BaseUrl"] = baseUrl;
            localSettings.Values["UserName"] = userName;
            localSettings.Values["Password"] = password;
            localSettings.Values["ServerName"] = serverName;
            localSettings.Values["ServerOnOff"] = serverOnOff;
        }
    }
}
