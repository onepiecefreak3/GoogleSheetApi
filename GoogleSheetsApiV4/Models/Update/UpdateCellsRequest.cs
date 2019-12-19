using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace GoogleSheetsApiV4.Models.Update
{
    class UpdateCellsRequest
    {
        [JsonProperty("rows")]
        public RowData[] Rows { get; set; }

        [JsonProperty("fields")] 
        public string Fields { get; set; } = "*";

        [JsonProperty("range")]
        public GridRange Range { get; set; }
    }
}
