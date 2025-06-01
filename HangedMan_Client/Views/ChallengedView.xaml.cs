using HangedMan_Client.HangedManService;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Threading;
using System.Globalization;
using System.Text;
using System.Linq;

namespace HangedMan_Client.Views
{
    /// <summary>
    /// Lógica de interacción para ChallengerView.xaml
    /// </summary>
    public partial class ChallengedView : Page
    {
        private DispatcherTimer dispatcherTimer;
        private List<TextBlock> textBlocks = new List<TextBlock>();
        GameServicesClient gameServices = new GameServicesClient();
        WordServicesClient wordServices = new WordServicesClient();
        Match match;
        string word;
        string clue;
        char? guestSelectLetter;
        int remainingAttempts = 6;
        private bool matchFinished = false;
        private List<char> charListWord;
        private HashSet<char> guessedLetters = new HashSet<char>();

        public ChallengedView(Match match)
        {
            InitializeComponent();
            LoadMatchWord(match);
            loadMatchClue(match);
            LoadCategory(match.WordID, match.MatchLanguage);
            this.match = match;
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Start();
            remainingAttempts = gameServices.GetRemainingAttempts(match.MatchID);
            lblCounter.Content = remainingAttempts.ToString(); 
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (!matchFinished) 
            {
                getGuestLetter(match);
                updateImage();
                checkMatchFinish();
                checkGuestLeave();                
            }
        }

        private void getGuestLetter(Match matchID)
        {
            guestSelectLetter = gameServices.GetGuestLetter(match.MatchID);

            if (guestSelectLetter.HasValue)
            {
                char selectLetter = guestSelectLetter.Value;
                updateWordLines(selectLetter);
            }
        }

        private void updateImage()
        {
            int newRemainingAttempts = gameServices.GetRemainingAttempts(match.MatchID);
            if (newRemainingAttempts != remainingAttempts)
            {
                remainingAttempts = newRemainingAttempts;
                lblCounter.Content = remainingAttempts.ToString();
                int errorCounter = 6 - remainingAttempts;
                changeImage(errorCounter);
            }
            else if (remainingAttempts == 0)
            {
                matchFinished = true; 
                dispatcherTimer.Stop();
                gameServices.FinishMatch(match.MatchID);
                string message = Properties.Resources.WinnerMatchMessageChallenger;
                var dialog = new WinDialog(message);
                dialog.Owner = Application.Current.MainWindow;
                dialog.ShowDialog();
            }
        }

        private void changeImage(int errorCounter)
        {
            string imagePath = $"pack://application:,,,/Resources/{errorCounter}.jpg";
            imgControl.Source = new BitmapImage(new Uri(imagePath));
        }

        private void updateWordLines(char Letter)
        {
            char normalizedSelected = RemoveDiacritics(Letter.ToString().ToLower())[0];

            for (int i = 0; i < word.Length; i++)
            {
                if (RemoveDiacritics(word[i].ToString().ToLower()) == normalizedSelected.ToString())
                {
                    textBlocks[i].Text = word[i].ToString();
                }
            }
        }

        private void generateWordLines(string word)
        {
            WordPanel.Children.Clear();
            textBlocks.Clear();

            foreach (char letter in word)
            {
                TextBlock texBlock = new TextBlock
                {
                    Text = letter == ' ' ? " " : "_",
                    FontSize = 60,
                    FontWeight = FontWeights.Bold,
                    Margin = new Thickness(5)
                };

                textBlocks.Add(texBlock);
                WordPanel.Children.Add(texBlock);
            }
        }

        private async void loadMatchClue(Match match)
        {
            try
            {
                clue = await getMatchClueAsync(match);
                lblClue.Content = clue;
            }
            catch (Exception e)
            {
                ShowMessage(e.Message, 3);
            }
        }

        private string RemoveDiacritics(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;
            var normalized = text.Normalize(NormalizationForm.FormD);
            var sb = new System.Text.StringBuilder();
            foreach (var c in normalized)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }
            return sb.ToString().Normalize(NormalizationForm.FormC);
        }

        private async void LoadMatchWord(Match match)
        {
            try
            {
                word = await getMatchWordAsync(match);
                generateWordLines(word);
                charListWord = new List<char>(RemoveDiacritics(word.ToLower()).Where(c => c != ' '));
            }
            catch (Exception e)
            {
                ShowMessage(e.Message, 3);
            }
        }

        private async Task<string> getMatchClueAsync(Match match)
        {
            string clueMatch = null;
            try
            {
                if (match != null && match.MatchLanguage == 1)
                {
                    clueMatch = await wordServices.GetClueSpanishAsync(match.WordID);
                }
                else if (match != null && match.MatchLanguage == 2)
                {
                    clueMatch = await wordServices.GetClueEnglishAsync(match.WordID);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return clueMatch;
        }

        private async Task<string> getMatchWordAsync(Match match)
        {
            string wordMatch = null;
            try
            {
                if (match != null && match.MatchLanguage == 1)
                {
                    wordMatch = await wordServices.GetWordSpanishAsync(match.WordID);
                }
                else if (match != null && match.MatchLanguage == 2)
                {
                    wordMatch = await wordServices.GetWordEnglishAsync(match.WordID);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return wordMatch;
        }

        private async void LoadCategory(int wordID, int matchLanguage)
        {
            try
            {
                string category = await Task.Run(() => wordServices.GetCategoryByWordID(wordID, matchLanguage));
                lblCategory.Content = category;
            }
            catch (Exception)
            {
                lblCategory.Content = "N/A";
            }
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
                Player player = SessionManager.Instance.LoggedInPlayer;
                bool exit = gameServices.LeaveMatch(match.MatchID);
                if (exit)
                {
                    gameServices.PenalizeAbandon(player.PlayerID);
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


        private void checkMatchFinish()
        {
            if (matchFinished) return;

            int matchStatus = gameServices.GetMatchStatus(match.MatchID);
            int? winnerId = gameServices.GetWinnerID(match.MatchID);

            if (matchStatus == 3)
            {
                if (winnerId.HasValue && winnerId.Value == match.ChallengerID)
                {
                    matchFinished = true;
                    dispatcherTimer.Stop();
                    string message = Properties.Resources.WinnerMatchMessageChallenger;
                    var dialog = new WinDialog(message);
                    dialog.Owner = Application.Current.MainWindow;
                    dialog.ShowDialog();
                }
                else
                {
                    matchFinished = true;
                    dispatcherTimer.Stop();
                    string message = Properties.Resources.LosserChallengerMessage;
                    var dialog = new LossDialog(message);
                    dialog.Owner = Application.Current.MainWindow;
                    dialog.ShowDialog();
                }
            }
        }

        private void checkGuestLeave()
        {
            int matchStatus = gameServices.GetMatchStatus(match.MatchID);
            if (matchStatus == 2)
            {
                dispatcherTimer.Stop();
                string message = Properties.Resources.GuestLeaveMatchMessage;
                ShowMessage(message, 2);
                NavigationService.Navigate(new LobbyView());
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
