using System.Windows;

namespace HangedMan_Client.Views
{
    /// <summary>
    /// Lógica de interacción para WinDialog.xaml
    /// </summary>
    public partial class WinDialog : Window
    {
        public WinDialog(string message)
        {
            InitializeComponent();
            this.lblMessage.Content = message;
        }

        private void BtnAcept_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow.frame.Navigate(new LobbyView());

        }
    }
}
