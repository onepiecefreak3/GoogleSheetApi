using Newtonsoft.Json;

namespace GoogleSheetsApiV4.Models.Update
{
    class RowData
    {
        [JsonProperty("values")]
        public CellData[] Values { get; set; }
    }
}
