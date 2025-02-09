using System.Text.Json.Serialization;

namespace GoogleSheetsApiV4.InternalContract.DataClasses
{
    internal class SheetResponseData
    {
        [JsonPropertyName("properties")]
        public SheetPropertiesResponseData Properties { get; set; }
    }
}
