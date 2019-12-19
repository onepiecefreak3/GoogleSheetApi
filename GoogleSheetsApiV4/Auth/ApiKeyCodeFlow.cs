using System;
using GoogleSheetsApiV4.Models;
using GoogleSheetsApiV4.Support;
using RestSharp;

namespace GoogleSheetsApiV4.Auth
{
    /// <summary>
    /// Implements the api key code flow for authorization at google services.
    /// </summary>
    class ApiKeyCodeFlow : ICodeFlow
    {
        private readonly string _apiKey;

        /// <inheritdoc cref="Scope"/>
        public Scope Scope { get; }

        /// <inheritdoc cref="CanRead"/>
        public bool CanRead => Scope == Scope.ReadOnly || Scope == Scope.ReadWrite;

        /// <inheritdoc cref="CanWrite"/>
        public bool CanWrite => Scope == Scope.ReadWrite;

        /// <summary>
        /// Creates a new instance of <see cref="ApiKeyCodeFlow"/>.
        /// </summary>
        /// <param name="apiKey">The api key to authorize with.</param>
        /// <param name="scope">The scope of the sheet.</param>
        public ApiKeyCodeFlow(string apiKey, Scope scope)
        {
            Contract.EnsureNotNull(apiKey, nameof(apiKey));
            Contract.EnsureMemberOfEnum(scope, nameof(scope));

            if (scope != Scope.ReadOnly)
                throw new InvalidOperationException("ApiKey access can only be read-only.");

            _apiKey = apiKey;
            Scope = scope;
        }

        /// <inheritdoc cref="CreateRequest"/>
        public IRestRequest CreateRequest(string relativeUri)
        {
            var request = new RestRequest(relativeUri);

            request.AddQueryParameter("key", _apiKey);

            return request;
        }
    }
}
