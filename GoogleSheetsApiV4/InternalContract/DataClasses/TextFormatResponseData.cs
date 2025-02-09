using System.Text.Json.Serialization;

namespace GoogleSheetsApiV4.InternalContract.DataClasses
{
    internal class TextFormatResponseData
    {
        [JsonPropertyName("foregroundColor")]
        public ColorResponseData ForeGroundColor { get; set; }
        [JsonPropertyName("fontFamily")]
        public string FontFamily { get; set; }
        [JsonPropertyName("fontSize")]
        public int FontSize { get; set; }
        [JsonPropertyName("bold")]
        public bool Bold { get; set; }
        [JsonPropertyName("italic")]
        public bool Italic { get; set; }
        [JsonPropertyName("strikethrough")]
        public bool StrikeThrough { get; set; }
        [JsonPropertyName("underline")]
        public bool Underline { get; set; }
    }
}
