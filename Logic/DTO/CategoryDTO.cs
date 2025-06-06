using Logic.POCO;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Linq;

namespace Logic.DTO
{
    /*
    * Fecha creación: 31/05/2025
    * Última modificación: 06/06/2025
    * Último modificador: René Ulises
    * Descripción: Clase de acceso a datos (DTO) para la entidad "Categoría" del juego "Ahorcado".
    *              Proporciona métodos estáticos para consultar las categorías almacenadas en la base de datos utilizando LINQ to SQL.
    */
    public class CategoryDTO
    {
        public static List<Category> GetCategories()
        {
            try
            {
                var connection = ConnectionDB.GetConnection();
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
