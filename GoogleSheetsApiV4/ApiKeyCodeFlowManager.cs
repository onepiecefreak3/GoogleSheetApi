using GoogleSheetsApiV4.InternalContract;

namespace GoogleSheetsApiV4
{
    public class ApiKeyCodeFlowManager : IApiKeyCodeFlowManager
    {
        private readonly string _apiKey;

        public ApiKeyCodeFlowManager(string apiKey)
        {
            _apiKey = apiKey;
        }

        public Task<HttpRequestMessage> CreateGetRequestAsync(string relativeUri)
        {
            relativeUri += $"?key={_apiKey}";

            var request = new HttpRequestMessage(HttpMethod.Get, relativeUri);

            return Task.FromResult(request);
        }

        public Task<HttpRequestMessage> CreatePostRequestAsync(string relativeUri)
        {
            relativeUri += $"?key={_apiKey}";

            var request = new HttpRequestMessage(HttpMethod.Post, relativeUri);

            return Task.FromResult(request);
        }
    }
}
