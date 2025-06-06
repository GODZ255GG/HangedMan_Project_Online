using System.Data.Linq.Mapping;

namespace Logic.POCO
{
    /*
    * Fecha creación: 31/05/2025
    * Última modificación: 06/06/2025
    * Último modificador: René Ulises
    * Descripción: Clase POCO que representa la entidad "Jugador" en la base de datos del juego "Ahorcado".
    *              Incluye propiedades para los datos del jugador, como identificador, correo, contraseña, apodo, nombre completo, fecha de nacimiento, teléfono y puntos obtenidos, mapeadas mediante LINQ to SQL.
    */
    [Table(Name = "Player")]
    public class Player
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int PlayerID { get; set; }
        [Column]
        public string Email { get; set; }
        [Column]
        public string Password { get; set; }
        [Column]
        public string NickName { get; set; }
        [Column]
        public string FullName { get; set; }
        [Column]
        public string BirthDate { get; set; }
        [Column]
        public string PhoneNumber { get; set; }
        [Column]
        public int PointsEarned { get; set; }
    }
}
