using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ColorFade_Win8
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void Rectangle_Tapped_1(object sender, TappedRoutedEventArgs e)
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
    }
}
