using Newtonsoft.Json;

namespace GoogleSheetsApiV4.Models.Update
{
    class ExtendedValue
    {
        [JsonProperty("stringValue")]
        public string StringValue { get; set; }

        [JsonProperty("numberValue")]
        public long? NumberValue { get; set; }

        [JsonProperty("boolValue")]
        public bool? BoolValue { get; set; }

        [JsonProperty("formulaValue")]
        public string FormulaValue { get; set; }

        [JsonProperty("errorValue")]
        public ErrorValue ErrorValue { get; set; }
    }
}
