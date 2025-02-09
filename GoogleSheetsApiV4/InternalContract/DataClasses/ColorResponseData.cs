using System.Text.Json.Serialization;

namespace GoogleSheetsApiV4.InternalContract.DataClasses
{
    internal class ColorResponseData
    {
        [JsonPropertyName("red")]
        public double Red { get; set; }
        [JsonPropertyName("green")]
        public double Green { get; set; }
        [JsonPropertyName("blue")]
        public double Blue { get; set; }
    }
}
