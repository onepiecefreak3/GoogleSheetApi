using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleSheetsApiV4.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnAttribute : Attribute
    {
        private string _columnLetter { get; set; }

        public ColumnAttribute(string columnLetter)
        {
            _columnLetter = columnLetter;
        }
    }
}
