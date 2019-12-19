using Newtonsoft.Json;

namespace GoogleSheetsApiV4.Models.Update
{
    class CellData
    {
        [JsonProperty("userEnteredValue")]
        public ExtendedValue UserEnteredValue { get; set; }
    }
}
