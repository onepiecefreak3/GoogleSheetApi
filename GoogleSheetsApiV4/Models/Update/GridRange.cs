using Newtonsoft.Json;

namespace GoogleSheetsApiV4.Models.Update
{
    class GridRange
    {
        [JsonProperty("sheetId")]
        public int SheetId { get; set; }

        [JsonProperty("startColumnIndex")]
        public int? StartColumnIndex { get; set; }

        [JsonProperty("endColumnIndex")]
        public int? EndColumnIndex { get; set; }

        [JsonProperty("startRowIndex")]
        public int? StartRowIndex { get; set; }

        [JsonProperty("endRowIndex")]
        public int? EndRowIndex { get; set; }
    }
}
