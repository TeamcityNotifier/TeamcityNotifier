// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TeamCityNotifierWindowsStore
{
    using System;

    using TeamCityNotifierWindowsStore.DataModel;

    using Windows.UI.Xaml.Navigation;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BuildDefinitionPage : TeamCityNotifierWindowsStore.Common.LayoutAwarePage
    {
        public BuildDefinitionPage()
        {
            this.InitializeComponent();
        }

        public override void ReloadData()
        {
            this.Frame.Navigate(typeof(ServerPage), "AllServers");
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.WebView.Source = new Uri(e.Parameter.ToString());
            base.OnNavigatedTo(e);
        }
    }
}
