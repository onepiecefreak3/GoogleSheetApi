using System.Text.Json.Serialization;

namespace GoogleSheetsApiV4.InternalContract.DataClasses.Contexts
{
    [JsonSerializable(typeof(OAuth2TokenResponseData))]
    internal partial class OAuth2TokenResponseDataContext : JsonSerializerContext
    {
    }
}
