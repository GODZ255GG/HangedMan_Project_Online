using HangedMan_Client.HangedManService;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace HangedMan_Client.Views
{
    /// <summary>
    /// Lógica de interacción para WaitingRoomView.xaml
    /// </summary>
    public partial class WaitingRoomView : Page
    {
        GameServicesClient gameServicesClient = new GameServicesClient();
        Match match;
        private DispatcherTimer dispatcherTimer;
        public WaitingRoomView(Match match)
        {
            InitializeComponent();
            this.match = match;
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Start();
        }

        private void BtnLeaveMatch_Click(object sender, RoutedEventArgs e)
        {
            string message = Properties.Resources.LeaveMatchConfirmation;
            var dialog = new QuestionMessage(message);
            dialog.Owner = Application.Current.MainWindow;
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
            isThereGuest();
        }

        private void isThereGuest()
        {
            bool isThereGuest = gameServicesClient.IsThereGuest(match.MatchID);
            if (isThereGuest)
            {
                dispatcherTimer.Stop();
                string message = Properties.Resources.PlayerJoinedMessage;
                lblWaitingPlayer.Content = message;
                btnLeaveMatch.IsEnabled = false;
                DispatcherTimer delayTimer = new DispatcherTimer();
                delayTimer.Interval = TimeSpan.FromSeconds(1);
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
            var dialog = new MessageBoxInformation(message, type);
            dialog.Owner = Application.Current.MainWindow;
            dialog.ShowDialog();
        }
    }
}
