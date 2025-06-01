using System.Data.SqlClient;

namespace HangedMan_Server.Model
{
    public class ConnectionDB
    {
        public static SqlConnection getConnection()
        {
            string connectionString = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=Hanged_Man;Integrated Security=True";
            return new SqlConnection(connectionString);
        }
    }
}