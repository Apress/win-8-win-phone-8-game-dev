using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using MultiplePages_WP8.Resources;

namespace MultiplePages_WP8
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

        private void buttonHello_Click(object sender, RoutedEventArgs e)
        {
            // Set the name into the name page's static YourName property
            NamePage.YourName = textName.Text;
            // Navigate to the Name page
            NavigationService.Navigate(new Uri("/NamePage.xaml", UriKind.Relative));
        }

        private void buttonAbout_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to the About page
            NavigationService.Navigate(new Uri("/AboutPage.xaml", UriKind.Relative));
        }




    }
}