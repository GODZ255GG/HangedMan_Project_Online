using System.Data.Linq.Mapping;

namespace Logic.POCO
{
    /*
    * Fecha creación: 31/05/2025
    * Última modificación: 06/06/2025
    * Último modificador: René Ulises
    * Descripción: Clase POCO que representa la entidad "Categoría" en la base de datos del juego "Ahorcado".
    *              Incluye propiedades para el identificador y los nombres de la categoría en español e inglés, mapeadas mediante LINQ to SQL.
    */
    [Table(Name = "Category")]
    public class Category
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int CategoryID { get; set; }
        [Column]
        public string SpanishCategory { get; set; }
        [Column]
        public string EnglishCategory { get; set; }
    }
}
