using System.Text.Json.Serialization;

namespace GoogleSheetsApiV4.InternalContract.DataClasses
{
    internal class PropertiesResponseData
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("locale")]
        public string Locale { get; set; }
        [JsonPropertyName("autoRecalc")]
        public string AutoRecalculation { get; set; }
        [JsonPropertyName("timeZone")]
        public string TimeZone { get; set; }
        [JsonPropertyName("defaultFormat")]
        public DefaultFormatResponseData DefaultFormat { get; set; }
    }
}
