using System.Windows;

namespace HangedMan_Client.Views
{
    /// <summary>
    /// Lógica de interacción para LossDialog.xaml
    /// </summary>
    public partial class LossDialog : Window
    {
        public LossDialog(string message)
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
