using GoogleSheetsApiV4.Contract.DataClasses;
using GoogleSheetsApiV4.InternalContract.DataClasses;

namespace GoogleSheetsApiV4.InternalContract
{
    internal interface IRequestContentBuilder
    {
        UpdateCellsRequestData CreateUpdateCellsRequest<TUpdate>(IList<TUpdate> data, int sheetId, CellIdentifier start, CellIdentifier end);
    }
}
