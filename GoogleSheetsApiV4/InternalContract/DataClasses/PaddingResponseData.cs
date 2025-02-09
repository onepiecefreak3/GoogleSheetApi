using System.Text.Json.Serialization;

namespace GoogleSheetsApiV4.InternalContract.DataClasses
{
    internal class PaddingResponseData
    {
        [JsonPropertyName("top")]
        public int? Top { get; set; }
        [JsonPropertyName("right")]
        public int? Right { get; set; }
        [JsonPropertyName("bottom")]
        public int? Bottom { get; set; }
        [JsonPropertyName("left")]
        public int? Left { get; set; }
    }
}
