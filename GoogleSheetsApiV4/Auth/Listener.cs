using System;
using System.Net;
using GoogleSheetsApiV4.Support;

namespace GoogleSheetsApiV4.Auth
{
    /// <summary>
    /// A basic <see cref="HttpListener"/> to retrieve user input from a given url.
    /// </summary>
    class BasicListener : IDisposable
    {
        private HttpListener _listener;

        /// <summary>
        /// Creates a new instance of <see cref="BasicListener"/>.
        /// </summary>
        /// <param name="consentUrl">The url to listen to.</param>
        public BasicListener(string consentUrl)
        {
            Contract.EnsureNotNull(consentUrl, nameof(consentUrl));

            _listener = new HttpListener();
            _listener.Prefixes.Add(consentUrl);
        }

        /// <summary>
        /// Starts the listening to user input.
        /// </summary>
        /// <returns>This <see cref="BasicListener"/>.</returns>
        public BasicListener Start()
        {
            Contract.EnsureNotNull(_listener, "listener");

            _listener.Start();
            return this;
        }

        /// <summary>
        /// Await the user input request.
        /// </summary>
        /// <returns>The user input request.</returns>
        public HttpListenerRequest AwaitRequest()
        {
            Contract.EnsureNotNull(_listener, "listener");

            return _listener.GetContext().Request;
        }

        /// <inheritdoc cref="Dispose"/>
        public void Dispose()
        {
            _listener?.Stop();
            _listener = null;
        }
    }
}
