using HangedMan_Client.HangedManService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Threading;
using System.Globalization;
using System.Text;

namespace HangedMan_Client.Views
{
    /// <summary>
    /// Lógica de interacción para ChallengerView.xaml
    /// </summary>
    public partial class ChallengerView : Page
    {
        private DispatcherTimer dispatcherTimer;
        private List<TextBlock> textBlocks = new List<TextBlock>();
        GameServicesClient gameServices = new GameServicesClient();
        WordServicesClient wordServices = new WordServicesClient();
        Match match;
        private List<char> charListWord;
        private HashSet<char> guessedLetters = new HashSet<char>();
        string word;
        string clue;
        char selectedLetter;
        int errorCounter = 0;
        int remainingAttempts = 6;

        public ChallengerView(Match match)
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
            remainingAttempts = 6;
            lblCounter.Content = remainingAttempts.ToString();
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            checkMatchStatus();
        }

        private void updateCharSelectDB(char selectedLetter)
        {
            this.selectedLetter = selectedLetter;

            try
            {
                bool confirmation = gameServices.UpdateCharBD(selectedLetter, match.MatchID);
                if (!confirmation)
                {
                    var message = "Error al actualizar la letra en la base de datos";
                    ShowMessage(message, 3);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void updateRemainingAttempts(int remainingAttempts)
        {
            this.remainingAttempts = remainingAttempts;

            try
            {
                bool confirmation = gameServices.UpdateRemainingAttempts(remainingAttempts, match.MatchID);
                if (!confirmation)
                {
                    var message = "Error al actualizar los intentos restantes";
                    ShowMessage(message,3);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
                MessageBox.Show($"Error loading match clue: {e.Message}");
            }
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

        private void LetterButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null || string.IsNullOrWhiteSpace(button.Content.ToString()))
                return;

            char selectedLetter = button.Content.ToString()[0];
            button.IsEnabled = false;

            string normalizedWord = RemoveDiacritics(word.ToLower());
            char normalizedSelected = RemoveDiacritics(selectedLetter.ToString().ToLower())[0];

            bool isCorrect = normalizedWord.Contains(normalizedSelected);

            for (int i = 0; i < word.Length; i++)
            {
                if (char.ToLower(word[i]) == char.ToLower(selectedLetter))
                {
                    isCorrect = true;
                }
            }

            if (!isCorrect)
            {
                remainingAttempts--;
                lblCounter.Content = remainingAttempts.ToString();
            }

            updateCharSelectDB(selectedLetter);

            if (normalizedWord.Contains(normalizedSelected))
            {
                updateWordLines(selectedLetter);
                guessedLetters.Add(normalizedSelected);

                if (wordIsComplete())
                {
                    Player player = SessionManager.Instance.LoggedInPlayer;
                    gameServices.UpdateWinner(player.PlayerID, match.MatchID);
                    gameServices.FinishMatch(match.MatchID);
                    gameServices.UpdatePointsEarned(match.MatchID, player.PlayerID);
                    string message = Properties.Resources.WinnerMatchMessageGuest;
                    var dialog = new WinDialog(message);
                    dialog.Owner = Application.Current.MainWindow;
                    dialog.ShowDialog();
                }

            }
            else
            {
                
                updateRemainingAttempts(remainingAttempts);
                errorCounter++;
                UpdateHangmanImage(errorCounter);

                if (remainingAttempts == 0)
                {
                    string message = Properties.Resources.LosserMatchMessage;
                    gameServices.UpdateWinner(match.ChallengerID, match.MatchID);
                    gameServices.UpdatePointsEarned(match.MatchID, match.ChallengerID);
                    gameServices.FinishMatch(match.MatchID);
                    var dialog = new LossDialog(message);
                    dialog.Owner = Application.Current.MainWindow;
                    dialog.ShowDialog();
                }


            }
        }

        private bool wordIsComplete()
        {
            return !charListWord.Except(guessedLetters).Any();
        }

        private void UpdateHangmanImage(int errorCounter)
        {
            string imagePath = $"pack://application:,,,/Resources/{errorCounter}.jpg";
            imgControl.Source = new BitmapImage(new Uri(imagePath));
        }

        private void updateWordLines(char Letter)
        {
            char normalizedSelected = RemoveDiacritics(Letter.ToString().ToLower())[0];
            guessedLetters.Add(normalizedSelected);

            for (int i = 0; i < word.Length; i++)
            {
                if (RemoveDiacritics(word[i].ToString().ToLower()) == normalizedSelected.ToString())
                {
                    textBlocks[i].Text = word[i].ToString();
                }
            }
        }



        private void checkMatchStatus()
        {
            int matchStatus = gameServices.GetMatchStatus(match.MatchID);
            if (matchStatus == 2)
            {
                string message = Properties.Resources.ChallengerLeaveMatchMessage;
                ShowMessage(message, 2);
                NavigationService.Navigate(new LobbyView());
                dispatcherTimer.Stop();
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

        private string RemoveDiacritics(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;
            var normalized = text.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();
            foreach (var c in normalized)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }
            return sb.ToString().Normalize(NormalizationForm.FormC);
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

        private void ShowMessage(string message, int type)
        {
            var dialog = new MessageBoxInformation(message, type);
            dialog.Owner = Application.Current.MainWindow;
            dialog.ShowDialog();
        }
    }
}
