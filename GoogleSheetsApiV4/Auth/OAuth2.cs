using GoogleSheetsApiV4.Models;
using GoogleSheetsApiV4.Support;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GoogleSheetsApiV4.Auth
{
    internal class OAuth2
    {
        private Dictionary<Scope, string> _scopeUrls = new Dictionary<Scope, string>
        {
            [Scope.ReadOnly] = "https://www.googleapis.com/auth/spreadsheets.readonly",
            [Scope.ReadWrite] = "https://www.googleapis.com/auth/spreadsheets"
        };

        private string _baseAuthUrl = "https://accounts.google.com/o/";
        private string _authResource = "oauth2/v2/auth";

        private string _baseTokenUrl = "https://www.googleapis.com/";
        private string _tokenResource = "oauth2/v4/token";

        private string _redirectUri = "http://localhost:6001/";

        private string _clientId;
        private string _clientSecret;

        private string _accessToken;
        private string _refreshToken;
        private DateTime _expiration;
        private Scope _scope;

        private Random _rand = new Random();

        public OAuth2(string clientId, string clientSecret, Scope scope)
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
            _scope = scope;
        }

        public string RetrieveAccessToken()
        {
            if (_expiration > DateTime.Now)
                return _accessToken;

            if (!string.IsNullOrEmpty(_refreshToken))
                return RefreshAccessToken();

            return CreateAccessToken();
        }

        private string CreateAccessToken()
        {
            if (!IsUserConsent(out var output))
                throw new InvalidOperationException(output);

            var tokenResponse = ExchangeCode(output);
            _accessToken = tokenResponse.Access_token;
            _refreshToken = tokenResponse.Refresh_token;
            _expiration = DateTime.Now + TimeSpan.FromSeconds(tokenResponse.Expires_in);

            return _accessToken;
        }

        //TODO: Refresh accessToken
        private string RefreshAccessToken()
        {
            var tokenResponse = ExchangeRefreshToken();
            _accessToken = tokenResponse.Access_token;
            //_refreshToken = tokenResponse.Refresh_token;
            _expiration = DateTime.Now + TimeSpan.FromSeconds(tokenResponse.Expires_in);

            return _accessToken;
        }

        private TokenResponse ExchangeRefreshToken()
        {
            var client = new Client(_baseTokenUrl);

            client.AddQueryParameter("refresh_token", _refreshToken);
            client.AddQueryParameter("client_id", _clientId);
            client.AddQueryParameter("client_secret", _clientSecret);
            client.AddQueryParameter("grant_type", "refresh_token");

            var response = client.Post(_tokenResource);
            if (response.IsSuccessful)
            {
                return JObject.Parse(response.Content).ToObject<TokenResponse>();
            }
            else
            {
                throw new InvalidDataException(response.ErrorMessage);
            }
        }

        private TokenResponse ExchangeCode(string code)
        {
            var client = new Client(_baseTokenUrl);

            client.AddQueryParameter("code", code);
            client.AddQueryParameter("client_id", _clientId);
            client.AddQueryParameter("client_secret", _clientSecret);
            client.AddQueryParameter("redirect_uri", _redirectUri);
            client.AddQueryParameter("grant_type", "authorization_code");

            var response = client.Post(_tokenResource);
            if (response.IsSuccessful)
            {
                return JObject.Parse(response.Content).ToObject<TokenResponse>();
            }
            else
            {
                throw new InvalidDataException(response.ErrorMessage);
            }
        }

        private bool IsUserConsent(out string output)
        {
            output = string.Empty;

            var state = GetRandomString(10);
            var authQuery = RetrieveAuthQuery(state);

            if (authQuery["state"] != state)
            {
                output = "The state was corrupted.";
                return false;
            }

            output = authQuery["code"];

            return true;
        }

        private NameValueCollection RetrieveAuthQuery(string state)
        {
            using (var listener = new BasicListener(_redirectUri).Start())
            {
                var proc = Process.Start(GetAuthUri(state));

                var request = listener.AwaitRequest();
                return request.QueryString;
            }
        }

        private string GetAuthUri(string state)
        {
            var client = new Client(_baseAuthUrl);

            client.AddQueryParameter("client_id", _clientId);
            client.AddQueryParameter("response_type", "code");
            client.AddQueryParameter("scope", $"{_scopeUrls[_scope]}");
            client.AddQueryParameter("redirect_uri", _redirectUri);
            client.AddQueryParameter("state", state);
            client.AddQueryParameter("nonce", GetRandomString(10));
            client.AddQueryParameter("access_type", "offline");

            var uri = client.BuildAbsoluteUri(_authResource);

            client.ClearQueryParameters();

            return uri;
        }

        private string GetRandomString(int length)
        {
            var sb = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                var randChar = (char)_rand.Next(0x41, 0x5A);
                sb.Append(randChar);
            }

            return sb.ToString().Replace("=", "-").Replace("&", "-").Replace("?", "-");
        }
    }
}
