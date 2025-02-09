using System.Text.Json.Serialization;

namespace GoogleSheetsApiV4.InternalContract.DataClasses
{
    internal class SheetObjectResponseData
    {
        [JsonPropertyName("spreadsheetId")]
        public string SpreadSheetId { get; set; }
        [JsonPropertyName("properties")]
        public PropertiesResponseData Properties { get; set; }
        [JsonPropertyName("sheets")]
        public List<SheetResponseData> Sheets { get; set; }
        [JsonPropertyName("spreadsheetUrl")]
        public string SpreadsheetUrl { get; set; }
    }
}
