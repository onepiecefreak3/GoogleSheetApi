using GoogleSheetsApiV4;
using GoogleSheetsV4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication
{
    class Program
    {
        static Dictionary<string, string> _sheetIds = new Dictionary<string, string>
        {
            ["Time Travelers"] = "1RAQBCtGgDYjLEuCqt9oE5nuK297tPegk6LbS1sqY1ek",
            ["Test"] = "1sSYYvuxuX5r4URl-0F7h8p45iepdRMXGCDUtb3vVxwU"
        };

        static void Main(string[] args)
        {
            string sheetId = null;
            while (true)
            {
                ListSheetIds();

                var name = Console.ReadLine();
                if (_sheetIds.ContainsKey(name))
                {
                    sheetId = _sheetIds[name];
                    break;
                }

                Console.WriteLine($"No sheetId found with name \"{name}\"");
            }

            var sheet = new GoogleSheet(sheetId, _clientId, _clientSecret);
            //var sheet = new GoogleSheet(sheetId, _apiKey);
            var chapterEntries = GetTimeTravelersTips(sheet, 1).ToList();
        }

        private static void ListSheetIds()
        {
            foreach (var sheetId in _sheetIds)
                Console.WriteLine(sheetId.Key);
        }

        private static IEnumerable<Entry> GetTimeTravelersTips(GoogleSheet sheet, int chapter)
        {
            var chapters = sheet.GetRange<Tips>("TIPS", "B2", "K447");
            return null;
            //var titles = sheet.GetRange("TIPS", "E2", "G447");
            //var finalTexts = sheet.GetRange("TIPS", "K2", "K447");

            //for (int i = 0; i < 445; i++)
            //{
            //    if (int.Parse(chapters[i][0].ToString()) == chapter)
            //    {
            //        if (finalTexts[i].Count <= 0)
            //            Console.WriteLine(i);
            //        else
            //        {
            //            yield return new Entry
            //            {
            //                OriginalTitle = titles[i][0].ToString(),
            //                OriginalTitleEscaped = titles[i][1].ToString(),
            //                TranslatedTitle = titles[i][2].ToString(),
            //                FinalText = finalTexts[i][0].ToString()
            //            };
            //        }
            //    }
            //}
        }

        private class Entry
        {
            public string OriginalTitle { get; set; }
            public string OriginalTitleEscaped { get; set; }
            public string TranslatedTitle { get; set; }
            public string FinalText { get; set; }
        }
    }
}
