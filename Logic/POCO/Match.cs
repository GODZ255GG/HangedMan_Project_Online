using System.Data.Linq.Mapping;

namespace Logic.POCO
{
    /*
    * Fecha creación: 31/05/2025
    * Última modificación: 06/06/2025
    * Último modificador: René Ulises
    * Descripción: Clase POCO que representa la entidad "Partida" en la base de datos del juego "Ahorcado".
    *              Incluye propiedades para los datos de la partida, como identificadores, estado, jugadores, intentos restantes y configuración de idioma, mapeadas mediante LINQ to SQL.
    */
    [Table(Name = "Match")]
    public class Match
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int MatchID { get; set; }
        [Column]
        public int WordID { get; set; }
        [Column]
        public string DateMatch { get; set; }
        [Column]
        public int? WinnerID { get; set; }
        [Column]
        public int ChallengerID { get; set; }
        [Column]
        public int? GuestID { get; set; }
        [Column]
        public int StatusMatchID { get; set; }
        [Column]
        public char? SelectedLetter { get; set; }
        [Column]
        public int RemainingAttempts { get; set; }
        [Column]
        public string NickNameChallenger { get; set; }
        [Column]
        public string EmailChallenger { get; set; }
        [Column]
        public int MatchLanguage { get; set; }

    }
}
