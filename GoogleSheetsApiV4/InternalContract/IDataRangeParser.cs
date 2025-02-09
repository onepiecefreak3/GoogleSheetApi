using GoogleSheetsApiV4.Contract.DataClasses;

namespace GoogleSheetsApiV4.InternalContract
{
    public interface IDataRangeParser
    {
        IReadOnlyCollection<TType> Parse<TType>(List<List<string>?>? list, CellIdentifier start, CellIdentifier end);
    }
}
