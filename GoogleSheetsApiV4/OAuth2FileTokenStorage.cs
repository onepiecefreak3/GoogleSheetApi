using System.Text.Json;
using GoogleSheetsApiV4.Contract;
using GoogleSheetsApiV4.Contract.DataClasses;
using GoogleSheetsApiV4.InternalContract.DataClasses;

namespace GoogleSheetsApiV4
{
    public class OAuth2FileTokenStorage : IOAuth2TokenStorage
    {
        private readonly TokenData _tokenData = new();
        private readonly string _filePath;

        public OAuth2FileTokenStorage(string filePath)
        {
            _filePath = filePath;
        }

        public Scope? GetScope() => _tokenData.Scope;

        public string? GetAccessToken() => _tokenData.AccessToken;

        public string? GetRefreshToken() => _tokenData.RefreshToken;

        public DateTime GetExpiration() => _tokenData.Expiration;

        public void SetScope(Scope scope) => _tokenData.Scope = scope;

        public void SetAccessToken(string accessToken) => _tokenData.AccessToken = accessToken;

        public void SetRefreshToken(string refreshToken) => _tokenData.RefreshToken = refreshToken;

        public void SetExpiration(DateTime expiration) => _tokenData.Expiration = expiration;

        public void Persist()
        {
            string? directory = Path.GetDirectoryName(_filePath);
            if (directory is not null)
                Directory.CreateDirectory(directory);

            string json = JsonSerializer.Serialize(_tokenData);
            File.WriteAllText(_filePath, json);
        }
    }
}
