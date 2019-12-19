using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace GoogleSheetsApiV4.Models.Update
{
    class RequestBody
    {
        [JsonProperty("requests")]
        public object[] Requests { get; set; }

        [JsonProperty("includeSpreadsheetInResponse")]
        public bool IncludeSpreadsheetInResponse { get; set; }

        [JsonProperty("responseRanges")]
        public string[] ResponseRanges { get; set; }

        [JsonProperty("responseIncludeGridData")]
        public bool ResponseIncludeGridData { get; set; }
    }
}
