using HangedMan_Client.Views;
using System;
using System.Windows;

namespace HangedMan_Client
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            goToRegisterView();
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("es");
        }

        public void goToRegisterView()
        {
            this.frame.Navigate(new RegisterView(this));
        }

        public void goToLoginView()
        {
            this.frame.Navigate(new LoginView(this));
        }

        public void changeLanguageRegister(string cultureCode)
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(cultureCode);
            goToRegisterView();
        }

        public void changeLanguageLogin(string cultureCode)
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(cultureCode);
            goToLoginView();
        }
    }
}
