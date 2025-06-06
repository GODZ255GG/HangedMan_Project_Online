using HangedMan_Client.HangedManService;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace HangedMan_Client.Views
{
    /*
    * Fecha creación: 26/05/2025
    * Última modificación: 05/06/2025
    * Último modificador: René Ulises
    * Descripción: Vista de WPF que permite al usuario editar su perfil, validando los datos ingresados y comunicándose con los servicios para actualizar la información del jugador.
    */
    public partial class EditProfileView : Page
    {
        private readonly PlayerServicesClient playerServices = new PlayerServicesClient();
        private readonly Player player;
        public EditProfileView(Player player)
        {
            InitializeComponent();
            this.player = player;
            DisableErrorLabels();
            ShowInformationPlayer();
        }

        private void ShowInformationPlayer()
        {
            txtFullName.Text = player.FullName;
            txtEmail.Text = player.Email;
            txtEmail.IsEnabled = false;
            txtTelephone.Text = player.PhoneNumber;
            txtNickname.Text = player.NickName;
            dpBirthDate.Text = player.BirthDate;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            string messageConfirm = Properties.Resources.CancelModifyConfirmation;
            var dialog = new QuestionMessage(messageConfirm)
            {
                Owner = Application.Current.MainWindow
            };
            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                NavigationService.Navigate(new ProfileView());
            }
        }


        private async void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckData())
            {
                if (CheckPasswords())
                {
                    if (CheckPasswordsMatch())
                    {
                        if (AllValidate())
                        {
                            try
                            {
                                player.FullName = txtFullName.Text.Trim();
                                player.NickName = txtNickname.Text.Trim();
                                player.Email = txtEmail.Text.Trim();
                                player.PhoneNumber = txtTelephone.Text.Trim();
                                player.Password = txtPassword.Password.Trim();
                                player.BirthDate = dpBirthDate.SelectedDate.Value.ToString("yyyy-MM-dd");

                                bool confirmation = await playerServices.UpdatePlayerProfileAsync(player);
                                if (confirmation)
                                {
                                    string message = Properties.Resources.ConfirmationModify;
                                    ShowMessage(message, 1);
                                    NavigationService.Navigate(new ProfileView());
                                }
                                else
                                {
                                    string message = Properties.Resources.ErrorModify;
                                    ShowMessage(message, 3);
                                }
                            }
                            catch (Exception ex)
                            {
                                ShowMessage(ex.Message, 3);
                            }
                        }
                    }
                    else
                    {
                        string message = Properties.Resources.PasswordDoesntMatch;
                        ShowMessage(message, 2);
                    }
                }
            }
        }

        private bool CheckData()
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

        private bool AllValidate()
        {
            string email = txtEmail.Text.Trim();
            string nickName = txtNickname.Text.Trim();
            string name = txtFullName.Text.Trim();
            string password = txtPassword.Password.Trim();
            string phoneNumber = txtTelephone.Text.Trim();

            if (ValidateNames(name) && ValidateNick(nickName)
                && ValidateEmail(email) && ValidatePassword(password) && ValidateTelephone(phoneNumber))
            {
                return true;
            }
            return false;
        }

        private bool ValidateNames(string name)
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

        private bool ValidateNick(string nickName)
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

        public bool ValidatePassword(string password)
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
