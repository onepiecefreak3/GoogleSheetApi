namespace GoogleSheetsApiV4.InternalContract
{
    public interface IOAuth2TokenProvider
    {
        Task<string?> GetAccessToken();
    }
}
