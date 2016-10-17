using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;
using Windows.Devices.Sensors;
using Microsoft.Xna.Framework.GamerServices;

#if NETFX_CORE
// These libraries are only available in Windows 8
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Automation.Provider;
#endif

namespace GameFramework
{
    public static class GameHelper
    {

        //-------------------------------------------------------------------------------------
        // Class-level variables

        // A static Random object that will be used by all calls to RandomNext throughout
        // the lifetime of the game.
        private static Random _rand;

        /// <summary>
        /// A structure representing the input capabilities of the current device
        /// </summary>
        public struct InputCapabilities
        {
            public bool IsAccelerometerPresent { get; set; }
            public bool IsGamePadPresent { get; set; }
            public bool IsKeyboardPresent { get; set; }
            public bool IsMousePresent { get; set; }
            public bool IsTouchPresent { get; set; }
            internal bool _isDataPopulated;
        }
        /// <summary>
        /// A cached copy of the InputCapabilities structure that can be returned
        /// on subsequent calls to get the input capabilities.
        /// </summary>
        private static InputCapabilities _inputCaps;


        //-------------------------------------------------------------------------------------
        // Random number generation


        /// <summary>
        /// Returns an nonnegative random value less than the specified maximum
        /// </summary>
        /// <param name="maxValue">The exclusive upper bound of the number to be generated. Must be greater than zero.</param>
        /// <returns></returns>
        public static int RandomNext(int maxValue)
        {
            return RandomNext(0, maxValue);
        }

        /// <summary>
        /// Returns an random value within a specified range
        /// </summary>
        /// <param name="minValue">The inclusive lower bound of the number to be generated.</param>
        /// <param name="maxValue">The exclusive upper bound of the number to be generated.</param>
        /// <returns></returns>
        public static int RandomNext(int minValue, int maxValue)
        {
            // Create the Random object is not already available
            if (_rand == null) _rand = new Random();
            return _rand.Next(minValue, maxValue);
        }

        /// <summary>
        /// Returns a random float between zero and the provided value.
        /// </summary>
        /// <param name="maxValue">The maximum permitted value</param>
        /// <returns></returns>
        public static float RandomNext(float maxValue)
        {
            return RandomNext(0.0f, maxValue);
        }

        /// <summary>
        /// Returns a random float between the two provided values.
        /// </summary>
        /// <param name="minValue">The minimum permitted value.</param>
        /// <param name="maxValue">The maximum permitted value.</param>
        /// <returns></returns>
        public static float RandomNext(float minValue, float maxValue)
        {
            // Create the Random object is not already available
            if (_rand == null) _rand = new Random();
            return (float)_rand.NextDouble() * (maxValue - minValue) + minValue;
        }



        //-------------------------------------------------------------------------------------
        // Input handling


        /// <summary>
        /// Determines which input capabilities are present for the current device
        /// </summary>
        /// <returns></returns>
        public static InputCapabilities GetInputCapabilities(bool forceRefresh = false)
        {
            // Have we already figured this all out?
            // (Or are we being asked to refresh any existing data?)
            if (!_inputCaps._isDataPopulated || forceRefresh)
            {
#if WINDOWS_PHONE
                // Check for keyboard support
                _inputCaps.IsKeyboardPresent = Microsoft.Phone.Info.DeviceStatus.IsKeyboardPresent;
                // WP doesn't support mice
                _inputCaps.IsMousePresent = false;
                // WP always supports touch screens
                _inputCaps.IsTouchPresent = true;
                // Attempt to obtain an accelerometer object -- if this succeeds then the accelerometer is present
                _inputCaps.IsAccelerometerPresent = (Accelerometer.GetDefault() != null);
                // WP doesn't support GamePads
                _inputCaps.IsGamePadPresent = false;
#else
                // Check the various device present values -- non-zero will indicate that the device is present
                _inputCaps.IsKeyboardPresent = (new Windows.Devices.Input.KeyboardCapabilities()).KeyboardPresent != 0;
                _inputCaps.IsMousePresent = (new Windows.Devices.Input.MouseCapabilities()).MousePresent != 0;
                _inputCaps.IsTouchPresent = (new Windows.Devices.Input.TouchCapabilities()).TouchPresent != 0;
                // Attempt to obtain an accelerometer object -- if this succeeds then the accelerometer is present
                _inputCaps.IsAccelerometerPresent = (Accelerometer.GetDefault() != null);
                // Check whether player one's gamepad is available.
                // This isn't the full picture, but will usually be indicative if a gamepad being connected.
                _inputCaps.IsGamePadPresent = Microsoft.Xna.Framework.Input.GamePad.GetCapabilities(PlayerIndex.One).IsConnected;
#endif
                // Remember that we've populated the data so that we don't need to do it again
                _inputCaps._isDataPopulated = true;
            }

            // Return the populated capabilities data
            return _inputCaps;
        }


        //-------------------------------------------------------------------------------------
        // Keyboard Input panel popup


        // A delegate describing the callback function that will be invoked after the keyboard input panel has been used
        public delegate void KeyboardInputCallback(bool result, string text);
        // The callback function to call
        private static KeyboardInputCallback _keyboardInputCallbackFnc;
        // Keeps track of whether the keyboard input panel is currently being displayed
        public static bool KeyboardInputIsVisible { get; set; }
#if NETFX_CORE
        // This is the main game page background panel, inside which the keyboard input panel will be displayed
        private static SwapChainBackgroundPanel _parent;
        // This is the grid that contains the keyboard input panel
        private static Grid _gridBackground;
        // This is the textbox into which the user will enter text
        private static TextBox _textboxInput;
        // The OK and Cancel buttons
        private static Button _buttonOK;
        private static Button _buttonCancel;
#endif
        /// <summary>
        /// Display the keyboard input panel
        /// </summary>
        /// <param name="graphics">The game's GraphicsDeviceManager (usually the _graphics object)</param>
        /// <param name="callbackfnc">A function to call once the keyboard panel has been used</param>
        /// <param name="title">The title for the panel</param>
        /// <param name="body">The body text for the panel</param>
        /// <param name="initialValue">The initial user text to display in the panel</param>
        public static void BeginShowKeyboardInput(GraphicsDeviceManager graphics, KeyboardInputCallback callbackfnc, string title, string body, string initialValue)
        {
            // Don't do anything if the keyboard input panel is already displayed
            if (KeyboardInputIsVisible) return;

            // Remember that the input panel is now being displayed
            KeyboardInputIsVisible = true;
            // Remember the callback function
            _keyboardInputCallbackFnc = callbackfnc;

#if WINDOWS_PHONE
            BeginShowKeyboardInput_WP(title, body, initialValue);
#else
            // Store the game's SwapChainPanel
            _parent = graphics.SwapChainPanel;
            // Show the keyboard 'popup'
            BeginShowKeyboardInput_Win8(title, body, initialValue);
#endif
        }
        
#if WINDOWS_PHONE
        /// <summary>
        /// Display the keyboard input panel
        /// </summary>
        private static void BeginShowKeyboardInput_WP(string title, string body, string initialValue)
        {
            // Show the input dialog to get text from the user
            Guide.BeginShowKeyboardInput(PlayerIndex.One, title, body, initialValue, InputCallback, null);
        }
        /// <summary>
        /// This function will be called when the Windows Phone name entry is completed or cancelled
        /// </summary>
        /// <param name="result"></param>
        private static void InputCallback(IAsyncResult result)
        {
            // Retrieve the entered text
            string text = Guide.EndShowKeyboardInput(result);

            // Indicate that the keyboard input panel has been closed
            KeyboardInputIsVisible = false;

            // Did we get some input from the user? Invoke the callback function
            // with the appropriate result and text values.
            if (text != null)
            {
                _keyboardInputCallbackFnc(true, text);
            }
            else
            {
                _keyboardInputCallbackFnc(false, null);
            }
        }
#else
        /// <summary>
        /// Display the keyboard input panel
        /// </summary>
        private static void BeginShowKeyboardInput_Win8(string title, string body, string initialValue)
        {
            TextBlock textTitle;
            TextBlock textBody;
            Grid gridInputArea;
            StackPanel stackButtons;

            // Create a grid to add to the page. The grid will fill the entire page and has a
            // semi-transparent black background to "mark" the graphics behind.
            _gridBackground = new Grid();
            _gridBackground.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Stretch;
            _gridBackground.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Stretch;
            _gridBackground.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            _gridBackground.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            _gridBackground.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            _gridBackground.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(100, 0, 0, 0));

            // Create another grid to appear as a strip across the center of the screen, inside which
            // the popup text, input box and buttons will be displayed
            gridInputArea = new Grid();
            gridInputArea.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 220, 220, 220));
            gridInputArea.HorizontalAlignment = HorizontalAlignment.Stretch;
            gridInputArea.VerticalAlignment = VerticalAlignment.Stretch;
            gridInputArea.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            gridInputArea.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            gridInputArea.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            gridInputArea.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            gridInputArea.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(20, GridUnitType.Star) });
            gridInputArea.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(60, GridUnitType.Star) });
            gridInputArea.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(20, GridUnitType.Star) });
            _gridBackground.Children.Add(gridInputArea);
            Grid.SetRow(gridInputArea, 1);

            // If we have some text, add it to the grid
            if (!string.IsNullOrEmpty(title))
            {
                textTitle = new TextBlock();
                textTitle.Text = title;
                textTitle.TextWrapping = TextWrapping.Wrap;
                textTitle.Foreground = new SolidColorBrush(Colors.Black);
                textTitle.FontSize = 25;
                textTitle.Margin = new Thickness(0, 20, 0, 0);
                gridInputArea.Children.Add(textTitle);
                Grid.SetRow(textTitle, 0);
                Grid.SetColumn(textTitle, 1);
            }

            // Add the body text
            textBody = new TextBlock();
            textBody.Text = body;
            textBody.TextWrapping = TextWrapping.Wrap;
            textBody.Foreground = new SolidColorBrush(Colors.Black);
            textBody.FontSize = 20;
            textBody.Margin = new Thickness(0, 20, 0, 20);
            gridInputArea.Children.Add(textBody);
            Grid.SetRow(textBody, 1);
            Grid.SetColumn(textBody, 1);

            // Create a textbox for the user to enter text into
            _textboxInput = new TextBox();
            _textboxInput.HorizontalAlignment = HorizontalAlignment.Stretch;
            _textboxInput.VerticalAlignment = VerticalAlignment.Stretch;
            _textboxInput.Height = 20;
            _textboxInput.Background = new SolidColorBrush(Colors.White);
            _textboxInput.Text = initialValue;
            _textboxInput.KeyDown += textboxInput_KeyDown;
            gridInputArea.Children.Add(_textboxInput);
            Grid.SetRow(_textboxInput, 2);
            Grid.SetColumn(_textboxInput, 1);

            // The final row of the popup is occupied by a StackPanel inside which the buttons are displayed
            stackButtons = new StackPanel();
            stackButtons.Orientation = Orientation.Horizontal;
            stackButtons.HorizontalAlignment = HorizontalAlignment.Right;
            stackButtons.Margin = new Thickness(0, 20, 0, 20);
            gridInputArea.Children.Add(stackButtons);
            Grid.SetRow(stackButtons, 3);
            Grid.SetColumn(stackButtons, 1);

            // Add the OK button...
            _buttonOK = new Button();
            _buttonOK.Content = "OK";
            _buttonOK.Padding = new Thickness(0, 5, 0, 5);
            _buttonOK.Width = 110;
            _buttonOK.Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 30, 30, 30));
            _buttonOK.Click += buttonOption_Click;
            stackButtons.Children.Add(_buttonOK);

            // ...and the Cancel button
            _buttonCancel = new Button();
            _buttonCancel.Content = "Cancel";
            _buttonCancel.Padding = new Thickness(0, 5, 0, 5);
            _buttonCancel.Width = 110;
            _buttonCancel.Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 30, 30, 30));
            _buttonCancel.Click += buttonOption_Click;
            stackButtons.Children.Add(_buttonCancel);

            // Add the whole grid to the game page
            _parent.Children.Add(_gridBackground);

            // Set focus into the input field
            _textboxInput.Focus(FocusState.Programmatic);
            _textboxInput.SelectionStart = _textboxInput.Text.Length;

            // Indicate that the input panel is now visible
            KeyboardInputIsVisible = true;
        }

        /// <summary>
        /// Process key presses for the input textbox
        /// </summary>
        static void textboxInput_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            // Did the user press a significant control key?
            switch (e.Key)
            {
                case Windows.System.VirtualKey.Enter:
                    // Simulate clicking the OK button
                    e.Handled = true;
                    ButtonAutomationPeer peer = new ButtonAutomationPeer(_buttonOK);
                    IInvokeProvider invokeProv = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                    invokeProv.Invoke();
                    break;
                case Windows.System.VirtualKey.Escape:
                    // Simulate clicking the Cancel button
                    e.Handled = true;
                    peer = new ButtonAutomationPeer(_buttonCancel);
                    invokeProv = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                    invokeProv.Invoke();
                    break;
            }
        }

        /// <summary>
        /// Respond to the OK or Cancel button being clicked
        /// </summary>
        static void buttonOption_Click(object sender, RoutedEventArgs e)
        {
            // Ensure that focus is taken away from the text field so that the SIP closes.
            // Set the focus to the button instead.
            _buttonOK.Focus(FocusState.Programmatic);

            // Cast the sender as a Button
            Button sourceButton = sender as Button;

            // Remove the popup from display
            _parent.Children.Remove(_gridBackground);
            KeyboardInputIsVisible = false;

            // Which button was pressed? Check and call the callback function as appropriate
            switch (sourceButton.Content.ToString())
            {
                case "OK":
                    _keyboardInputCallbackFnc(true, _textboxInput.Text);
                    break;
                default:
                    _keyboardInputCallbackFnc(false, null);
                    break;
            }
        }
#endif


    }
}
