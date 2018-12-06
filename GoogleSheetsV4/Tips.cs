using GoogleSheetsApiV4.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleSheetsV4
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
        public string Finaltext { get; set; }
    }
}
