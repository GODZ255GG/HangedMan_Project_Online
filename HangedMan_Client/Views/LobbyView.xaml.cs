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
    /// <summary>
    /// Lógica de interacción para Lobby.xaml
    /// </summary>
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
            catch (Exception ex)
            {
                string message = Properties.Resources.ErrorLoadingMatches + ": " + ex.Message;
                ShowMessage(message, 3);
                NoMatchesText.Visibility = Visibility.Visible;
                MatchesScrollViewer.Visibility = Visibility.Collapsed;
            }
        }

        private async void ChargeCategories()
        {
            try
            {
                Category[] aux = await wordServicesClient.GetCategoriesAsync();
                categories = aux.ToList();
                cbxCategory.ItemsSource = categories;

            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, 3);
            }
        }

        private async Task ChargeWordsPerCategory(int category)
        {
            try
            {
                Word[] aux = await wordServicesClient.GetWordsPerCategoryAsync(category);
                words = aux.ToList();
                cbxWord.ItemsSource = words;
                cbxWord.IsEnabled = true;
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, 3);
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
            try
            {
                if (AllSelectioned())
                {
                    Match newMatch = CreateNewMatch();
                    Match confirmation = await gameServicesClient.CreateMatchAsync(newMatch);
                    if (confirmation != null)
                    {
                        string message = Properties.Resources.MatchCreatedMessage;
                        ShowMessage(message, 1);
                        NavigationService.Navigate(new WaitingRoomView(confirmation));
                    }
                }
                else
                {
                    string message = Properties.Resources.WarningCreateMatch;
                    ShowMessage(message, 2);
                }
            }
            catch (Exception ex)
            {
                ShowMessage(ex.ToString(), 3);
            }
        }

        private bool AllSelectioned()
        {
            if (cbxWord.SelectedItem != null && cbxCategory != null)
            {
                return true;
            }
            return false;
        }

        private Match CreateNewMatch()
        {
            try
            {
                Player player = SessionManager.Instance.LoggedInPlayer;
                Word selectedWord = (Word)cbxWord.SelectedItem;

                Match newMatch = new Match();
                newMatch.WordID = selectedWord.WordID;
                newMatch.ChallengerID = player.PlayerID;
                newMatch.StatusMatchID = 1;
                newMatch.GuestID = null;
                newMatch.WinnerID = null;
                newMatch.DateMatch = DateTime.Now.ToString(("dd/MM/yyyy"));
                newMatch.SelectedLetter = null;
                newMatch.RemainingAttempts = 6;
                newMatch.NickNameChallenger = player.NickName;
                newMatch.EmailChallenger = player.Email;
                newMatch.MatchLanguage = matchLanguage;

                return newMatch;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void BtnJoinMatch_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button joinButton)
            {
                if (joinButton.DataContext is Match selectedMatch)
                {
                    try
                    {
                        Player user = SessionManager.Instance.LoggedInPlayer;
                        gameServicesClient.InitMatch(user.PlayerID, selectedMatch.MatchID);
                        string message = Properties.Resources.JoinGameMessage;
                        ShowMessage(message, 1);
                        dispatcherTimer.Stop();
                        NavigationService.Navigate(new ChallengerView(selectedMatch));
                    }
                    catch (Exception)
                    {
                        string message = Properties.Resources.JoinMatchErrorMessage;
                        ShowMessage(message, 3);
                    }
                }
            }
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
