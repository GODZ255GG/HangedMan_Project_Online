using HangedMan_Client.HangedManService;
using System.Windows.Controls;

namespace HangedMan_Client.Views
{
    /*
    * Fecha creación: 05/06/2025
    * Última modificación: 05/06/2025
    * Último modificador: René Ulises
    * Descripción: Vista de WPF que muestra la información del perfil del usuario en el juego "Ahorcado", incluyendo datos personales y puntaje global, y permite navegar para editar la información o regresar al lobby.
    */
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
