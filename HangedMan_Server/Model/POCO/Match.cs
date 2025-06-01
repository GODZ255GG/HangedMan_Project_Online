using System.Data.Linq.Mapping;

namespace HangedMan_Server.Model.POCO
{
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