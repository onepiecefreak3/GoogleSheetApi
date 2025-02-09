using GoogleSheetsApiV4.Contract.DataClasses;

namespace GoogleSheetsApiV4.Contract
{
    public interface IOAuth2TokenStorage
    {
        Scope? GetScope();
        string? GetAccessToken();
        string? GetRefreshToken();
        DateTime GetExpiration();

        void SetScope(Scope scope);
        void SetAccessToken(string accessToken);
        void SetRefreshToken(string refreshToken);
        void SetExpiration(DateTime expiration);

        void Persist();
    }
}
