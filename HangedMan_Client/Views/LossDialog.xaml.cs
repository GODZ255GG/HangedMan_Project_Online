using System.Windows;

namespace HangedMan_Client.Views
{
    /*
    * Fecha creación: 05/06/2025
    * Última modificación: 05/06/2025
    * Último modificador: René Ulises
    * Descripción: Ventana de diálogo en WPF que muestra un mensaje de derrota al usuario en el juego "Ahorcado" y permite regresar al lobby.
*/
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
