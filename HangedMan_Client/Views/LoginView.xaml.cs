using HangedMan_Client.HangedManService;
using System;
using System.Windows;
using System.Windows.Controls;

namespace HangedMan_Client.Views
{
    /*
    * Fecha creación: 22/05/2025
    * Última modificación: 10/06/2025
    * Último modificador: René Ulises
    * Descripción: Vista de WPF que gestiona la autenticación de usuarios en el juego "Ahorcado". Permite iniciar sesión, cambiar el idioma de la interfaz y navegar hacia el registro de nuevos usuarios.
    */
    public partial class LoginView : Page
    {
        private readonly PlayerServicesClient playerServices = new PlayerServicesClient();
        private readonly MainWindow mainWindow;
        public LoginView(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
        }

        private void BtnChangeEnglish_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.ChangeLanguageLogin("en");
        }

        private void BtnChangeSpanish_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.ChangeLanguageLogin("es");
        }

        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.GoToRegisterView();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string email = txtEmail.Text;
            string password = txtPassword.Password;

            if (string.IsNullOrWhiteSpace(email))
            {
                ShowMessage(Properties.Resources.EmailRequiredMessage, 2);
                return;
            }
            if (string.IsNullOrWhiteSpace(password))
            {
                ShowMessage(Properties.Resources.PasswordRequiredMessage, 2);
                return;
            }

            try
            {
                RealizarLogin(email, password);
            }
            catch (Exception)
            {
                ShowMessage(Properties.Resources.GenericErrorMessage, 3);
            }
        }

        private void RealizarLogin(string email, string password)
        {
            Player player = playerServices.LogIn(email, password);
            if (player != null)
            {
                string message = $"{Properties.Resources.LogginMessage} {player.NickName}";
                ShowMessage(message, 1);
                SessionManager.Instance.Login(player);
                NavigationService.Navigate(new LobbyView());
            }
            else
            {
                ShowMessage(Properties.Resources.ErrorLogginMessage, 3);
            }
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.Close();
        }

        private void ShowMessage(string message, int type)
        {
            var dialog = new MessageBoxInformation(message, type)
            {
                Owner = Application.Current.MainWindow
            };
            dialog.ShowDialog();
        }
    }
}
