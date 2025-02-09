using System.Net;

namespace GoogleSheetsApiV4.InternalContract
{
    internal interface IOAuth2Listener
    {
        void Start(string consentUrl);
        Task<HttpListenerRequest> AwaitRequest();
    }
}
