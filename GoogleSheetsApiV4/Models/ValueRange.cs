using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace GoogleSheetsApiV4.Models
{
    class ValueRange
    {
        [JsonProperty("range")]
        public string Range { get; set; }

        [JsonProperty("dimension")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Dimension Dimension { get; set; }

        [JsonProperty("values")]
        public List<List<object>> Values { get; set; }
    }
}
