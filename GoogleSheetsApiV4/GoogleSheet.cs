using GoogleSheetsApiV4.Auth;
using GoogleSheetsApiV4.Models;
using GoogleSheetsApiV4.Support;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GoogleSheetsApiV4
{
    /* Explaining authorization methods for Google Apis
     * 
     * ApiKey:
     * The ApiKey will be created and managed in ones google development console, it only needs to be created once and
     *   needs to be given some restrictive permissions.
     * The ApiKey will be added as a query parameter to the each request.
     * The ApiKey method can't be used for write operations.
     * 
     * OAuth2Flow:
     * OAuth2Flow is a protocol needing 3 endpoints to verify and authorize an operation with an OAuth token.
     * One must create a clientId and clientSecret in the google development console and a trusted domain running
     *   an own Api of any sort. (JS, C# web api).
     * ClientId, ClientSecret and other required information get send to the google auth endpoint with a specific scope,
     *   which will again make a request to the trusted domain with your api on it, which verifies the token request by
     *   Google's auth endpoint.
     * After the endpoints verified the chain of trust, the main application will receive an active OAuth token and a refresh token,
     *   for when the OAuth token expired.
     * The OAuth token will then be attached to every request.
     */

    public class GoogleSheet
    {
        private readonly Client _client;

        private string _baseUrl = "https://sheets.googleapis.com";
        private string _resource = "v4/spreadsheets";

        private readonly string _apiKey;
        private readonly OAuth2Flow _oauth2;

        /// <summary>
        /// The sheet to work with.
        /// </summary>
        public string SheetId { get; }

        /// <summary>
        /// The authorization to use.
        /// </summary>
        public KeyType KeyType { get; }

        /// <summary>
        /// The scope to work with on the sheet.
        /// </summary>
        public Scope Scope { get; }

        /// <summary>
        /// Creates a new instance of <see cref="GoogleSheet"/> with ApiKey authorization.
        /// </summary>
        /// <param name="sheetId">The sheet to work with.</param>
        /// <param name="apiKey">The api key to authorize with.</param>
        public GoogleSheet(string sheetId, string apiKey)
        {
            KeyType = KeyType.ApiKey;
            Scope = Scope.ReadOnly;
            SheetId = sheetId;

            _apiKey = apiKey;

            _client = new Client(_baseUrl);
        }

        /// <summary>
        /// Creates a new instance of <see cref="GoogleSheet"/> with ApiKey authorization.
        /// </summary>
        /// <param name="sheetId">The sheet to work with.</param>
        /// <param name="clientId">The client id to authorize with.</param>
        /// <param name="clientSecret">The client secret to authorize with.</param>
        /// <param name="scope">The scope of the sheet.</param>
        public GoogleSheet(string sheetId, string clientId, string clientSecret, Scope scope = Scope.ReadOnly)
        {
            Contract.EnsureNotNull(sheetId, nameof(sheetId));
            Contract.EnsureNotNull(sheetId, nameof(clientId));
            Contract.EnsureNotNull(sheetId, nameof(clientSecret));
            Contract.EnsureMemberOfEnum(scope, nameof(scope));

            KeyType = KeyType.OAuth2;
            Scope = scope;
            SheetId = sheetId;

            _oauth2 = new OAuth2Flow(clientId, clientSecret, scope);

            _client = new Client(_baseUrl);
        }

        #region Public methods

        /// <summary>
        /// Get the content of all tables on the given sheet.
        /// </summary>
        /// <returns>The content of all tables on the given sheet.</returns>
        /// <exception cref="InvalidDataException">If the request was not successful.</exception>
        public SheetResponse GetSheet()
        {
            Contract.EnsureAccessModeAllowed(AccessMode.Read, Scope);
            SetupClient();

            var response = _client.Get($"{_resource}/{SheetId}");
            if (!response.IsSuccessful)
                throw new InvalidDataException(response.ErrorMessage);

            ResetClient();

            return JObject.Parse(response.Content).ToObject<SheetResponse>();
        }

        /// <summary>
        /// Retrieve all table names of the sheet.
        /// </summary>
        /// <returns>The titles of all tables on the given sheet.</returns>
        public IEnumerable<string> GetTableNames()
        {
            return GetSheet().Sheets.Select(s => s.Properties.Title);
        }

        /// <summary>
        /// Get a range of data from a table of the sheet.
        /// </summary>
        /// <typeparam name="TParse">Type to parse the result into.</typeparam>
        /// <param name="tableName">The name of the table to retrieve data from.</param>
        /// <param name="start">The line and column to start from.</param>
        /// <param name="end">The line and column to end at.</param>
        /// <returns>Parsed result of the ranged data.</returns>
        /// <exception cref="InvalidDataException">If the request was not successful.</exception>
        public IEnumerable<TParse> GetRange<TParse>(string tableName, string start, string end)
        {
            Contract.EnsureNotNull(tableName, nameof(tableName));
            Contract.EnsureNotNull(start, nameof(start));
            Contract.EnsureNotNull(end, nameof(end));
            Contract.EnsureRangePattern(start, nameof(start));
            Contract.EnsureRangePattern(end, nameof(end));
            Contract.EnsureAccessModeAllowed(AccessMode.Read, Scope);
            SetupClient();

            var response = _client.Get($"{_resource}/{SheetId}/values/{tableName}!{start}:{end}");
            if (!response.IsSuccessful)
                throw new InvalidDataException(response.ErrorMessage);

            ResetClient();

            var parsedResult = JObject.Parse(response.Content)["values"].ToObject<List<List<object>>>();
            return parsedResult.ParseType<TParse>(start, end);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Setup the client with required authorization.
        /// </summary>
        private void SetupClient()
        {
            switch (KeyType)
            {
                case KeyType.ApiKey:
                    _client.AddQueryParameter("key", _apiKey);
                    break;
                case KeyType.OAuth2:
                    var accessToken = _oauth2.RetrieveAccessToken();
                    _client.AddHeader("Authorization", $"Bearer {accessToken}");
                    break;
            }
        }

        /// <summary>
        /// Reset the authorization in the client.
        /// </summary>
        private void ResetClient()
        {
            switch (KeyType)
            {
                case KeyType.ApiKey:
                    _client.ClearQueryParameters();
                    break;
                case KeyType.OAuth2:
                    _client.ClearHeaders();
                    break;
            }
        }

        #endregion

        //public void UpdateRange(List<List<object>> range, string tableName, string start, string end)
        //{
        //    if (_type == KeyType.ApiKey)
        //        throw new InvalidOperationException("ApiKey can't be used for updating.");

        //    var body = new JObject();
        //    body.Add("values", JArray.FromObject(range));

        //    _client.SetJsonBody(JsonConvert.SerializeObject(body));

        //    var response = _client.Put($"{_resource}/{_sheetId}/values/{tableName}!{start}:{end}");
        //    if (response.IsSuccessful)
        //    {
        //        var result = JObject.Parse(response.Content)["values"].ToObject<List<List<object>>>();
        //    }
        //    else
        //    {
        //        throw new InvalidDataException(response.ErrorMessage);
        //    }
        //}
    }
}
