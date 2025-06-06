using System.Windows;

namespace HangedMan_Client.Views
{
    /*
    * Fecha creación: 05/06/2025
    * Última modificación: 05/06/2025
    * Último modificador: René Ulises
    * Descripción: Ventana de diálogo en WPF que muestra un mensaje de confirmación o pregunta al usuario, permitiendo aceptar o cancelar la acción solicitada.
    */
    public partial class QuestionMessage : Window
    {
        public QuestionMessage(string message)
        {
            InitializeComponent();
            this.txtbMessage.Text = message;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void BtnAcept_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
    }
}
