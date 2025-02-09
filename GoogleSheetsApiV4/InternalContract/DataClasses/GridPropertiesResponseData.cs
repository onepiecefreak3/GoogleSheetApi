using System.Text.Json.Serialization;

namespace GoogleSheetsApiV4.InternalContract.DataClasses
{
    internal class GridPropertiesResponseData
    {
        [JsonPropertyName("rowCount")]
        public int RowCount { get; set; }
        [JsonPropertyName("columnCount")]
        public int ColumnCount { get; set; }
    }
}
