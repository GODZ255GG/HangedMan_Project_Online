using System;
using System.Globalization;
using System.Windows.Data;

namespace HangedMan_Client.Resources
{
    public class LanguageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int languageId)
            {
                return languageId == 1 ?
                    Properties.Resources.SpanishLanguage :
                    Properties.Resources.EnglishLanguage;
            }
            return Properties.Resources.UnknownLanguage;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}