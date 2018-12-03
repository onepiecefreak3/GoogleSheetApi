using GoogleSheetsApiV4.Models;
using GoogleSheetsV4.Models;
using GoogleSheetsV4.Support;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleSheetsV4
{
    /* Explaining authorization methods for Google Apis
     * 
     * ApiKey:
     * The ApiKey will be created and managed in ones google development console, it only needs to be created once and needs to be given some restricitive permissions.
     * The ApiKey will be added as a query parameter to the each request.
     * The ApiKey method can't be used for write operations seemingly
     * 
     * OAuth2:
     * OAuth2 is a protocol needing 3 endpoints to verify and authorize an operation with an OAuth token.
     * One must create a clientId and clientSecret in the google development console and a trusted domain running
     * an own Api of any sort. (JS, C# webapi)
     * ClientId, clientSecret and other required information get send to the google auth endpoint with a specific scope,
     * which will again make a request to the trusted domain with your api on it, which verifies the token request by
     * googles auth endpoint
     * After the endpoints verified the chain of trust, the main application will receive an active OAuth token and maybe a refresh token,
     * for when the OAuth token expired
     * The OAuth token will then be attached to every operation
     */

    public class GoogleSheet
    {
        private enum Mode
        {
            Read,
            Write
        }

        private string _sheetId;

        private string _apiKey;

        private string _clientId;
        private string _clientSecret;

        private string _baseUrl = "https://sheets.googleapis.com";
        private string _resource = "v4/spreadsheets";

        private Client _client;
        private KeyType _type;
        private Scope _scope;

        public GoogleSheet(string sheetId, string apiKey)
        {
            _type = KeyType.ApiKey;
            _scope = Scope.ReadOnly;

            _apiKey = apiKey;

            Initialize(sheetId);
        }

        public GoogleSheet(string sheetId, string clientId, string clientSecret, Scope scope = Scope.ReadOnly)
        {
            _type = KeyType.OAuth2;
            _scope = scope;

            _clientId = clientId;
            _clientSecret = clientSecret;

            Initialize(sheetId);
        }

        #region Public methods
        public SheetResponse GetSheet()
        {
            Validate(Mode.Read);
            SetupClient();

            var response = _client.Get($"{_resource}/{_sheetId}");
            if (response.IsSuccessful)
            {
                ResetClient();

                var result = JObject.Parse(response.Content).ToObject<SheetResponse>();
                return result;
            }
            else
            {
                throw new InvalidDataException(response.ErrorMessage);
            }
        }

        public IEnumerable<string> GetTableNames()
        {
            return GetSheet().Sheets.Select(x => x.Properties.Title);
        }

        public List<List<object>> GetRange(string tableName, string start, string end)
        {
            SetupClient();

            var response = _client.Get($"{_resource}/{_sheetId}/values/{tableName}!{start}:{end}");
            if (response.IsSuccessful)
            {
                ResetClient();

                var result = JObject.Parse(response.Content)["values"].ToObject<List<List<object>>>();
                return result;
            }
            else
            {
                throw new InvalidDataException(response.ErrorMessage);
            }
        }
        #endregion

        #region Private methods
        private void Initialize(string sheetId)
        {
            _sheetId = sheetId;
            _client = new Client(_baseUrl);
        }

        private void Validate(Mode mode)
        {
            switch (mode)
            {
                case Mode.Write:
                    if (_scope == Scope.ReadOnly)
                        throw new InvalidOperationException("Operation needs write permission. Scope is ReadOnly");
                    break;
            }
        }

        private void SetupClient()
        {
            switch (_type)
            {
                case KeyType.ApiKey:
                    _client.AddQueryParameter("key", _apiKey);
                    break;
                case KeyType.OAuth2:
                    break;
            }
        }

        private void ResetClient()
        {
            switch (_type)
            {
                case KeyType.ApiKey:
                    _client.ClearQueryParameters();
                    break;
                case KeyType.OAuth2:
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
