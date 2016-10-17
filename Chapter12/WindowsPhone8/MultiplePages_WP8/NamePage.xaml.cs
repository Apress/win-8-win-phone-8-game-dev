using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace MultiplePages_WP8
{
    public partial class NamePage : PhoneApplicationPage
    {

        /// <summary>
        /// Static property to contain the name from the main page
        /// </summary>
        public static string YourName { get; set; }

        public NamePage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // Do we have a name?
            if (!string.IsNullOrEmpty(YourName))
            {
                // Display the name
                textblockName.Text = "Hello, " + YourName + "!";
            }
            else
            {
                // Couldn't locate a name
                textblockName.Text = "Couldn't find your name, sorry!";
            }
        }

    }
}