using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using MonoGame.Framework;


namespace MonoGameAndXAML_Win8
{
    /// <summary>
    /// The root page used to display the game.
    /// </summary>
    public sealed partial class GamePage : SwapChainBackgroundPanel
    {
        readonly MonoGameAndXAMLGame _game;

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
        public GamePage(string launchArguments)
        {
            this.InitializeComponent();

            // Set the current instance of the page so that the game can access it
            Current = this;

            // Create the game
            _game = XamlGame<MonoGameAndXAMLGame>.Create(launchArguments, Window.Current.CoreWindow, this);
        }

        /// <summary>
        /// Set the displayed number of lives
        /// </summary>
        public void SetLives(int lives)
        {
            if (lives != _lives)
            {
                textLives.Text = "Lives: " + lives.ToString();
                _lives = lives;
            }
        }

        /// <summary>
        /// Set the displayed score
        /// </summary>
        public void SetScore(int score)
        {
            if (score != _score)
            {
                textScore.Text = "Score: " + score.ToString();
                _score = score;
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
            Window.Current.Content = new MenuPage();
        }

    }
}
