using System.Text.Json.Serialization;

namespace GoogleSheetsApiV4.InternalContract.DataClasses.Contexts
{
    [JsonSerializable(typeof(PostUpdateCellsRequestData))]
    internal partial class PostUpdateCellsRequestDataContext : JsonSerializerContext
    {
    }
}
