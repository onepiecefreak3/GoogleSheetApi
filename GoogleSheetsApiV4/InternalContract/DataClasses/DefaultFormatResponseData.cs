using System.Text.Json.Serialization;

namespace GoogleSheetsApiV4.InternalContract.DataClasses
{
    internal class DefaultFormatResponseData
    {
        [JsonPropertyName("backgroundColor")]
        public ColorResponseData BackgroundColor { get; set; }
        [JsonPropertyName("padding")]
        public PaddingResponseData Padding { get; set; }
        [JsonPropertyName("verticalAlignment")]
        public string VerticalAlignment { get; set; }
        [JsonPropertyName("wrapStrategy")]
        public string WrapStrategy { get; set; }
        [JsonPropertyName("textFormat")]
        public TextFormatResponseData TextFormat { get; set; }
    }
}
