using System;
using System.Globalization;
using System.Windows.Data;

namespace HangedMan_Client.Resources
{
    /*
    * Fecha creación: 22/05/2025
    * Última modificación: 06/06/2025
    * Último modificador: René Ulises
    * Descripción: Este archivo facilita la presentación amigable del idioma en la interfaz gráfica del cliente del juego "Ahorcado", transformando IDs numéricos en nombres de idioma mediante recursos localizados.
    */
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