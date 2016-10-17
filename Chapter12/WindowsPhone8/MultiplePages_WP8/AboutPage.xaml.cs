using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace MultiplePages_WP8
{
    public partial class AboutPage : PhoneApplicationPage
    {
        public AboutPage()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            // Can we navigate backward?
            if (NavigationService.CanGoBack)
            {

                NavigationService.GoBack();
            }
        }
    }
}