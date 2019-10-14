using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using GoogleSheetsApiV4.Attributes;

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

        #endregion
    }
}
