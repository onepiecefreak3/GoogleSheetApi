using System.Text.Json.Serialization;

namespace GoogleSheetsApiV4.InternalContract.DataClasses
{
    internal class SheetPropertiesResponseData
    {
        [JsonPropertyName("sheetId")]
        public int SheetId { get; set; }
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("index")]
        public int Index { get; set; }
        [JsonPropertyName("sheetType")]
        public string SheetType { get; set; }
        [JsonPropertyName("gridProperties")]
        public GridPropertiesResponseData GridProperties { get; set; }
        [JsonPropertyName("tabColor")]
        public ColorResponseData TabColor { get; set; }
    }
}
