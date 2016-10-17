using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using ColorFade_WP8.Resources;
using System.Windows.Media;

namespace ColorFade_WP8
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        private void Rectangle_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            GradientStop gradStop;
            Random rand = new Random();

            // Clear the existing gradient stops
            FadeBrush.GradientStops.Clear();

            // Add a new stop with offset 0 (leading edge)
            gradStop = new GradientStop();
            gradStop.Color = Color.FromArgb(255, (byte)rand.Next(256), (byte)rand.Next(256), (byte)rand.Next(256));
            gradStop.Offset = 0;
            FadeBrush.GradientStops.Add(gradStop);

            // Add a new stop with offset 1 (trailing edge)
            gradStop = new GradientStop();
            gradStop.Color = Color.FromArgb(255, (byte)rand.Next(256), (byte)rand.Next(256), (byte)rand.Next(256));
            gradStop.Offset = 1;
            FadeBrush.GradientStops.Add(gradStop);

            // Add a new stop with a random offset
            gradStop = new GradientStop();
            gradStop.Color = Color.FromArgb(255, (byte)rand.Next(256), (byte)rand.Next(256), (byte)rand.Next(256));
            gradStop.Offset = rand.Next(100) / 100.0f;
            FadeBrush.GradientStops.Add(gradStop);
        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}