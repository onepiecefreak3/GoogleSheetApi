using System.Collections.Generic;
using System.Linq;
using GoogleSheetsApiV4;
using GoogleSheetsV4.Models;

namespace GoogleSheetsV4
{
    class Program
    {
        static readonly OptionsParser Parser = new OptionsParser();

        static void Main(string[] args)
        {
            var options = Parser.Parse(args);
            var sheet = new GoogleSheet(options.SheetId, options.ClientId, options.ClientSecret);

            // Custom code per game
            var chapterEntries = GetTimeTravelersTips(sheet, 1).ToList();
        }

        private static IEnumerable<Tips> GetTimeTravelersTips(GoogleSheet sheet, int chapter)
        {
            var chapters = sheet.GetRange<Tips>("TIPS", "B2", "K447");
            return chapters;
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
    }
}
