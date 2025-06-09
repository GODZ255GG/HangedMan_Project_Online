using HangedMan_Client.HangedManService;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace HangedMan_Client.Views
{
    /*
    * Fecha creación: 27/05/2025
    * Última modificación: 05/06/2025
    * Último modificador: René Ulises
    * Descripción: Vista de WPF que gestiona la sala de espera para el juego "Ahorcado". Permite al jugador esperar a que otro usuario se una a la partida, salir de la sala o continuar cuando el oponente se conecta.
    */
    public partial class WaitingRoomView : Page
    {
        private readonly GameServicesClient gameServicesClient = new GameServicesClient();
        private readonly Match match;
        private readonly DispatcherTimer dispatcherTimer;
        public WaitingRoomView(Match match)
        {
            InitializeComponent();
            this.match = match;
            dispatcherTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Start();
        }

        private void BtnLeaveMatch_Click(object sender, RoutedEventArgs e)
        {
            string message = Properties.Resources.LeaveMatchConfirmation;
            var dialog = new QuestionMessage(message)
            {
                Owner = Application.Current.MainWindow
            };
            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                LeaveMatch();
            }
        }

        private void LeaveMatch()
        {
            dispatcherTimer.Stop();
            try
            {
                bool exit = gameServicesClient.LeaveMatch(match.MatchID);
                if (exit)
                {
                    string message = Properties.Resources.MatchLeaveMessage;
                    ShowMessage(message, 1);
                    NavigationService.Navigate(new LobbyView());

                }
                else
                {
                    string message = Properties.Resources.MatchLeaveMessageError;
                    ShowMessage(message, 3);
                }

            }
            catch (Exception e)
            {
                ShowMessage(e.Message, 3);
            }
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            IsThereGuest();
        }

        private void IsThereGuest()
        {
            bool isThereGuest = gameServicesClient.IsThereGuest(match.MatchID);
            if (isThereGuest)
            {
                dispatcherTimer.Stop();
                string message = Properties.Resources.PlayerJoinedMessage;
                lblWaitingPlayer.Content = message;
                btnLeaveMatch.IsEnabled = false;
                DispatcherTimer delayTimer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromSeconds(1)
                };
                delayTimer.Tick += (sender, e) =>
                {
                    delayTimer.Stop();
                    NavigationService.Navigate(new ChallengedView(match));
                };
                delayTimer.Start();
            }
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
