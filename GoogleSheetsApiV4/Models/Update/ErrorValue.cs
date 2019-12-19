using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace GoogleSheetsApiV4.Models.Update
{
    class ErrorValue
    {
        [JsonProperty("type")]
        public ErrorType ErrorType { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
