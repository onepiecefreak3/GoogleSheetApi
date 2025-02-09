using GoogleSheetsApiV4.Contract.DataClasses;

namespace GoogleSheetsApiV4.Contract.Aspects
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnAttribute : Attribute
    {
        public ColumnIdentifier Column { get; }

        public ColumnAttribute(string column)
        {
            Column = ColumnIdentifier.Parse(column);
        }
    }
}
