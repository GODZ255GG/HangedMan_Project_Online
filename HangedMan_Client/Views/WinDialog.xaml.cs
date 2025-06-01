using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HangedMan_Client.Views
{
    /// <summary>
    /// Lógica de interacción para WinDialog.xaml
    /// </summary>
    public partial class WinDialog : Window
    {
        private string message;
        public WinDialog(string message)
        {
            InitializeComponent();
            this.message = message;
            TextView(message);
        }

        private void TextView(String message)
        {
            lblMessage.Content = message;
        }

        private void BtnAcept_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow.frame.Navigate(new LobbyView());
            
        }
    }
}
