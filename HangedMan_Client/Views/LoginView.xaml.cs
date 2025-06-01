using HangedMan_Client.HangedManService;
using System;
using System.Windows;
using System.Windows.Controls;

namespace HangedMan_Client.Views
{
    /// <summary>
    /// Lógica de interacción para LoginView.xaml
    /// </summary>
    public partial class LoginView : Page
    {
        PlayerServicesClient playerServices = new PlayerServicesClient();
        private MainWindow mainWindow;
        public LoginView(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
        }

        private void BtnChangeEnglish_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.changeLanguageLogin("en");
        }

        private void BtnChangeSpanish_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.changeLanguageLogin("es");
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.goToRegisterView();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string email = txtEmail.Text;
                string password = txtPassword.Password;

                if (!checkCredentials(email, password))
                {
                    string message = Properties.Resources.IncompleteLogginMessage;
                    ShowMessage(message, 2);
                }
                else
                {
                    Player player = playerServices.LogIn(email, password);
                    if (player != null)
                    {
                        string message = Properties.Resources.LogginMessage;
                        string messageComplete = message + " " + player.NickName;
                        ShowMessage(messageComplete, 1);
                        SessionManager.Instance.Login(player);

                        NavigationService.Navigate(new LobbyView());
                    }
                    else
                    {
                        string message = Properties.Resources.ErrorLogginMessage;
                        ShowMessage(message, 3);
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, 3);
            }
        }

        private bool checkCredentials(string emai, string password)
        {
            if (string.IsNullOrEmpty(emai) || string.IsNullOrEmpty(password))
            {
                return false;
            }

            return true;

        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.Close();
        }

        private void ShowMessage(string message, int type)
        {
            var dialog = new MessageBoxInformation(message, type);
            dialog.Owner = Application.Current.MainWindow;
            dialog.ShowDialog();
        }
    }
}
