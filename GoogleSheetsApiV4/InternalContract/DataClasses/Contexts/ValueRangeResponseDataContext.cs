using System.Text.Json.Serialization;

namespace GoogleSheetsApiV4.InternalContract.DataClasses.Contexts
{
    [JsonSerializable(typeof(ValueRangeResponseData))]
    internal partial class ValueRangeResponseDataContext : JsonSerializerContext
    {
    }
}
