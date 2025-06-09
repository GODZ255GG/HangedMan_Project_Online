using System.Data.Linq.Mapping;

namespace Logic.POCO
{
    /*
    * Fecha creación: 31/05/2025
    * Última modificación: 06/06/2025
    * Último modificador: René Ulises
    * Descripción: Clase POCO que representa la entidad "Palabra" en la base de datos del juego "Ahorcado".
    *              Incluye propiedades para los datos de la palabra en español e inglés, sus pistas y la categoría asociada, mapeadas mediante LINQ to SQL.
    */
    [Table(Name = "Word")]
    public class Word
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int WordID { get; set; }
        [Column]
        public string EnglishWord { get; set; }
        [Column]
        public string SpanishWord { get; set; }
        [Column]
        public string EnglishClue { get; set; }
        [Column]
        public string SpanishClue { get; set; }
        [Column]
        public int CategoryID { get; set; }
    }
}
