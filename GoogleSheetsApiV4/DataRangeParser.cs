using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using GoogleSheetsApiV4.Contract;
using GoogleSheetsApiV4.Contract.DataClasses;
using GoogleSheetsApiV4.InternalContract;

namespace GoogleSheetsApiV4
{
    internal class DataRangeParser : IDataRangeParser
    {
        private readonly IPropertyAspectProvider _aspectProvider;

        public DataRangeParser(IPropertyAspectProvider aspectProvider)
        {
            _aspectProvider = aspectProvider;
        }

        public IReadOnlyCollection<TType> Parse<TType>(List<List<string>?>? list, CellIdentifier start, CellIdentifier end)
        {
            if (list == null)
                return Array.Empty<TType>();

            var columns = new Dictionary<int, PropertyInfo>();
            foreach (PropertyInfo property in typeof(TType).GetProperties())
            {
                int? column = _aspectProvider.GetPropertyColumn(property);
                if (column.HasValue)
                    columns[column.Value] = property;
            }

            var result = new List<TType>();

            Type type = typeof(TType);
            foreach (List<string>? entry in list)
            {
                if (entry == null)
                    continue;

                var model = (TType)Activator.CreateInstance(type)!;
                for (var i = 0; i < entry.Count; i++)
                {
                    int column = start.Column.Value + i;
                    if (!columns.TryGetValue(column, out PropertyInfo? chosenProperty))
                        continue;

                    chosenProperty.SetValue(model, Convert.ChangeType(entry[i], chosenProperty.PropertyType));
                }

                result.Add(model);
            }

            return result;
        }
    }
}
