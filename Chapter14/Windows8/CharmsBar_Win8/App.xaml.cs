using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ApplicationSettings;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

namespace CharmsBar_Win8
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {


        /// <summary>
        /// Store an instance of the About charm popup
        /// </summary>
        private Popup _aboutCharmPopup;

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
        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            Frame rootFrame = Window.Current.Content as Frame;

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

        // Set up an event handler for responding to the charms menu
        SettingsPane.GetForCurrentView().CommandsRequested += App_CommandsRequested;
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
        /// Respond to the charms menu appearing
        /// </summary>
        void App_CommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            SettingsCommand cmd;

            // Add the "about" item and its handler function
            cmd = new SettingsCommand("about", "About (App Name)", new UICommandInvokedHandler(SettingsAboutCommand));
            args.Request.ApplicationCommands.Add(cmd);

            // Add an "alert" item and its handler function
            cmd = new SettingsCommand("alert", "Show alert box", new UICommandInvokedHandler(SettingsAlertCommand));
            args.Request.ApplicationCommands.Add(cmd);

        }

        private void SettingsAboutCommand(IUICommand command)
        {
            // Check on which side of the screen the SettingsPane is displayed
            bool isRightEdge = (SettingsPane.Edge == SettingsEdgeLocation.Right);

            // Create the flyout and set its size
            AboutFlyout flyout = new AboutFlyout();
            flyout.Width = 346;
            flyout.Height = Window.Current.Bounds.Height;

            // Create and initialize a new Popup object for the flyout
            _aboutCharmPopup = new Popup();

            // Add the proper animation for the panel.
            _aboutCharmPopup.ChildTransitions = new TransitionCollection();
            _aboutCharmPopup.ChildTransitions.Add(new PaneThemeTransition()
            {
                Edge = isRightEdge ? EdgeTransitionLocation.Right : EdgeTransitionLocation.Left
            });

            // Associate the flyout with the popup and set its position and size
            _aboutCharmPopup.Child = flyout;
            _aboutCharmPopup.Width = flyout.Width;
            _aboutCharmPopup.Height = flyout.Height;
            _aboutCharmPopup.SetValue(Canvas.LeftProperty, isRightEdge ? Window.Current.Bounds.Width - flyout.Width : 0);
            _aboutCharmPopup.SetValue(Canvas.TopProperty, 0);
            _aboutCharmPopup.IsLightDismissEnabled = true;

            // Display the flyout
            _aboutCharmPopup.IsOpen = true;

            // Set a handler for the window's Activated event so that we can automatically close
            // the Popup when the window deactivates, and for the Popup's Closed event so that
            // we can remove the window Activated event handler when the popup closes.
            Window.Current.Activated += OnWindowActivated;
            _aboutCharmPopup.Closed += OnPopupClosed;
        }

        private async void SettingsAlertCommand(IUICommand command)
        {
            MessageDialog msg = new MessageDialog("The charm command was selected.");
            await msg.ShowAsync();
        }

        /// <summary>
        /// Handle deactivation with a Charm popup open
        /// </summary>
        void OnWindowActivated(object sender, Windows.UI.Core.WindowActivatedEventArgs e)
        {
            if (e.WindowActivationState == Windows.UI.Core.CoreWindowActivationState.Deactivated)
            {
                if (_aboutCharmPopup != null) _aboutCharmPopup.IsOpen = false;
            }
        }

        /// <summary>
        /// When a popup closes, remove the OnWindowActivated event handler
        /// </summary>
        void OnPopupClosed(object sender, object e)
        {
            Window.Current.Activated -= OnWindowActivated;
        }
    }
}
