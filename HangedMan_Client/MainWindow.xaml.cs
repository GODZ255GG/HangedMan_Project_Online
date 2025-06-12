using HangedMan_Client.Views;
using System.Windows;

namespace HangedMan_Client
{
    /*
    * Fecha creación: 22/05/2025
    * Última modificación: 05/06/2025
    * Último modificador: René Ulises
    * Descripción: Ventana principal de la aplicación WPF para el juego "Ahorcado". Gestiona la navegación entre las vistas de registro y login, y permite el cambio de idioma de la interfaz.
    */
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            GoToRegisterView();
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("es");
        }

        public void GoToRegisterView()
        {
            this.frame.Navigate(new RegisterView(this));
        }

        public void GoToLoginView()
        {
            this.frame.Navigate(new LoginView(this));
        }

        public void ChangeLanguageRegister(string cultureCode)
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(cultureCode);
            GoToRegisterView();
        }

        public void ChangeLanguageLogin(string cultureCode)
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(cultureCode);
            GoToLoginView();
        }
    }
}
