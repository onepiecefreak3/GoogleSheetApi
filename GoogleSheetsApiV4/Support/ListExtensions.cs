using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using GoogleSheetsApiV4.Attributes;
using GoogleSheetsApiV4.Models.Attributes;
using GoogleSheetsApiV4.Models.Update;

namespace GoogleSheetsApiV4.Support
{
    static class ListExtensions
    {
        public static IEnumerable<TParse> ParseType<TParse>(this List<List<object>> list, string start, string end)
        {
            Contract.EnsureNotNull(list, nameof(list));
            Contract.EnsureNotNull(start, nameof(start));
            Contract.EnsureNotNull(start, nameof(start));
            Contract.EnsureRangePattern(start, nameof(start));
            Contract.EnsureRangePattern(end, nameof(end));

            var columns = GetValidColumnProperties<TParse>();

            var startColumn = Regex.Match(start, "[A-Z]+").Value;
            var endColumn = Regex.Match(end, "[A-Z]+").Value;
            var range = CreateLetterRange(startColumn, endColumn).ToArray();

            var type = typeof(TParse);
            foreach (var entry in list)
            {
                var model = (TParse)Activator.CreateInstance(type);

                if (entry == null)
                    continue;

                for (var i = 0; i < entry.Count; i++)
                {
                    if (columns.ContainsKey(range[i]))
                    {
                        var chosenProp = type.GetProperty(columns[range[i]]);
                        chosenProp?.SetValue(model, Convert.ChangeType(entry[i], chosenProp.PropertyType));
                    }
                }

                yield return model;
            }
        }

        public static UpdateCellsRequest CreateUpdateCellsRequest<TUpdate>(this IList<TUpdate> list, int sheetId, string start, string end)
        {
            Contract.EnsureNotNull(list, nameof(list));
            Contract.EnsureNotNull(start, nameof(start));
            Contract.EnsureNotNull(start, nameof(start));
            Contract.EnsureRangePattern(start, nameof(start));
            Contract.EnsureRangePattern(end, nameof(end));

            var updateCellsRequest = new UpdateCellsRequest
            {
                Range = new GridRange
                {
                    SheetId = sheetId,
                    StartColumnIndex = GetColumnIndex(start),  // inclusive index
                    EndColumnIndex = GetColumnIndex(end) + 1,  // exclusive index
                    StartRowIndex = int.Parse(Regex.Match(start, "\\d+").Value) - 1,  // zero-based, inclusive index
                    EndRowIndex = int.Parse(Regex.Match(end, "\\d+").Value)   // zero-based, exclusive index
                },
                Rows = new RowData[list.Count]
            };

            var startColumn = Regex.Match(start, "[A-Z]+").Value;
            var endColumn = Regex.Match(end, "[A-Z]+").Value;
            var range = CreateLetterRange(startColumn, endColumn).ToArray();

            var validColumns = GetValidColumnProperties<TUpdate>();

            var type = typeof(TUpdate);
            for (var rowIndex = 0; rowIndex < list.Count; rowIndex++)
            {
                updateCellsRequest.Rows[rowIndex] = new RowData
                {
                    Values = new CellData[range.Length]
                };

                for (var columnIndex = 0; columnIndex < range.Length; columnIndex++)
                {
                    var extendedValue = new ExtendedValue();
                    updateCellsRequest.Rows[rowIndex].Values[columnIndex] = new CellData
                    {
                        UserEnteredValue = extendedValue
                    };

                    if (validColumns.ContainsKey(range[columnIndex]))
                    {
                        var property = type.GetProperty(validColumns[range[columnIndex]]);

                        var cellType = GetCellType(property);
                        var propertyValue = property?.GetValue(list[rowIndex]);
                        SetCellValue(extendedValue, propertyValue, cellType);
                    }
                }
            }

            return updateCellsRequest;
        }

        #region Private methods

        private static IDictionary<string, string> GetValidColumnProperties<TParse>()
        {
            var result = new Dictionary<string, string>();

            foreach (var prop in typeof(TParse).GetProperties())
            {
                var attribute = prop.GetCustomAttribute<ColumnAttribute>();
                if (attribute != null)
                    result[attribute.ColumnLetter] = prop.Name;
            }

            return result;
        }

        private static IEnumerable<string> CreateLetterRange(string startLetter, string endLetter)
        {
            var currentString = startLetter;

            while (currentString != endLetter)
            {
                yield return currentString;
                currentString = AddOne(currentString);
            }

            yield return currentString;
        }

        private static string AddOne(string input)
        {
            var result = "";
            var carry = true;

            foreach (var c in input.Reverse())
            {
                if (!carry)
                {
                    result += c;
                    continue;
                }

                var newC = (char)(c + 1);
                if (newC >= 0x5B)
                {
                    result += "A";
                }
                else
                {
                    result += newC;
                    carry = false;
                }
            }

            if (carry)
                result += "A";

            return new string(result.Reverse().ToArray());
        }

        private static int GetColumnIndex(string input)
        {
            var letters = Regex.Match(input, "[A-Z]+").Value;

            var index = 0;
            for (var i = 0; i < letters.Length; i++)
            {
                index += (letters[i] - '@') * (int)Math.Pow(26, letters.Length - 1 - i);
            }

            return index - 1;
        }

        private static CellType GetCellType(PropertyInfo property)
        {
            var cellTypeAttribute = property.GetCustomAttribute<CellTypeAttribute>();
            if (cellTypeAttribute != null)
                return cellTypeAttribute.CellType;

            switch (Type.GetTypeCode(property.PropertyType))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                    return CellType.Number;

                case TypeCode.Boolean:
                    return CellType.Boolean;

                default:
                    return CellType.String;
            }
        }

        private static void SetCellValue(ExtendedValue extendedValue, object value, CellType cellType)
        {
            switch (cellType)
            {
                case CellType.String:
                    extendedValue.StringValue = (string)Convert.ChangeType(value, typeof(string));
                    break;

                case CellType.Number:
                    extendedValue.NumberValue = (long)Convert.ChangeType(value, typeof(long));
                    break;

                case CellType.Boolean:
                    extendedValue.BoolValue = (bool)Convert.ChangeType(value, typeof(bool));
                    break;

                case CellType.Formula:
                    extendedValue.FormulaValue = (string)Convert.ChangeType(value, typeof(string));
                    break;
            }
        }

        #endregion
    }
}
