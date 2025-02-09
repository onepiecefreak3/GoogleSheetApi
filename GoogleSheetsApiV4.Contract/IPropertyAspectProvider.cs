using System.Reflection;
using GoogleSheetsApiV4.Contract.DataClasses;

namespace GoogleSheetsApiV4.Contract
{
    public interface IPropertyAspectProvider
    {
        int? GetPropertyColumn(PropertyInfo property);
        CellType GetPropertyCellType(PropertyInfo property);
    }
}
