using GoogleSheetsApiV4.Contract;

namespace GoogleSheetsApiV4
{
    public class GoogleApiConnector : IGoogleApiConnector
    {
        public static readonly GoogleApiConnector Instance = new();

        public ISheetManager CreateSheetManager(string sheetId, ICodeFlowManager codeFlowProvider)
        {
            var dataRangeProvider = new DataRangeParser(PropertyAspectProvider.Instance);
            var requestBuilder = new RequestContentBuilder(PropertyAspectProvider.Instance);

            return new SheetManager(sheetId, codeFlowProvider, dataRangeProvider, requestBuilder);
        }
    }
}
