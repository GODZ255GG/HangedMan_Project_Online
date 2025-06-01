using HangedMan_Client.HangedManService;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace HangedMan_Client.Views
{
    /// <summary>
    /// Lógica de interacción para RegisterView.xaml
    /// </summary>
    public partial class RegisterView : Page
    {
        PlayerServicesClient playerServicesClient = new PlayerServicesClient();
        private MainWindow mainWindow;
        public RegisterView(MainWindow window)
        {
            InitializeComponent();
            disableErrorLabels();
            this.mainWindow = window;
        }

        private bool checkData()
        {
            bool hasEmptyField = false;

            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                labelEmailEmpty.Visibility = Visibility.Visible;
                hasEmptyField = true;
            }
            else
            {
                labelEmailEmpty.Visibility = Visibility.Hidden;
            }

            if (string.IsNullOrWhiteSpace(txtNickname.Text))
            {
                labelNicknameEmpty.Visibility = Visibility.Visible;
                hasEmptyField = true;
            }
            else
            {
                labelNicknameEmpty.Visibility = Visibility.Hidden;
            }

            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                labelNamesEmpty.Visibility = Visibility.Visible;
                hasEmptyField = true;
            }
            else
            {
                labelNamesEmpty.Visibility = Visibility.Hidden;
            }

            if (string.IsNullOrWhiteSpace(txtTelephone.Text))
            {
                labelTelephoneEmpty.Visibility = Visibility.Visible;
                hasEmptyField = true;
            }
            else
            {
                labelTelephoneEmpty.Visibility = Visibility.Hidden;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Password))
            {
                labelPasswordEmpty.Visibility = Visibility.Visible;
                hasEmptyField = true;
            }
            else
            {
                labelPasswordEmpty.Visibility = Visibility.Hidden;
            }

            if (string.IsNullOrWhiteSpace(txtPasswordConfirmation.Password))
            {
                labelPasswordConfirmationEmpty.Visibility = Visibility.Visible;
                hasEmptyField = true;
            }
            else
            {
                labelPasswordConfirmationEmpty.Visibility = Visibility.Hidden;
            }

            if (dpBirthDate.SelectedDate == null)
            {
                labelBirthDateEmpty.Visibility = Visibility.Visible;
                hasEmptyField = true;
            }
            else
            {
                labelBirthDateEmpty.Visibility = Visibility.Hidden;
            }

            return hasEmptyField;
        }

        private void disableErrorLabels()
        {
            labelBirthDateEmpty.Visibility = Visibility.Hidden;
            labelEmailEmpty.Visibility = Visibility.Hidden;
            labelNamesEmpty.Visibility = Visibility.Hidden;
            labelNicknameEmpty.Visibility = Visibility.Hidden;
            labelPasswordConfirmationEmpty.Visibility = Visibility.Hidden;
            labelPasswordEmpty.Visibility = Visibility.Hidden;
            labelTelephoneEmpty.Visibility = Visibility.Hidden;
        }

        private bool checkPasswords()
        {
            return !string.IsNullOrEmpty(txtPassword.Password) && !string.IsNullOrEmpty(txtPasswordConfirmation.Password);
        }


        private bool checkPasswordsMatch()
        {
            return txtPassword.Password == txtPasswordConfirmation.Password;
        }

        private async void Register_Click(object sender, RoutedEventArgs e)
        {
            if (!checkData())
            {
                if (checkPasswords())
                {
                    if (checkPasswordsMatch())
                    {
                        if (allValidate())
                        {
                            if (await playerServicesClient.NicknameAlreadyRegisteredAsync(txtNickname.Text))
                            {
                                string message = Properties.Resources.NicknameAlreadyRegistered;
                                ShowMessage(message, 2);
                            }
                            else if (await playerServicesClient.EmailAlreadyRegisteredAsync(txtEmail.Text))
                            {
                                string message = Properties.Resources.EmailAlreadyRegistered;
                                ShowMessage(message, 2);
                            }
                            else if (await playerServicesClient.TelephoneAlreadyExistAsync(txtTelephone.Text))
                            {
                                string message = Properties.Resources.TelephoneAlreadyRegistered;
                                ShowMessage(message, 2);
                            }
                            else
                            {
                                try
                                {
                                    Player newPlayer = createNewPlayer();
                                    bool confirmation = await playerServicesClient.RegisterPlayerAsync(newPlayer);
                                    if (confirmation)
                                    {
                                        string message = Properties.Resources.ConfirmationUserRegister;
                                        ShowMessage(message, 1);
                                        mainWindow.goToLoginView();
                                    }
                                    else
                                    {
                                        string message = Properties.Resources.ErrorUserRegister;
                                        ShowMessage(message, 3);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    ShowMessage(ex.Message, 3);
                                }
                            }
                        }
                    }
                    else
                    {
                        string message = Properties.Resources.PasswordDoesntMatch;
                        MessageBox.Show(message);
                    }
                }
            }
        }

        private Player createNewPlayer()
        {
            Player newPlayer = new Player();
            newPlayer.Email = txtEmail.Text.Trim();
            newPlayer.NickName = txtNickname.Text.Trim();
            newPlayer.Password = txtPassword.Password.Trim();
            newPlayer.PhoneNumber = txtTelephone.Text.Trim();
            newPlayer.FullName = txtFullName.Text.Trim();
            newPlayer.PointsEarned = 0;
            newPlayer.BirthDate = dpBirthDate.Text;

            return newPlayer;
        }

        private bool allValidate()
        {
            string email = txtEmail.Text.Trim();
            string nickName = txtNickname.Text.Trim();
            string fullName = txtFullName.Text.Trim();
            string password = txtPassword.Password.Trim();
            string phoneNumber = txtTelephone.Text.Trim();

            if (validateFullName(fullName) && validateNick(nickName)
                && validateEmail(email) && validatePassword(password) && validateTelephone(phoneNumber))
            {
                return true;
            }
            return false;
        }

        private bool validateFullName(string name)
        {

            if (Regex.IsMatch(name, @"^[a-zA-Z\s]+$"))
            {
                return true;
            }
            else
            {
                string message = Properties.Resources.NamesNotValid;
                ShowMessage(message, 2);
                return false;
            }
        }

        private bool validateTelephone(string phone)
        {
            string message = Properties.Resources.PhoneNotValid;
            if (phone.Length != 10)
            {
                MessageBox.Show(message);
                return false;
            }

            foreach (char c in phone)
            {
                if (!char.IsDigit(c))
                {
                    ShowMessage(message, 2);
                    txtTelephone.Clear();
                    return false;
                }
            }
            return true;
        }

        private bool validateEmail(string email)
        {
            string message = Properties.Resources.EmailNotValid;
            if (Regex.IsMatch(email, @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*" + "@"
                + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$"))
            {
                return true;
            }
            else
            {

                ShowMessage(message, 2);
                return false;
            }
        }

        private bool validateNick(string nickName)
        {
            string message = Properties.Resources.NicknameNotValid;
            if (Regex.IsMatch(nickName, @"^[a-zA-Z0-9]+$"))
            {
                return true;
            }
            else
            {
                ShowMessage(message, 2);
                return false;
            }
        }

        public bool validatePassword(string password)
        {
            string message = Properties.Resources.PasswordNotValid;
            if (Regex.IsMatch(password, @"^[a-zA-Z0-9]{8,15}$"))
            {
                return true;
            }
            else
            {
                ShowMessage(message, 2);
                return false;
            }
        }

        private void BtnChangeEnglish_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.changeLanguageRegister("en");
        }

        private void BtnChangeSpanish_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.changeLanguageRegister("es");
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.goToLoginView();
        }

        private void ShowMessage(string message, int type)
        {
            var dialog = new MessageBoxInformation(message, type);
            dialog.Owner = Application.Current.MainWindow;
            dialog.ShowDialog();
        }
    }
}