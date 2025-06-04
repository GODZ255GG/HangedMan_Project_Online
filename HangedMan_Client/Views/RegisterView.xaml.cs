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
        private readonly PlayerServicesClient playerServicesClient = new PlayerServicesClient();
        private readonly MainWindow mainWindow;
        public RegisterView(MainWindow window)
        {
            InitializeComponent();
            DisableErrorLabels();
            this.mainWindow = window;
        }

        private bool CheckData()
        {
            bool hasEmptyField = false;

            bool CheckField(Func<string> getValue, Label errorLabel)
            {
                if (string.IsNullOrWhiteSpace(getValue()))
                {
                    errorLabel.Visibility = Visibility.Visible;
                    return true;
                }
                else
                {
                    errorLabel.Visibility = Visibility.Hidden;
                    return false;
                }
            }

            hasEmptyField |= CheckField(() => txtEmail.Text, labelEmailEmpty);
            hasEmptyField |= CheckField(() => txtNickname.Text, labelNicknameEmpty);
            hasEmptyField |= CheckField(() => txtFullName.Text, labelNamesEmpty);
            hasEmptyField |= CheckField(() => txtTelephone.Text, labelTelephoneEmpty);
            hasEmptyField |= CheckField(() => txtPassword.Password, labelPasswordEmpty);
            hasEmptyField |= CheckField(() => txtPasswordConfirmation.Password, labelPasswordConfirmationEmpty);

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

        private void DisableErrorLabels()
        {
            labelBirthDateEmpty.Visibility = Visibility.Hidden;
            labelEmailEmpty.Visibility = Visibility.Hidden;
            labelNamesEmpty.Visibility = Visibility.Hidden;
            labelNicknameEmpty.Visibility = Visibility.Hidden;
            labelPasswordConfirmationEmpty.Visibility = Visibility.Hidden;
            labelPasswordEmpty.Visibility = Visibility.Hidden;
            labelTelephoneEmpty.Visibility = Visibility.Hidden;
        }

        private bool CheckPasswords()
        {
            return !string.IsNullOrEmpty(txtPassword.Password) && !string.IsNullOrEmpty(txtPasswordConfirmation.Password);
        }


        private bool CheckPasswordsMatch()
        {
            return txtPassword.Password == txtPasswordConfirmation.Password;
        }

        private async void Register_Click(object sender, RoutedEventArgs e)
        {
            if (CheckData())
                return;

            if (!CheckPasswords())
                return;

            if (!CheckPasswordsMatch())
            {
                ShowMessage(Properties.Resources.PasswordDoesntMatch,2);
                return;
            }

            if (!AllValidate())
                return;

            if (await playerServicesClient.NicknameAlreadyRegisteredAsync(txtNickname.Text))
            {
                ShowMessage(Properties.Resources.NicknameAlreadyRegistered, 2);
                return;
            }
            if (await playerServicesClient.EmailAlreadyRegisteredAsync(txtEmail.Text))
            {
                ShowMessage(Properties.Resources.EmailAlreadyRegistered, 2);
                return;
            }
            if (await playerServicesClient.TelephoneAlreadyExistAsync(txtTelephone.Text))
            {
                ShowMessage(Properties.Resources.TelephoneAlreadyRegistered, 2);
                return;
            }

            try
            {
                Player newPlayer = CreateNewPlayer();
                bool confirmation = await playerServicesClient.RegisterPlayerAsync(newPlayer);
                if (confirmation)
                {
                    ShowMessage(Properties.Resources.ConfirmationUserRegister, 1);
                    mainWindow.goToLoginView();
                }
                else
                {
                    ShowMessage(Properties.Resources.ErrorUserRegister, 3);
                }
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, 3);
            }
        }

        private Player CreateNewPlayer()
        {
            Player newPlayer = new Player
            {
                Email = txtEmail.Text.Trim(),
                NickName = txtNickname.Text.Trim(),
                Password = txtPassword.Password.Trim(),
                PhoneNumber = txtTelephone.Text.Trim(),
                FullName = txtFullName.Text.Trim(),
                PointsEarned = 0,
                BirthDate = dpBirthDate.Text
            };

            return newPlayer;
        }

        private bool AllValidate()
        {
            string email = txtEmail.Text.Trim();
            string nickName = txtNickname.Text.Trim();
            string fullName = txtFullName.Text.Trim();
            string password = txtPassword.Password.Trim();
            string phoneNumber = txtTelephone.Text.Trim();

            if (ValidateFullName(fullName) && ValidateNick(nickName)
                && ValidateEmail(email) && ValidatePassword(password) && ValidateTelephone(phoneNumber))
            {
                return true;
            }
            return false;
        }

        private bool ValidateFullName(string name)
        {

            if (Regex.IsMatch(name, @"^[a-zA-Z\s]+$"))
            {
                return true;
            }
            else
            {
                ShowMessage(Properties.Resources.NamesNotValid, 2);
                return false;
            }
        }

        private bool ValidateTelephone(string phone)
        {
            string message = Properties.Resources.PhoneNotValid;
            if (phone.Length != 10)
            {
                ShowMessage(message, 2);
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

        private bool ValidateEmail(string email)
        {
            if (Regex.IsMatch(email, @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*" + "@"
                + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$"))
            {
                return true;
            }
            else
            {
                ShowMessage(Properties.Resources.EmailNotValid, 2);
                return false;
            }
        }

        private bool ValidateNick(string nickName)
        {
            if (Regex.IsMatch(nickName, @"^[a-zA-Z0-9]+$"))
            {
                return true;
            }
            else
            {
                ShowMessage(Properties.Resources.NicknameNotValid, 2);
                return false;
            }
        }

        public bool ValidatePassword(string password)
        {
            if (Regex.IsMatch(password, @"^[a-zA-Z0-9]{8,15}$"))
            {
                return true;
            }
            else
            {
                ShowMessage(Properties.Resources.PasswordNotValid, 2);
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
            var dialog = new MessageBoxInformation(message, type)
            {
                Owner = Application.Current.MainWindow
            };
            dialog.ShowDialog();
        }
    }
}