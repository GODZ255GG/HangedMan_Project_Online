using System.Windows;

namespace HangedMan_Client.Views
{
    /// <summary>
    /// Lógica de interacción para QuestionMessage.xaml
    /// </summary>
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
