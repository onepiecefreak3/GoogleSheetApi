using System.Net.Http.Json;
using System.Text.Json;
using GoogleSheetsApiV4.Contract;
using GoogleSheetsApiV4.Contract.DataClasses;
using GoogleSheetsApiV4.InternalContract;
using GoogleSheetsApiV4.InternalContract.DataClasses;
using GoogleSheetsApiV4.InternalContract.DataClasses.Contexts;

namespace GoogleSheetsApiV4
{
    internal class SheetManager : ISheetManager
    {
        private static readonly JsonSerializerOptions PostRequestOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true,
            TypeInfoResolver = PostUpdateCellsRequestDataContext.Default
        };

        private const string BaseUrl_ = "https://sheets.googleapis.com";
        private const string SheetResource_ = "v4/spreadsheets";

        private readonly string _sheetId;
        private readonly ICodeFlowManager _codeFlowManager;
        private readonly IDataRangeParser _dataRangeParser;
        private readonly IRequestContentBuilder _contentBuilder;

        private readonly HttpClient _client;
        private readonly IDictionary<string, int> _sheetIdCache;

        public SheetManager(string sheetId, ICodeFlowManager codeFlowManager, IDataRangeParser dataRangeParser, IRequestContentBuilder contentBuilder)
        {
            _sheetId = sheetId;
            _codeFlowManager = codeFlowManager;
            _dataRangeParser = dataRangeParser;
            _contentBuilder = contentBuilder;

            _sheetIdCache = new Dictionary<string, int>();

            _client = new HttpClient
            {
                BaseAddress = new Uri(BaseUrl_)
            };
        }

        public async Task<SheetData[]?> GetSheetsAsync()
        {
            SheetObjectResponseData? sheetObject = await GetSheetObjectAsync();

            return sheetObject?.Sheets.Select(s => new SheetData
            {
                Id = s.Properties.SheetId,
                Name = s.Properties.Title
            }).ToArray();
        }

        public async Task<int?> GetSheetIdAsync(string sheetTitle)
        {
            if (_sheetIdCache.TryGetValue(sheetTitle, out int sheetId))
                return sheetId;

            SheetObjectResponseData? sheetObject = await GetSheetObjectAsync();

            int? resSheetId = sheetObject?.Sheets.FirstOrDefault(s => s.Properties.Title == sheetTitle)?.Properties.SheetId;
            if (resSheetId.HasValue)
                _sheetIdCache[sheetTitle] = resSheetId.Value;

            return resSheetId;
        }

        public async Task<TParse[]?> GetRangeAsync<TParse>(string sheetTitle, CellIdentifier start, CellIdentifier end)
        {
            HttpRequestMessage sheetRequest = await _codeFlowManager.CreateGetRequestAsync($"{SheetResource_}/{_sheetId}/values/{sheetTitle}!{start}:{end}");
            HttpResponseMessage sheetResponse = await _client.SendAsync(sheetRequest);

            if (!sheetResponse.IsSuccessStatusCode)
                return null;

            var valueRange = await sheetResponse.Content.ReadFromJsonAsync(ValueRangeResponseDataContext.Default.ValueRangeResponseData);
            TParse[] parsedRange = _dataRangeParser.Parse<TParse>(valueRange?.Values, start, end).ToArray();

            return parsedRange;
        }

        public async Task<bool> UpdateRangeAsync<TUpdate>(IList<TUpdate> updateData, string sheetTitle, CellIdentifier start, CellIdentifier end)
        {
            int? sheetId = await GetSheetIdAsync(sheetTitle);
            if (!sheetId.HasValue)
                return false;

            UpdateCellsRequestData updateCellsRequest = _contentBuilder.CreateUpdateCellsRequest(updateData, sheetId.Value, start, end);
            var postRequest = new PostUpdateCellsRequestData
            {
                Requests = new PostUpdateCellsContentRequestData[] { new() { UpdateCells = updateCellsRequest } }
            };

            HttpRequestMessage sheetRequest = await _codeFlowManager.CreatePostRequestAsync($"{SheetResource_}/{_sheetId}:batchUpdate");
            sheetRequest.Content = JsonContent.Create(postRequest, options: PostRequestOptions);

            HttpResponseMessage sheetResponse = await _client.SendAsync(sheetRequest);

            return sheetResponse.IsSuccessStatusCode;
        }

        private async Task<SheetObjectResponseData?> GetSheetObjectAsync()
        {
            HttpRequestMessage sheetRequest = await _codeFlowManager.CreateGetRequestAsync($"{SheetResource_}/{_sheetId}");
            HttpResponseMessage sheetResponse = await _client.SendAsync(sheetRequest);

            if (!sheetResponse.IsSuccessStatusCode)
                return null;

            return await sheetResponse.Content.ReadFromJsonAsync(SheetObjectResponseDataContext.Default.SheetObjectResponseData);
        }
    }
}
