using HangedMan_Client.HangedManService;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace HangedMan_Client.Views
{
    /*
    * Fecha creación: 27/05/2025
    * Última modificación: 05/06/2025
    * Último modificador: René Ulises
    * Descripción: Vista de WPF que gestiona la lógica y la interfaz para el jugador retado en una partida del juego "Ahorcado".
    *              Permite seleccionar letras, mostrar la palabra, pista y categoría, controlar los intentos, y comunicarse con los servicios de juego y palabras.
    */
    public partial class ChallengerView : Page
    {
        private readonly DispatcherTimer dispatcherTimer;
        private readonly List<TextBlock> textBlocks = new List<TextBlock>();
        private readonly GameServicesClient gameServices = new GameServicesClient();
        private readonly WordServicesClient wordServices = new WordServicesClient();
        private readonly Match match;
        private List<char> charListWord;
        private readonly HashSet<char> guessedLetters = new HashSet<char>();
        string word;
        string clue;
        char selectedLetter;
        int errorCounter = 0;
        int remainingAttempts = 6;

        public ChallengerView(Match match)
        {
            InitializeComponent();
            LoadMatchWord(match);
            LoadMatchClue(match);
            LoadCategory(match.WordID, match.MatchLanguage);
            this.match = match;
            dispatcherTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Start();
            remainingAttempts = 6;
            lblCounter.Content = remainingAttempts.ToString();
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            CheckMatchStatus();
        }

        private void UpdateCharSelectDB(char selectedLetter)
        {
            this.selectedLetter = selectedLetter;

            try
            {
                bool confirmation = gameServices.UpdateCharBD(selectedLetter, match.MatchID);
                if (!confirmation)
                {
                    var message = Properties.Resources.ErrorUpdatingLetter;
                    ShowMessage(message, 3);
                }
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, 3);
            }
        }

        private void UpdateRemainingAttempts(int remainingAttempts)
        {
            this.remainingAttempts = remainingAttempts;

            try
            {
                bool confirmation = gameServices.UpdateRemainingAttempts(remainingAttempts, match.MatchID);
                if (!confirmation)
                {
                    var message = Properties.Resources.ErrorUpdatingRemainigAttempts;
                    ShowMessage(message, 3);
                }
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, 3);
            }
        }

        private void GenerateWordLines(string word)
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


        private async void LoadMatchClue(Match match)
        {
            try
            {
                clue = await GetMatchClueAsync(match);
                lblClue.Content = clue;
            }
            catch (Exception)
            {
                dispatcherTimer.Stop();

                ShowMessage(Properties.Resources.GenericErrorMessage, 3);

                var mainWindow = Application.Current.MainWindow as MainWindow;
                mainWindow.GoToLoginView();
            }
        }

        private async void LoadMatchWord(Match match)
        {
            try
            {
                word = await GetMatchWordAsync(match);
                GenerateWordLines(word);
                charListWord = new List<char>(RemoveDiacritics(word.ToLower()).Where(c => c != ' '));
            }
            catch (Exception)
            {
                dispatcherTimer.Stop();

                ShowMessage(Properties.Resources.GenericErrorMessage, 3);

                var mainWindow = Application.Current.MainWindow as MainWindow;
                mainWindow.GoToLoginView();
            }
        }



        private async Task<string> GetMatchClueAsync(Match match)
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

        private async Task<string> GetMatchWordAsync(Match match)
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
            if (!(sender is Button button) || string.IsNullOrWhiteSpace(button.Content.ToString()))
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

            UpdateCharSelectDB(selectedLetter);

            if (normalizedWord.Contains(normalizedSelected))
            {
                UpdateWordLines(selectedLetter);
                guessedLetters.Add(normalizedSelected);

                if (WordIsComplete())
                {
                    Player player = SessionManager.Instance.LoggedInPlayer;
                    gameServices.UpdateWinner(player.PlayerID, match.MatchID);
                    gameServices.FinishMatch(match.MatchID);
                    gameServices.UpdatePointsEarned(match.MatchID, player.PlayerID);
                    string message = Properties.Resources.WinnerMatchMessageGuest;
                    var dialog = new WinDialog(message)
                    {
                        Owner = Application.Current.MainWindow
                    };
                    dialog.ShowDialog();
                }

            }
            else
            {

                UpdateRemainingAttempts(remainingAttempts);
                errorCounter++;
                UpdateHangmanImage(errorCounter);

                if (remainingAttempts == 0)
                {
                    string message = Properties.Resources.LosserMatchMessage;
                    gameServices.UpdateWinner(match.ChallengerID, match.MatchID);
                    gameServices.UpdatePointsEarned(match.MatchID, match.ChallengerID);
                    gameServices.FinishMatch(match.MatchID);
                    var dialog = new LossDialog(message)
                    {
                        Owner = Application.Current.MainWindow
                    };
                    dialog.ShowDialog();
                }


            }
        }

        private bool WordIsComplete()
        {
            return !charListWord.Except(guessedLetters).Any();
        }

        private void UpdateHangmanImage(int errorCounter)
        {
            string imagePath = $"pack://application:,,,/Resources/{errorCounter}.jpg";
            imgControl.Source = new BitmapImage(new Uri(imagePath));
        }

        private void UpdateWordLines(char Letter)
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



        private void CheckMatchStatus()
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
                dispatcherTimer.Stop();

                ShowMessage(Properties.Resources.GenericErrorMessage, 3);

                var mainWindow = Application.Current.MainWindow as MainWindow;
                mainWindow.GoToLoginView();
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
