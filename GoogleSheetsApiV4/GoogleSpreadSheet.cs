using GoogleSheetsApiV4.Auth;
using GoogleSheetsApiV4.Models;
using GoogleSheetsApiV4.Support;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GoogleSheetsApiV4.Models.Update;
using RestSharp;
using RestSharp.Serializers.Newtonsoft.Json;

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
     * OAuth2CodeFlow:
     * OAuth2CodeFlow is a protocol needing 3 endpoints to verify and authorize an operation with an OAuth token.
     * One must create a clientId and clientSecret in the google development console and a trusted domain running
     *   an own Api of any sort. (JS, C# web api).
     * ClientId, ClientSecret and other required information get send to the google auth endpoint with a specific scope,
     *   which will again make a request to the trusted domain with your api on it, which verifies the token request by
     *   Google's auth endpoint.
     * After the endpoints verified the chain of trust, the main application will receive an active OAuth token and a refresh token,
     *   for when the OAuth token expired.
     * The OAuth token will then be attached to every request.
     */

    public class GoogleSpreadSheet
    {
        private readonly ICodeFlow _codeFlow;
        private readonly IRestClient _client;

        private string _baseUrl = "https://sheets.googleapis.com";
        private string _resource = "v4/spreadsheets";

        /// <summary>
        /// The sheet to work with.
        /// </summary>
        public string SpreadSheetId { get; }

        /// <summary>
        /// The scope to work with on the sheet.
        /// </summary>
        public Scope Scope => _codeFlow.Scope;

        /// <summary>
        /// Creates a new instance of <see cref="GoogleSpreadSheet"/> with ApiKey authorization.
        /// </summary>
        /// <param name="spreadSheetId">The sheet to work with.</param>
        /// <param name="apiKey">The api key to authorize with.</param>
        public GoogleSpreadSheet(string spreadSheetId, string apiKey) :
            this(spreadSheetId, new ApiKeyCodeFlow(apiKey, Scope.ReadOnly))
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="GoogleSpreadSheet"/> with ApiKey authorization.
        /// </summary>
        /// <param name="spreadSheetId">The sheet to work with.</param>
        /// <param name="clientId">The client id to authorize with.</param>
        /// <param name="clientSecret">The client secret to authorize with.</param>
        /// <param name="scope">The scope of the sheet.</param>
        public GoogleSpreadSheet(string spreadSheetId, string clientId, string clientSecret, Scope scope = Scope.ReadOnly) :
            this(spreadSheetId, new OAuth2CodeFlow(clientId, clientSecret, scope))
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="GoogleSpreadSheet"/> with ApiKey authorization.
        /// </summary>
        /// <param name="spreadSheetId">The sheet to work with.</param>
        /// <param name="clientId">The client id to authorize with.</param>
        /// <param name="clientSecret">The client secret to authorize with.</param>
        /// <param name="refreshToken">The RefreshToken for this spreadsheet.</param>
        /// <param name="scope">The scope of the sheet.</param>
        public GoogleSpreadSheet(string spreadSheetId, string clientId, string clientSecret, string refreshToken, Scope scope = Scope.ReadOnly) :
            this(spreadSheetId, new OAuth2CodeFlow(clientId, clientSecret, scope, refreshToken))
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="GoogleSpreadSheet"/> with a custom code flow.
        /// </summary>
        /// <param name="spreadSheetId">The sheet to work with.</param>
        /// <param name="codeFlow">The <see cref="ICodeFlow"/> to authorize.</param>
        public GoogleSpreadSheet(string spreadSheetId, ICodeFlow codeFlow)
        {
            Contract.EnsureNotNull(spreadSheetId, nameof(spreadSheetId));

            SpreadSheetId = spreadSheetId;

            _codeFlow = codeFlow;
            _client = new RestClient(_baseUrl);
        }

        #region Public methods

        /// <summary>
        /// Get the content of all sheets on the given spreadsheet.
        /// </summary>
        /// <returns>The content of all sheets on the given spreadsheet.</returns>
        /// <exception cref="InvalidDataException">If the request was not successful.</exception>
        public SheetResponse GetSheet()
        {
            Contract.EnsureTrue(_codeFlow.CanRead, nameof(_codeFlow.CanRead));

            var sheetRequest = _codeFlow.CreateRequest($"{_resource}/{SpreadSheetId}");

            var response = _client.Get<SheetResponse>(sheetRequest);
            if (!response.IsSuccessful)
                throw new InvalidDataException(response.Content);

            return response.Data;
        }

        /// <summary>
        /// Retrieve all sheet names of the spreadsheet.
        /// </summary>
        /// <returns>The titles of all sheets on the given spreadsheet.</returns>
        public IEnumerable<string> GetSheetNames()
        {
            var sheets = GetSheet().Sheets;
            return sheets.Select(s => s.Properties.Title);
        }

        /// <summary>
        /// Retrieve the id of the sheet with the given title.
        /// </summary>
        /// <param name="sheetTitle">The title of the sheet.</param>
        /// <returns>The id of the sheet.</returns>
        public int GetSheetId(string sheetTitle)
        {
            var sheets = GetSheet().Sheets;
            return sheets.FirstOrDefault(s => s.Properties.Title == sheetTitle)?.Properties.SheetId ?? -1;
        }

        /// <summary>
        /// Get a range of data from a sheet of the spreadsheet.
        /// </summary>
        /// <typeparam name="TParse">Type to parse the result into.</typeparam>
        /// <param name="sheetTitle">The name of the table to retrieve data from.</param>
        /// <param name="start">The line and column to start from.</param>
        /// <param name="end">The line and column to end at.</param>
        /// <returns>Parsed result of the ranged data.</returns>
        /// <exception cref="InvalidDataException">If the request was not successful.</exception>
        public async Task<IEnumerable<TParse>> GetRange<TParse>(string sheetTitle, string start, string end)
        {
            Contract.EnsureNotNull(sheetTitle, nameof(sheetTitle));
            Contract.EnsureNotNull(start, nameof(start));
            Contract.EnsureNotNull(end, nameof(end));
            Contract.EnsureRangePattern(start, nameof(start));
            Contract.EnsureRangePattern(end, nameof(end));
            Contract.EnsureTrue(_codeFlow.CanRead, nameof(_codeFlow.CanRead));

            var sheetRequest = _codeFlow.CreateRequest($"{_resource}/{SpreadSheetId}/values/{sheetTitle}!{start}:{end}");

            var valueRange = await _client.GetAsync<ValueRange>(sheetRequest);
            return valueRange.Values.ParseType<TParse>(start, end);
        }

        public async void UpdateRange<TUpdate>(IList<TUpdate> update, string sheetTitle, string start, string end)
        {
            Contract.EnsureNotNull(update, nameof(update));
            Contract.EnsureNotNull(start, nameof(start));
            Contract.EnsureNotNull(end, nameof(end));
            Contract.EnsureRangePattern(start, nameof(start));
            Contract.EnsureRangePattern(end, nameof(end));
            Contract.EnsureTrue(_codeFlow.CanWrite, nameof(_codeFlow.CanWrite));

            var sheetRequest = _codeFlow.CreateRequest($"{_resource}/{SpreadSheetId}:batchUpdate");
            sheetRequest.JsonSerializer = new NewtonsoftJsonSerializer();

            var updateCellRequest = update.CreateUpdateCellsRequest(GetSheetId(sheetTitle), start, end);
            var requestBody = new Models.Update.RequestBody
            {
                Requests = new object[] { new { updateCells = updateCellRequest } }
            };
            sheetRequest.AddJsonBody(requestBody);

            await _client.PostAsync<object>(sheetRequest);
        }

        #endregion
    }
}
