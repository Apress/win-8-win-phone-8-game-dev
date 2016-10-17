using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Store;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

namespace TrialMode_Win8
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {

        /// <summary>
        /// The application's LicenseInformation object
        /// </summary>
        private LicenseInformation _licenseInfo = null;
        /// <summary>
        /// Track whether the application is currently running in Trial mode
        /// </summary>
        internal static bool IsTrial { get; set; }

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected async override void OnLaunched(LaunchActivatedEventArgs args)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            await RefreshLicenseInfoAsync();

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                if (!rootFrame.Navigate(typeof(MainPage), args.Arguments))
                {
                    throw new Exception("Failed to create initial page");
                }
            }
            // Ensure the current window is active
            Window.Current.Activate();
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }

        /// <summary>
        /// Refresh the license information for the app
        /// </summary>
        private async Task RefreshLicenseInfoAsync()
        {
            // Do we already have a license information object?
            if (_licenseInfo == null)
            {
                // No, so create one now
#if DEBUG
                // Get a reference to the project's WindowsStoreProxy.xml file
                var sourceFile = await Package.Current.InstalledLocation.GetFileAsync("WindowsStoreProxy.xml");
                // Get the output location for the file
                var destFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync("Microsoft\\Windows Store\\ApiData", CreationCollisionOption.OpenIfExists);
                // Create the output file
                var destFile = await destFolder.CreateFileAsync("WindowsStoreProxy.xml", CreationCollisionOption.ReplaceExisting);
                // Copy the WindowsStoreProxy.xml file to the output file
                await sourceFile.CopyAndReplaceAsync(destFile);
                // Use CurrentAppSimulator to simulate access to the license data
                _licenseInfo = CurrentAppSimulator.LicenseInformation;
#else
                // Use the genuine application license information
                _licenseInfo = CurrentApp.LicenseInformation;
#endif
                // Add an event listener in case the license data is updated while the app is running
                _licenseInfo.LicenseChanged += LicenseInfo_LicenseChanged;
            }

            // Check if we're in trial mode
            IsTrial = (_licenseInfo.IsTrial == true || _licenseInfo.IsActive == false);
        }

        /// <summary>
        /// Respond to the application's license information changing at run-time
        /// </summary>
        private async void LicenseInfo_LicenseChanged()
        {
            // Call back into RefreshLicenseInfoAsync to read the new license information
            await RefreshLicenseInfoAsync();
        }

    }
}
