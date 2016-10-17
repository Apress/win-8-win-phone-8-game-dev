using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Store;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TrialMode_Win8
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            // Display the current trial state
            ShowTrialState();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        /// <summary>
        /// Display information about the trial state of the app
        /// </summary>
        private void ShowTrialState()
        {
            // Display the trial state of the app
            if (App.IsTrial)
            {
                textMode.Text = "This application is currently running in trial mode.";
                buttonPurchase.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
            else
            {
                textMode.Text = "This application is currently running in full (purchased) mode.";
                buttonPurchase.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
        }

        private async void buttonPurchase_Click(object sender, RoutedEventArgs e)
        {
            try
            {
#if DEBUG
                await CurrentAppSimulator.RequestAppPurchaseAsync(false);
#else
                await CurrentApp.RequestAppPurchaseAsync(false);
#endif
            }
            catch
            {
                // Failed to complete the purchase
            }
            // The license information will have been updated by this time.
            // Refresh the page content as appropriate for the new trial state.
            ShowTrialState();
        }

        private async void buttonVisitStore_Click(object sender, RoutedEventArgs e)
        {
            var uri = new Uri(@"ms-windows-store:PDP?PFN=" + Package.Current.Id.FamilyName);
            await Windows.System.Launcher.LaunchUriAsync(uri);
        }
    }
}
