using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using VersionNumber_WP8.Resources;

namespace VersionNumber_WP8
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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // Get the assembly name
            string name = Assembly.GetExecutingAssembly().FullName;
            // Use this to obtain its version
            Version version = new AssemblyName(name).Version;
            // Display the string in the page
            textVersion.Text = "The version number is " + version;
        }
    }
}