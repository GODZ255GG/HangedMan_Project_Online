using System.Data.Linq.Mapping;

namespace Logic.POCO
{
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
