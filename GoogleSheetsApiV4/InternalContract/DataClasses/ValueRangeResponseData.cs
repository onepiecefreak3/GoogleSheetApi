using System.Text.Json.Serialization;

namespace GoogleSheetsApiV4.InternalContract.DataClasses
{
    internal class ValueRangeResponseData
    {
        [JsonPropertyName("range")]
        public string Range { get; set; }

        [JsonPropertyName("dimension")]
        public Dimension Dimension { get; set; }

        [JsonPropertyName("values")]
        public List<List<string>?> Values { get; set; }
    }
}
