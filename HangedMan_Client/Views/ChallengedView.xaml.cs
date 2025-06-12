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
    * Descripción: Vista de WPF que gestiona la lógica y la interfaz para el jugador que reta en una partida del juego "Ahorcado". 
    *              Permite mostrar la palabra, pista, categoría, controlar los intentos restantes y manejar la interacción con los servicios de juego y palabras.
    */

    public partial class ChallengedView : Page
    {
        private readonly DispatcherTimer dispatcherTimer;
        private readonly List<TextBlock> _textBlocks = new List<TextBlock>();
        private readonly GameServicesClient _gameServices = new GameServicesClient();
        private readonly WordServicesClient _wordServices = new WordServicesClient();
        private readonly Match _match;
        string word;
        string clue;
        char? guestSelectLetter;
        int remainingAttempts = 6;
        bool matchFinished = false;

        public ChallengedView(Match match)
        {
            InitializeComponent();
            LoadMatchWord(match);
            LoadMatchClue(match);
            LoadCategory(match.WordID, match.MatchLanguage);
            this._match = match;
            dispatcherTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Start();
            remainingAttempts = _gameServices.GetRemainingAttempts(match.MatchID);
            lblCounter.Content = remainingAttempts.ToString();
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (!matchFinished)
            {
                GetGuestLetter(_match);
                UpdateImage();
                CheckMatchFinish();
                CheckGuestLeave();
            }
        }

        private void GetGuestLetter(Match matchID)
        {
            try
            {
                guestSelectLetter = _gameServices.GetGuestLetter(_match.MatchID);

                if (guestSelectLetter.HasValue)
                {
                    char selectLetter = guestSelectLetter.Value;
                    UpdateWordLines(selectLetter);
                }
            }
            catch (Exception)
            {
                dispatcherTimer.Stop();

                ShowMessage(Properties.Resources.GenericErrorMessage, 3);

                var mainWindow = Application.Current.MainWindow as MainWindow;
                mainWindow.GoToLoginView();
            }            
        }

        private void UpdateImage()
        {
            try
            {
                int newRemainingAttempts = _gameServices.GetRemainingAttempts(_match.MatchID);
                if (newRemainingAttempts != remainingAttempts)
                {
                    remainingAttempts = newRemainingAttempts;
                    lblCounter.Content = remainingAttempts.ToString();
                    int errorCounter = 6 - remainingAttempts;
                    ChangeImage(errorCounter);
                }
                else if (remainingAttempts == 0)
                {
                    matchFinished = true;
                    dispatcherTimer.Stop();
                    _gameServices.FinishMatch(_match.MatchID);
                    string message = Properties.Resources.WinnerMatchMessageChallenger;
                    var dialog = new WinDialog(message)
                    {
                        Owner = Application.Current.MainWindow
                    };
                    dialog.ShowDialog();
                }
            }catch (Exception)
            {
                dispatcherTimer.Stop();

                var mainWindow = Application.Current.MainWindow as MainWindow;
                mainWindow.GoToLoginView();
            }
            
        }

        private void ChangeImage(int errorCounter)
        {
            string imagePath = $"pack://application:,,,/Resources/{errorCounter}.jpg";
            imgControl.Source = new BitmapImage(new Uri(imagePath));
        }

        private void UpdateWordLines(char Letter)
        {
            char normalizedSelected = RemoveDiacritics(Letter.ToString().ToLower())[0];

            for (int i = 0; i < word.Length; i++)
            {
                if (RemoveDiacritics(word[i].ToString().ToLower()) == normalizedSelected.ToString())
                {
                    _textBlocks[i].Text = word[i].ToString();
                }
            }
        }

        private void GenerateWordLines(string word)
        {
            WordPanel.Children.Clear();
            _textBlocks.Clear();

            foreach (char letter in word)
            {
                TextBlock texBlock = new TextBlock
                {
                    Text = letter == ' ' ? " " : "_",
                    FontSize = 60,
                    FontWeight = FontWeights.Bold,
                    Margin = new Thickness(5)
                };

                _textBlocks.Add(texBlock);
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
                List<char> charListWord;
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
                    clueMatch = await _wordServices.GetClueSpanishAsync(match.WordID);
                }
                else if (match != null && match.MatchLanguage == 2)
                {
                    clueMatch = await _wordServices.GetClueEnglishAsync(match.WordID);
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
                    wordMatch = await _wordServices.GetWordSpanishAsync(match.WordID);
                }
                else if (match != null && match.MatchLanguage == 2)
                {
                    wordMatch = await _wordServices.GetWordEnglishAsync(match.WordID);
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
                string category = await Task.Run(() => _wordServices.GetCategoryByWordID(wordID, matchLanguage));
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
                Player player = SessionManager.Instance.LoggedInPlayer;
                bool exit = _gameServices.LeaveMatch(_match.MatchID);
                if (exit)
                {
                    _gameServices.PenalizeAbandon(player.PlayerID);
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


        private void CheckMatchFinish()
        {
            try
            {
                if (matchFinished) return;

                int matchStatus = _gameServices.GetMatchStatus(_match.MatchID);
                int? winnerId = _gameServices.GetWinnerID(_match.MatchID);

                if (matchStatus == 3)
                {
                    if (winnerId.HasValue && winnerId.Value == _match.ChallengerID)
                    {
                        matchFinished = true;
                        dispatcherTimer.Stop();
                        string message = Properties.Resources.WinnerMatchMessageChallenger;
                        var dialog = new WinDialog(message)
                        {
                            Owner = Application.Current.MainWindow
                        };
                        dialog.ShowDialog();
                    }
                    else
                    {
                        matchFinished = true;
                        dispatcherTimer.Stop();
                        string message = Properties.Resources.LosserChallengerMessage;
                        var dialog = new LossDialog(message)
                        {
                            Owner = Application.Current.MainWindow
                        };
                        dialog.ShowDialog();
                    }
                }
            }
            catch (Exception)
            {
                dispatcherTimer.Stop();

                var mainWindow = Application.Current.MainWindow as MainWindow;
                mainWindow.GoToLoginView();
            }
        }

        private void CheckGuestLeave()
        {
            try
            {
                int matchStatus = _gameServices.GetMatchStatus(_match.MatchID);
                if (matchStatus == 2)
                {
                    dispatcherTimer.Stop();
                    string message = Properties.Resources.GuestLeaveMatchMessage;
                    ShowMessage(message, 2);
                    NavigationService.Navigate(new LobbyView());
                }
            }
            catch (Exception)
            {
                dispatcherTimer.Stop();

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
