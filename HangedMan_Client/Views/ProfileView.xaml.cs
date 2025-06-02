using HangedMan_Client.HangedManService;
using System.Windows.Controls;

namespace HangedMan_Client.Views
{
    /// <summary>
    /// Lógica de interacción para ProfileView.xaml
    /// </summary>
    public partial class ProfileView : Page
    {
        private readonly PlayerServicesClient playerServicesClient = new PlayerServicesClient();
        public ProfileView()
        {
            InitializeComponent();
            ShowInformationPlayer();
        }

        private void ShowInformationPlayer()
        {
            Player player = SessionManager.Instance.LoggedInPlayer;
            lblPlayerNickname.Content = player.NickName;
            txtFullName.Text = player.FullName;
            txtEmail.Text = player.Email;
            txtTelephone.Text = player.PhoneNumber;
            txtBirthDate.Text = player.BirthDate;
            lblGlobalScore.Content = playerServicesClient.GetPoints(player.PlayerID) + " " + Properties.Resources.labelPoints;
        }

        private void BtnBackLobby_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService.Navigate(new LobbyView());
        }

        private void BtnEditInfo_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Player player = SessionManager.Instance.LoggedInPlayer;
            NavigationService.Navigate(new EditProfileView(player));
        }
    }
}
