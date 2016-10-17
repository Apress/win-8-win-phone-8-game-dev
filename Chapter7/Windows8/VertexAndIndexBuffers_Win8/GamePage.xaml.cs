﻿using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using MonoGame.Framework;


namespace VertexAndIndexBuffers_Win8
{
    /// <summary>
    /// The root page used to display the game.
    /// </summary>
    public sealed partial class GamePage : SwapChainBackgroundPanel
    {
        readonly VertexAndIndexBuffersGame _game;

        public GamePage(string launchArguments)
        {
            this.InitializeComponent();

            // Create the game.
            _game = XamlGame<VertexAndIndexBuffersGame>.Create(launchArguments, Window.Current.CoreWindow, this);
        }
    }
}
