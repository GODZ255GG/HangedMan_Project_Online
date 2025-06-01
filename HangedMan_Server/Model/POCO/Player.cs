using System.Data.Linq.Mapping;

namespace HangedMan_Server.Model.POCO
{
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