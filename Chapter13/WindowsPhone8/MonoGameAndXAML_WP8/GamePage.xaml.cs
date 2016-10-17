using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Xna.Framework;
using MonoGame.Framework.WindowsPhone;
using MonoGameAndXAML_WP8.Resources;

namespace MonoGameAndXAML_WP8
{
    public partial class GamePage : PhoneApplicationPage
    {
        private MonoGameAndXAMLGame _game;

        /// <summary>
        /// A static property that returns the current instance of the game page
        /// </summary>
        public static GamePage Current { get; set; }

        // Last-known values to prevent unnecessary updates of the control text
        private int _lives = -1;
        private int _score = -1;

        /// <summary>
        /// Track whether the Reset button has been clicked
        /// </summary>
        public bool ResetButtonClicked { get; set; }

        /// <summary>
        /// Class constructor
        /// </summary>
        public GamePage()
        {
            InitializeComponent();

            // Set the current instance of the page so that the game can access it
            Current = this;

            // Create the game
            _game = XamlGame<MonoGameAndXAMLGame>.Create("", this);
        }

        /// <summary>
        /// Set the displayed number of lives
        /// </summary>
        public void SetLives(int lives)
        {
            if (lives != _lives)
            {
                Dispatcher.BeginInvoke(delegate()
                {
                    textLives.Text = "Lives: " + lives.ToString();
                    _lives = lives;
                });
            }
        }

        /// <summary>
        /// Set the displayed score
        /// </summary>
        public void SetScore(int score)
        {
            if (score != _score)
            {
                Dispatcher.BeginInvoke(delegate()
                {
                    textScore.Text = "Score: " + score.ToString();
                    _score = score;
                });
            }
        }

        /// <summary>
        /// Respond to the user clicking the Reset button
        /// </summary>
        private void buttonReset_Click(object sender, RoutedEventArgs e)
        {
            // Indicate that the button was clicked
            ResetButtonClicked = true;
        }

        /// <summary>
        /// Respond to the user clicking the Menu button
        /// </summary>
        private void buttonMenu_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to the Menu page
            NavigationService.Navigate(new Uri("/MenuPage.xaml", UriKind.Relative));
        }

    }
}