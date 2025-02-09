using System.Text.Json.Serialization;

namespace GoogleSheetsApiV4.InternalContract.DataClasses.Contexts
{
    [JsonSerializable(typeof(SheetObjectResponseData))]
    internal partial class SheetObjectResponseDataContext : JsonSerializerContext
    {
    }
}
