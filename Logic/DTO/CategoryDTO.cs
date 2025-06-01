using Logic.POCO;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Linq;

namespace Logic.DTO
{
    public class CategoryDTO
    {
        public static List<Category> GetCategories()
        {
            try
            {
                var connection = ConnectionDB.getConnection();
                connection.Open();
                DataContext dataContext = new DataContext(connection);
                var categories = (from cat in dataContext.GetTable<Category>()
                                  select cat).ToList();
                return categories;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}
