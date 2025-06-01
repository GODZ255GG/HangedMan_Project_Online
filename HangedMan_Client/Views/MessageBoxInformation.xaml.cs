using System;
using System.Windows;

namespace HangedMan_Client.Views
{
    public partial class MessageBoxInformation : Window
    {
        public MessageBoxInformation(string message, int type)
        {
            InitializeComponent();
            UploadMessage(message, type);
        }

        private void BtnAcept_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void UploadMessage(string message, int type)
        {
            switch (type)
            {
                case 1: // Information
                    imgIcon.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/Resources/iconInformation.png"));
                    txtbMessage.Text = message;
                    break;
                case 2: // Warning
                    imgIcon.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/Resources/iconWarning.png"));
                    txtbMessage.Text = message;
                    break;
                case 3: // Error
                    imgIcon.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/Resources/iconLoss.png"));
                    txtbMessage.Text = message;
                    break;
            }
        }
    }
}
