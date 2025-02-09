namespace GoogleSheetsApiV4.InternalContract.DataClasses
{
    internal class PostUpdateCellsRequestData : PostRequestData
    {
        public PostUpdateCellsContentRequestData[] Requests { get; set; }
    }
}
