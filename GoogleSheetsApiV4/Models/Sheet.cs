using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleSheetsApiV4.Models
{
    public class Sheet
    {
        public SheetProperties Properties { get; set; }
    }

    public class SheetProperties
    {
        public int SheetId { get; set; }
        public string Title { get; set; }
        public int Index { get; set; }
        public string SheetType { get; set; }
        public GridProperties GridProperties { get; set; }
        public General.Color TabColor { get; set; }
    }

    public class GridProperties
    {
        public int RowCount { get; set; }
        public int ColumnCount { get; set; }
    }
}
