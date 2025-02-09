namespace GoogleSheetsApiV4.Contract
{
    public interface ICodeFlowManager
    {
        Task<HttpRequestMessage> CreateGetRequestAsync(string relativeUri);
        Task<HttpRequestMessage> CreatePostRequestAsync(string relativeUri);
    }
}
