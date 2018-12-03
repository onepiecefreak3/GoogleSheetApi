using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleSheetsV4.Models
{
    public class SheetResponse
    {
        public string SpreadSheetId { get; set; }
        public Properties Properties { get; set; }
        public List<Sheet> Sheets { get; set; }
        //public List<> NamedRanges { get; set; }
        public string SpreadsheetUrl { get; set; }
    }

    public class Properties
    {
        public string Title { get; set; }
        public string Locale { get; set; }
        public string AutoRecalc { get; set; }
        public string TimeZone { get; set; }
        public DefaultFormat DefaultFormat { get; set; }
    }

    public class DefaultFormat
    {
        public General.Color BackgroundColor { get; set; }
        public Padding Padding { get; set; }
        public string VerticalAlignment { get; set; }
        public string WrapStrategy { get; set; }
        public TextFormat TextFormat { get; set; }
    }

    public class Padding
    {
        public int? Top { get; set; }
        public int? Right { get; set; }
        public int? Bottom { get; set; }
        public int? Left { get; set; }
    }

    public class TextFormat
    {
        public General.Color ForeGroundColor { get; set; }
        public string FontFamily { get; set; }
        public int FontSize { get; set; }
        public bool Bold { get; set; }
        public bool Italic { get; set; }
        public bool Strikethrough { get; set; }
        public bool Underline { get; set; }
    }
}
