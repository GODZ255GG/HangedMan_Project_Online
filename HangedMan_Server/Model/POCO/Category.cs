using System.Data.Linq.Mapping;

namespace HangedMan_Server.Model.POCO
{
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