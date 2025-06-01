using HangedMan_Client.HangedManService;
using System.Windows;

namespace HangedMan_Client.Views
{
    /// <summary>
    /// Lógica de interacción para ProfileDialog.xaml
    /// </summary>
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
            var dialog = new QuestionMessage(message);
            dialog.Owner = Application.Current.MainWindow;
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
