﻿using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using MonoGame.Framework;


namespace OrientationSupport_Win8
{
    /// <summary>
    /// The root page used to display the game.
    /// </summary>
    public sealed partial class GamePage : SwapChainBackgroundPanel
    {
        readonly OrientationSupportGame _game;

        public GamePage(string launchArguments)
        {
            this.InitializeComponent();

            // Create the game.
            _game = XamlGame<OrientationSupportGame>.Create(launchArguments, Window.Current.CoreWindow, this);
        }
    }
}
