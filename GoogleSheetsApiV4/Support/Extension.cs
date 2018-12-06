using GoogleSheetsApiV4.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GoogleSheetsApiV4.Support
{
    internal static class Extension
    {
        public static IEnumerable<T> ParseType<T>(this List<List<object>> list, string start, string end)
        {
            var propColumnDictionary = new Dictionary<string, string>();
            foreach (var prop in typeof(T).GetProperties())
            {
                var attr = prop.CustomAttributes.Where(x => x.AttributeType == typeof(ColumnAttribute));
                if (attr.Any())
                {
                    propColumnDictionary.Add(attr.First().ConstructorArguments.First().Value.ToString(), prop.Name);
                }
            }

            var startColumn = Regex.Match(start, "[A-Z]+").Value;
            var endColumn = Regex.Match(end, "[A-Z]+").Value;
            var range = CreateLetterRange(startColumn, endColumn).ToArray();

            foreach (var entry in list)
            {
                var model = (T)Activator.CreateInstance(typeof(T));

                for (int i = 0; i < entry.Count; i++)
                {
                    if (propColumnDictionary.ContainsKey(range[i]))
                    {
                        var chosenProp = model.GetType().GetProperty(propColumnDictionary[range[i]]);
                        chosenProp.SetValue(model, Convert.ChangeType(entry[i], chosenProp.PropertyType));
                    }
                }

                yield return model;
            }
        }

        public static IEnumerable<string> CreateLetterRange(string start, string end)
        {
            string currentString = start;
            while (currentString != end)
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

                char newC = (char)(c + 1);
                if (newC >= 0x5B)
                {
                    result += "A";
                    carry = true;
                }
                else
                {
                    result += newC;
                    carry = false;
                }
            }
            if (carry)
                result += "A";

            return result.Reverse().Aggregate("", (a, b) => a + b);
        }
    }
}
