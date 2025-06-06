using HangedMan_Client.HangedManService;
using System.Windows;

namespace HangedMan_Client.Views
{
    /*
    * Fecha creación: 05/06/2025
    * Última modificación: 05/06/2025
    * Último modificador: René Ulises
    * Descripción: Ventana de diálogo en WPF que muestra información del perfil del usuario, permite cerrar sesión, ver el perfil completo o consultar el historial de partidas en el juego "Ahorcado".
    */
    public partial class ProfileDialog : Window
    {
        public ProfileDialog()
        {
            InitializeComponent();
            ShowInformationPlayer();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            string message = Properties.Resources.LogOutMessage;
            var dialog = new QuestionMessage(message)
            {
                Owner = Application.Current.MainWindow
            };
            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                var mainWindow = Application.Current.MainWindow as MainWindow;
                mainWindow.goToLoginView();
            }

        }

        private void ShowInformationPlayer()
        {
            Player player = SessionManager.Instance.LoggedInPlayer;
            lblPlayerNickname.Content = player.NickName;
        }

        private void BtnViewProfile_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow.frame.Navigate(new ProfileView());
        }

        private void BtnViewMatchHistory_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow.frame.Navigate(new MatchHistoryView());
        }
    }
}
