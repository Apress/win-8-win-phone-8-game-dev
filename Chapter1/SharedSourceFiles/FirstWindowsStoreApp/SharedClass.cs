using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Windows.UI.Popups;

namespace SharedAppCode
{
    class SharedClass
    {
        private const string MessageText = "Welcome to your app!";
        private const string MessageTitle = "Message";

#if WINDOWS_PHONE
        public void ShowMessage()
        {
            MessageBox.Show(MessageText, MessageTitle, MessageBoxButton.OK);
        }
#else
        public async Task ShowMessage()
        {
            MessageDialog msg = new MessageDialog(MessageText, MessageTitle);
            await msg.ShowAsync();
        }
#endif
    }
}
