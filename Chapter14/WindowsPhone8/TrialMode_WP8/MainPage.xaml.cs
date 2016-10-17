using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using TrialMode_WP8.Resources;

namespace TrialMode_WP8
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // Display the current trial state
            ShowTrialState();
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
                buttonPurchase.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                textMode.Text = "This application is currently running in full (purchased) mode.";
                buttonPurchase.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private void buttonPurchase_Click(object sender, RoutedEventArgs e)
        {
#if DEBUG
            // In debug mode, prompt the user to switch to full mode
            if (MessageBox.Show("Simulate switching to full (purchased) mode?", "Simulate purchase", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                // Switch to full mode
                App.SimulateTrialMode = false;
            }
#endif
            MarketplaceDetailTask marketplaceDetailTask = new MarketplaceDetailTask();
            marketplaceDetailTask.Show();
        }

        private void buttonVisitStore_Click(object sender, RoutedEventArgs e)
        {
            MarketplaceDetailTask marketplaceDetailTask = new MarketplaceDetailTask();
            marketplaceDetailTask.Show();
        }
    
    
    }
}