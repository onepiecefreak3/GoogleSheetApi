using Newtonsoft.Json;

namespace GoogleSheetsApiV4.Models
{
    public class Properties
    {
        public string Title { get; set; }
        public string Locale { get; set; }
        [JsonProperty("autorecalc")]
        public string AutoRecalculation { get; set; }
        public string TimeZone { get; set; }
        public DefaultFormat DefaultFormat { get; set; }
    }
}
