namespace GoogleSheetsApiV4.InternalContract.DataClasses
{
    internal abstract class PostRequestData
    {
        public bool IncludeSpreadsheetInResponse { get; set; }
        public string[] ResponseRanges { get; set; }
        public bool ResponseIncludeGridData { get; set; }
    }
}
