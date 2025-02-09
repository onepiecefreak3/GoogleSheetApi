using GoogleSheetsApiV4.Contract.DataClasses;

namespace GoogleSheetsApiV4.InternalContract.DataClasses
{
    internal class TokenData
    {
        public Scope? Scope { get; set; }
        public DateTime Expiration { get; set; }
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
