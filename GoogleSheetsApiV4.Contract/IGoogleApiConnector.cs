namespace GoogleSheetsApiV4.Contract
{
    public interface IGoogleApiConnector
    {
        ISheetManager CreateSheetManager(string sheetId, ICodeFlowManager codeFlowManager);
    }
}
