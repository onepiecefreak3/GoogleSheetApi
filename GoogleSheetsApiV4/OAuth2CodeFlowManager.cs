using System.Net.Http.Headers;
using GoogleSheetsApiV4.Contract;
using GoogleSheetsApiV4.Contract.DataClasses;
using GoogleSheetsApiV4.InternalContract;

namespace GoogleSheetsApiV4
{
    public class OAuth2CodeFlowManager : IOAuth2CodeFlowManager
    {
        private readonly IOAuth2TokenProvider _tokenProvider;

        public OAuth2CodeFlowManager(IOAuth2TokenProvider tokenProvider)
        {
            _tokenProvider = tokenProvider;
        }

        public static OAuth2CodeFlowManager Create(Scope scope, string clientId, string clientSecret, IOAuth2TokenStorage tokenStorage)
        {
            var tokenListener = new OAuth2Listener();
            var tokenProvider = new OAuth2TokenProvider(scope, clientId, clientSecret, tokenListener, tokenStorage);
            return new OAuth2CodeFlowManager(tokenProvider);
        }

        public async Task<HttpRequestMessage> CreateGetRequestAsync(string relativeUri)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, relativeUri);

            string accessToken = await _tokenProvider.GetAccessToken() ?? string.Empty;
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            return request;
        }

        public async Task<HttpRequestMessage> CreatePostRequestAsync(string relativeUri)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, relativeUri);

            string accessToken = await _tokenProvider.GetAccessToken() ?? string.Empty;
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            return request;
        }
    }
}
