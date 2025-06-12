using HangedMan_Client.HangedManService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace HangedMan_Client.Views
{
    /*
    * Fecha creación: 22/05/2025
    * Última modificación: 10/06/2025
    * Último modificador: René Ulises
    * Descripción: Vista de WPF que gestiona el lobby del juego "Ahorcado". 
    *              Permite visualizar partidas disponibles, crear nuevas partidas, filtrar por categoría y palabra, y unirse a partidas existentes. Se comunica con los servicios de juego y palabras para actualizar la información en tiempo real.
    */
    public partial class LobbyView : Page
    {
        private int matchLanguage;
        private DispatcherTimer dispatcherTimer;
        List<Match> matchesAvaliables;
        List<Category> categories;
        List<Word> words;
        private readonly WordServicesClient wordServicesClient = new WordServicesClient();
        private readonly GameServicesClient gameServicesClient = new GameServicesClient();

        public LobbyView()
        {
            InitializeComponent();
            ShowInformationPlayer();
            ChargeCategories();
            SetupTimer();
            GetAvaliableMatches();
            cbxWord.IsEnabled = false;
            cbxCategory.IsEnabled = false;
        }

        private void SetupTimer()
        {
            dispatcherTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Start();
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            GetAvaliableMatches();

        }

        private async void GetAvaliableMatches()
        {
            try
            {
                Player player = SessionManager.Instance.LoggedInPlayer;
                Match[] aux = await gameServicesClient.GetMatchListAsync(player.PlayerID);
                matchesAvaliables = aux.ToList();

                MatchesItemsControl.ItemsSource = matchesAvaliables;

                if (matchesAvaliables.Count == 0)
                {
                    NoMatchesText.Visibility = Visibility.Visible;
                    MatchesScrollViewer.Visibility = Visibility.Collapsed;
                }
                else
                {
                    NoMatchesText.Visibility = Visibility.Collapsed;
                    MatchesScrollViewer.Visibility = Visibility.Visible;
                }
            }
            catch (Exception)
            {
                dispatcherTimer.Stop();

                ShowMessage(Properties.Resources.ErrorLoadingMatches, 3);

                var mainWindow = Application.Current.MainWindow as MainWindow;
                mainWindow.GoToLoginView();
            }
        }

        private async void ChargeCategories()
        {
            cbxCategory.ItemsSource = null;
            cbxCategory.IsEnabled = false;

            try
            {
                Category[] aux = await wordServicesClient.GetCategoriesAsync();
                categories = aux?.ToList() ?? new List<Category>();

                if (categories.Count == 0)
                {
                    ShowMessage(Properties.Resources.NoCategoriesAvailable, 2);
                    cbxCategory.IsEnabled = false;
                    return;
                }

                cbxCategory.ItemsSource = categories;
                cbxCategory.IsEnabled = true;
            }
            catch (Exception)
            {
                ShowMessage(Properties.Resources.ErrorLoadingCategories, 3);
                cbxCategory.ItemsSource = null;
                cbxCategory.IsEnabled = false;
            }
        }

        private async Task ChargeWordsPerCategory(int category)
        {
            cbxWord.ItemsSource = null;
            cbxWord.IsEnabled = false;

            try
            {
                Word[] aux = await wordServicesClient.GetWordsPerCategoryAsync(category);
                words = aux?.ToList() ?? new List<Word>();

                if (words.Count == 0)
                {
                    ShowMessage(Properties.Resources.NoWordsInCategory, 2);
                    cbxWord.IsEnabled = false;
                    return;
                }

                cbxWord.ItemsSource = words;
                cbxWord.IsEnabled = true;
            }
            catch (Exception)
            {
                ShowMessage(Properties.Resources.ErrorLoadingWords, 3);
                cbxWord.ItemsSource = null;
                cbxWord.IsEnabled = false;
            }
        }

        private void SpanishSelection_Click(object sender, RoutedEventArgs e)
        {
            cbxCategory.DisplayMemberPath = "SpanishCategory";
            cbxWord.DisplayMemberPath = "SpanishWord";
            cbxCategory.IsEnabled = true;
            cbxWord.IsEnabled = true;
            matchLanguage = 1;

        }

        private void EnglishSelection_Click(object sender, RoutedEventArgs e)
        {
            cbxCategory.DisplayMemberPath = "EnglishCategory";
            cbxWord.DisplayMemberPath = "EnglishWord";
            cbxCategory.IsEnabled = true;
            cbxWord.IsEnabled = true;
            matchLanguage = 2;
        }

        private async void BtnCreateGame_Click(object sender, RoutedEventArgs e)
        {
            if (!IsSelectionValid())
            {
                ShowMessage(Properties.Resources.WarningCreateMatch, 2);
                return;
            }

            try
            {
                await CrearPartidaAsync();
            }
            catch (Exception)
            {
                ShowMessage(Properties.Resources.GenericErrorMessage, 3);
            }
        }

        private bool IsSelectionValid()
        {
            return cbxCategory.SelectedItem != null && cbxWord.SelectedItem != null;
        }

        private async Task CrearPartidaAsync()
        {
            Match newMatch = CreateNewMatch();
            if (newMatch == null)
                return;
            Match confirmation = await gameServicesClient.CreateMatchAsync(newMatch);
            if (confirmation != null)
            {
                ShowMessage(Properties.Resources.MatchCreatedMessage, 1);
                NavigationService.Navigate(new WaitingRoomView(confirmation));
            }
            else
            {
                ShowMessage(Properties.Resources.ErrorCreateMatch, 3);
            }
        }

        private Match CreateNewMatch()
        {
            Player player = SessionManager.Instance.LoggedInPlayer;
            if (player == null)
            {
                ShowMessage(Properties.Resources.ErrorNoPlayer, 3);
                return null;
            }

            if (!(cbxWord.SelectedItem is Word selectedWord))
            {
                ShowMessage(Properties.Resources.ErrorNoWordSelected, 3);
                return null;
            }

            var newMatch = new Match
            {
                WordID = selectedWord.WordID,
                ChallengerID = player.PlayerID,
                StatusMatchID = 1,
                GuestID = null,
                WinnerID = null,
                DateMatch = DateTime.Now.ToString(("dd/MM/yyyy")),
                SelectedLetter = null,
                RemainingAttempts = 6,
                NickNameChallenger = player.NickName,
                EmailChallenger = player.Email,
                MatchLanguage = matchLanguage
            };

            return newMatch;
        }

        private async void BtnJoinMatch_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button joinButton) || !(joinButton.DataContext is Match selectedMatch))
                return;

            Player user = SessionManager.Instance.LoggedInPlayer;
            if (user == null)
            {
                ShowMessage(Properties.Resources.GenericErrorMessage, 3);
                return;
            }

            try
            {
                await UnirseAPartidaAsync(user.PlayerID, selectedMatch);
            }
            catch (Exception)
            {
                ShowMessage(Properties.Resources.JoinMatchErrorMessage, 3);
            }
        }

        private async Task UnirseAPartidaAsync(int playerId, Match match)
        {
            await Task.Run(() => gameServicesClient.InitMatch(playerId, match.MatchID));
            ShowMessage(Properties.Resources.JoinGameMessage, 1);
            dispatcherTimer.Stop();
            NavigationService.Navigate(new ChallengerView(match));
        }

        private void BtnProfile_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ProfileDialog
            {
                Owner = Application.Current.MainWindow
            };
            dialog.ShowDialog();
        }

        private void ShowInformationPlayer()
        {
            Player player = SessionManager.Instance.LoggedInPlayer;
            lblPlayerNickname.Content = player.NickName;
        }

        private async void CbxCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbxCategory.SelectedItem != null)
            {
                Category selectedCategory = (Category)cbxCategory.SelectedItem;
                int categoryId = selectedCategory.CategoryID;
                await ChargeWordsPerCategory(categoryId);
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
