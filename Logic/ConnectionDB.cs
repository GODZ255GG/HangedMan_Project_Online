using System;
using System.Configuration;
using System.Data.SqlClient;

namespace Logic
{
    /*
    * Fecha creación: 31/05/2025
    * Última modificación: 06/06/2025
    * Último modificador: René Ulises
    * Descripción: Clase de utilidad para gestionar la conexión a la base de datos SQL del juego "Ahorcado".
    *              Proporciona un método estático para obtener una instancia de SqlConnection configurada con los parámetros de conexión necesarios.
    */
    internal class ConnectionDB
    {
        public static SqlConnection GetConnection()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["HangedManDB"]?.ConnectionString;
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new InvalidOperationException("La cadena de conexión no está configurada.");
            return new SqlConnection(connectionString);
        }
    }
}
