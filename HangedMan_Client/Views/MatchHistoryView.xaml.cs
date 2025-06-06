using HangedMan_Client.HangedManService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace HangedMan_Client.Views
{
    /*
    * Fecha creación: 27/05/2025
    * Última modificación: 05/06/2025
    * Último modificador: René Ulises
    * Descripción: Vista de WPF que muestra el historial de partidas jugadas por el usuario en el juego "Ahorcado". Permite filtrar por partidas ganadas, perdidas o abandonadas, y navegar entre páginas de resultados.
    */
    public partial class MatchHistoryView : Page
    {
        private readonly GameServicesClient gameServicesClient = new GameServicesClient();
        private readonly WordServicesClient wordServicesClient = new WordServicesClient();
        private List<MatchHistoryItem> allHistoryItems = new List<MatchHistoryItem>();
        private int currentPage = 0;
        private const int PageSize = 4;
        public MatchHistoryView()
        {
            InitializeComponent();
            Loaded += async (s, e) => await InitializeData();
        }

        private async Task InitializeData()
        {
            try
            {
                var player = SessionManager.Instance.LoggedInPlayer;
                var matches = await gameServicesClient.GetMatchesPlayedAsync(player.PlayerID);

                allHistoryItems = new List<MatchHistoryItem>();

                foreach (var match in matches)
                    allHistoryItems.Add(await CreateHistoryItem(match, player.PlayerID));
                UpdatePage();
            }
            catch (Exception ex)
            {
                var message = $"{Properties.Resources.ErrorLoadingHistory}: {ex.Message}";
                var dialog = new MessageBoxInformation(message, 3)
                {
                    Owner = Application.Current.MainWindow
                };
                dialog.ShowDialog();
            }
        }

        private void UpdatePage()
        {
            IEnumerable<MatchHistoryItem> filteredItems = allHistoryItems;

            string emptyMessage = "";
            switch (Tabs.SelectedIndex)
            {
                case 0:
                    filteredItems = allHistoryItems.Where(x => x.Result == Properties.Resources.MatchWon || x.Result == Properties.Resources.WonOpponentLeft);
                    emptyMessage = Properties.Resources.EmptyWins;
                    break;
                case 1:
                    filteredItems = allHistoryItems.Where(x => x.Result == Properties.Resources.MatchLost || x.Result == Properties.Resources.LostYouLeft);
                    emptyMessage = Properties.Resources.EmptyLost;
                    break;
                case 2:
                    filteredItems = allHistoryItems.Where(x => x.Result == Properties.Resources.MatchAbandoned);
                    emptyMessage = Properties.Resources.EmptyAbandoned;
                    break;
            }

            var pageItems = filteredItems.Skip(currentPage * PageSize).Take(PageSize).ToList();
            HistoryList.ItemsSource = pageItems;

            if (!filteredItems.Any())
            {
                lblEmptyMessage.Text = emptyMessage;
                lblEmptyMessage.Visibility = Visibility.Visible;
            }
            else
            {
                lblEmptyMessage.Visibility = Visibility.Collapsed;
            }

            PrevPageBtn.IsEnabled = currentPage > 0;
            NextPageBtn.IsEnabled = (currentPage + 1) * PageSize < filteredItems.Count();
        }


        private void Tabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!IsLoaded) return;

            switch (Tabs.SelectedIndex)
            {
                case 0:
                    lblTitle.Content = Properties.Resources.TitleYourVictories;
                    break;
                case 1:
                    lblTitle.Content = Properties.Resources.TitleLostGames;
                    break;
                case 2:
                    lblTitle.Content = Properties.Resources.TitleAbandonedGames;
                    break;
            }

            currentPage = 0;
            UpdatePage();
        }


        private void PrevPageBtn_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 0)
            {
                currentPage--;
                UpdatePage();
            }
        }

        private void NextPageBtn_Click(object sender, RoutedEventArgs e)
        {
            if ((currentPage + 1) * PageSize < allHistoryItems.Count)
            {
                currentPage++;
                UpdatePage();
            }
        }

        private async Task<MatchHistoryItem> CreateHistoryItem(Match match, int currentPlayerId)
        {
            return new MatchHistoryItem
            {
                MatchId = match.MatchID,
                Opponent = await GetOpponentName(match, currentPlayerId),
                Word = await GetMatchWord(match),
                Result = GetMatchResult(match, currentPlayerId),
                Date = match.DateMatch,
                Language = match.MatchLanguage == 1 ? Properties.Resources.SpanishLanguage
                                                   : Properties.Resources.EnglishLanguage
            };
        }

        private async Task<string> GetOpponentName(Match match, int currentPlayerId)
        {
            if (match.ChallengerID == currentPlayerId)
            {
                return match.GuestID.HasValue
                    ? await gameServicesClient.GetGuestNickNameAsync(match.GuestID.Value)
                    : Properties.Resources.WaitingForPlayer;
            }
            return match.NickNameChallenger;
        }

        private async Task<string> GetMatchWord(Match match)
        {
            try
            {
                return match.MatchLanguage == 1
                    ? await wordServicesClient.GetWordSpanishAsync(match.WordID)
                    : await wordServicesClient.GetWordEnglishAsync(match.WordID);
            }
            catch
            {
                return Properties.Resources.WordNotAvailable;
            }
        }

        private string GetMatchResult(Match match, int playerId)
        {
            if (match.StatusMatchID == 2)
            {
                if (match.WinnerID == null) return Properties.Resources.MatchAbandoned;
                return match.WinnerID == playerId
                    ? Properties.Resources.WonOpponentLeft
                    : Properties.Resources.LostYouLeft;
            }

            if (match.WinnerID == playerId) return Properties.Resources.MatchWon;
            if (match.WinnerID != null) return Properties.Resources.MatchLost;
            return Properties.Resources.MatchIncomplete;
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new LobbyView());
        }
    }

    public class MatchHistoryItem
    {
        public int MatchId { get; set; }
        public string Opponent { get; set; }
        public string Word { get; set; }
        public string Result { get; set; }
        public string Date { get; set; }
        public string Language { get; set; }
    }
}