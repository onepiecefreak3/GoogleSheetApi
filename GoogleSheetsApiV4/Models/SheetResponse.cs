using System.Collections.Generic;
using Newtonsoft.Json;

namespace GoogleSheetsApiV4.Models
{
    public class SheetResponse
    {
        public string SpreadSheetId { get; set; }
        public Properties Properties { get; set; }
        public List<Sheet> Sheets { get; set; }
        //public List<> NamedRanges { get; set; }
        public string SpreadsheetUrl { get; set; }
    }
}
