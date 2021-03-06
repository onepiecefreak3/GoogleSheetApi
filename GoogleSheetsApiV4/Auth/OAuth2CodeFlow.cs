﻿using GoogleSheetsApiV4.Models;
using GoogleSheetsApiV4.Support;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Text;
using RestSharp;

namespace GoogleSheetsApiV4.Auth
{
    /// <summary>
    /// Implements the OAuth2 flow for authorization at google services.
    /// </summary>
    class OAuth2CodeFlow : ICodeFlow
    {
        private readonly Dictionary<Scope, string> _scopeUrls = new Dictionary<Scope, string>
        {
            [Scope.ReadOnly] = "https://www.googleapis.com/auth/spreadsheets.readonly",
            [Scope.ReadWrite] = "https://www.googleapis.com/auth/spreadsheets"
        };

        private readonly Random _rand = new Random();
        private readonly string _clientId;
        private readonly string _clientSecret;

        private string _baseAuthUrl = "https://accounts.google.com/o/";
        private string _authResource = "oauth2/v2/auth";

        private string _baseTokenUrl = "https://www.googleapis.com/";
        private string _tokenResource = "oauth2/v4/token";

        private string _redirectUri = "http://localhost:6001/";

        private string _accessToken;
        private string _refreshToken;
        private DateTime _expiration;

        /// <inheritdoc cref="Scope"/>
        public Scope Scope { get; }

        /// <inheritdoc cref="CanRead"/>
        public bool CanRead => Scope == Scope.ReadOnly || Scope == Scope.ReadWrite;

        /// <inheritdoc cref="CanWrite"/>
        public bool CanWrite => Scope == Scope.ReadWrite;

        /// <summary>
        /// Initializes a new instance of <see cref="OAuth2CodeFlow"/>.
        /// </summary>
        /// <param name="clientId">The client id to authorize with.</param>
        /// <param name="clientSecret">The client secret to authorize with.</param>
        /// <param name="scope">The scope of the sheet.</param>
        public OAuth2CodeFlow(string clientId, string clientSecret, Scope scope)
        {
            Contract.EnsureNotNull(clientId, nameof(clientId));
            Contract.EnsureNotNull(clientSecret, nameof(clientSecret));
            Contract.EnsureMemberOfEnum(scope, nameof(scope));

            _clientId = clientId;
            _clientSecret = clientSecret;
            Scope = scope;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="OAuth2CodeFlow"/>.
        /// </summary>
        /// <param name="clientId">The client id to authorize with.</param>
        /// <param name="clientSecret">The client secret to authorize with.</param>
        /// <param name="scope">The scope of the sheet.</param>
        /// <param name="refreshToken">The RefreshToken for this flow.</param>
        public OAuth2CodeFlow(string clientId, string clientSecret, Scope scope, string refreshToken)
        {
            Contract.EnsureNotNull(clientId, nameof(clientId));
            Contract.EnsureNotNull(clientSecret, nameof(clientSecret));
            Contract.EnsureNotNull(refreshToken, nameof(refreshToken));
            Contract.EnsureMemberOfEnum(scope, nameof(scope));

            _clientId = clientId;
            _clientSecret = clientSecret;
            _refreshToken = refreshToken;
            Scope = scope;
        }

        /// <inheritdoc cref="CreateRequest"/>
        public IRestRequest CreateRequest(string relativeUri)
        {
            var request = new RestRequest(relativeUri);

            request.AddHeader("Authorization", $"Bearer {RetrieveAccessToken()}");

            return request;
        }

        /// <summary>
        /// Retrieves a valid access token.
        /// </summary>
        /// <returns>A valid access token.</returns>
        private string RetrieveAccessToken()
        {
            // If access token is not expired, return it
            if (_expiration > DateTime.UtcNow)
                return _accessToken;

            // If the refresh token is set, use it to refresh the access token
            if (!string.IsNullOrEmpty(_refreshToken))
                return RefreshAccessToken();

            // Otherwise create a new set of access and refresh token
            return CreateAccessToken();
        }

        #region Private methods

        /// <summary>
        /// Create a new pair of access and refresh token.
        /// </summary>
        /// <returns>Valid access token.</returns>
        private string CreateAccessToken()
        {
            if (!IsUserConsent(out var authCode, out var error))
                throw new InvalidOperationException(error);

            var tokenResponse = ExchangeAuthCode(authCode);
            _accessToken = tokenResponse.AccessToken;
            _refreshToken = tokenResponse.RefreshToken;
            _expiration = DateTime.Now + TimeSpan.FromSeconds(tokenResponse.ExpiresIn);

            return _accessToken;
        }

        /// <summary>
        /// Create a new access token by using the refresh token.
        /// </summary>
        /// <returns>Valid access token.</returns>
        private string RefreshAccessToken()
        {
            var tokenResponse = ExchangeRefreshToken();
            _accessToken = tokenResponse.AccessToken;
            _expiration = DateTime.UtcNow + TimeSpan.FromSeconds(tokenResponse.ExpiresIn);

            return _accessToken;
        }

        /// <summary>
        /// Send authorization data and code to create a new pair of access and refresh token.
        /// </summary>
        /// <param name="code">The auth code for the creation.</param>
        /// <returns>The authorization response.</returns>
        private TokenResponse ExchangeAuthCode(string code)
        {
            var client = new RestClient(_baseTokenUrl);

            var request = new RestRequest(_tokenResource);
            request.AddQueryParameter("code", code);
            request.AddQueryParameter("client_id", _clientId);
            request.AddQueryParameter("client_secret", _clientSecret);
            request.AddQueryParameter("redirect_uri", _redirectUri);
            request.AddQueryParameter("grant_type", "authorization_code");

            var response = client.Post<TokenResponse>(request);
            if (!response.IsSuccessful)
                throw new InvalidDataException(response.ErrorMessage);

            return response.Data;
        }

        /// <summary>
        /// Send authorization data and refreshToken to refresh the access token.
        /// </summary>
        /// <returns>The authorization response.</returns>
        private TokenResponse ExchangeRefreshToken()
        {
            var client = new RestClient(_baseTokenUrl);

            var request = new RestRequest(_tokenResource);
            request.AddQueryParameter("refresh_token", _refreshToken);
            request.AddQueryParameter("client_id", _clientId);
            request.AddQueryParameter("client_secret", _clientSecret);
            request.AddQueryParameter("grant_type", "refresh_token");

            var response = client.Post<TokenResponse>(request);
            if (!response.IsSuccessful)
                throw new InvalidDataException(response.ErrorMessage);

            return response.Data;
        }

        /// <summary>
        /// Asks the user to give access consent to the requested data.
        /// </summary>
        /// <param name="code">The authorization code after user gave consent.</param>
        /// <param name="error">The possible error if user did not give consent.</param>
        /// <returns>Is user consent for accessing the data.</returns>
        private bool IsUserConsent(out string code, out string error)
        {
            error = null;
            code = null;

            var state = GetRandomString(10);
            var authQuery = RetrieveAuthQuery(state);

            if (authQuery["state"] != state)
            {
                error = "The state was corrupted.";
                return false;
            }

            code = authQuery["code"];
            return true;
        }

        /// <summary>
        /// Retrieve the authorization response from the user.
        /// </summary>
        /// <param name="state">The state the response will be connected to.</param>
        /// <returns>The authorization response.</returns>
        private NameValueCollection RetrieveAuthQuery(string state)
        {
            using var listener = new BasicListener(_redirectUri).Start();
            OpenUrl(GetAuthUri(state));

            var request = listener.AwaitRequest();
            return request.QueryString;
        }

        /// <summary>
        /// Build uri to request consent from the user.
        /// </summary>
        /// <param name="state">The state the response will be connected to.</param>
        /// <returns>The authorization uri.</returns>
        private string GetAuthUri(string state)
        {
            var client = new RestClient(_baseAuthUrl);

            var request = new RestRequest(_authResource);
            request.AddQueryParameter("client_id", _clientId);
            request.AddQueryParameter("response_type", "code");
            request.AddQueryParameter("scope", $"{_scopeUrls[Scope]}");
            request.AddQueryParameter("redirect_uri", _redirectUri);
            request.AddQueryParameter("state", state);
            request.AddQueryParameter("nonce", GetRandomString(10));
            request.AddQueryParameter("access_type", "offline");

            return client.BuildUri(request).AbsoluteUri;
        }

        /// <summary>
        /// Open a specified url.
        /// </summary>
        /// <param name="url">The url to open.</param>
        /// <remarks>https://github.com/dotnet/corefx/issues/10361</remarks>
        private void OpenUrl(string url)
        {
            Contract.EnsureNotNull(url, nameof(url));

            var psi = new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            };

            Process.Start(psi);
        }

        /// <summary>
        /// Creates a string with random content.
        /// </summary>
        /// <param name="length">The arbitrary length of the string.</param>
        /// <returns>Randomized string.</returns>
        private string GetRandomString(int length)
        {
            var sb = new StringBuilder(length);

            for (var i = 0; i < length; i++)
            {
                var randChar = (char)_rand.Next(0x41, 0x5A);
                sb.Append(randChar);
            }

            return sb.ToString();
        }

        #endregion
    }
}
