using GoogleSheetsApiV4.Attributes;

namespace GoogleSheetsV4.Models
{
    public class Tips
    {
        [Column("B")]
        public int ChapterId { get; set; }

        [Column("E")]
        public string OriginalTitle { get; set; }

        [Column("F")]
        public string OriginalTitleEscaped { get; set; }

        [Column("G")]
        public string TranslatedTitle { get; set; }

        [Column("K")]
        public string FinalText { get; set; }
    }
}
