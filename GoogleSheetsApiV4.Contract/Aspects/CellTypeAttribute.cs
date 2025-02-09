using GoogleSheetsApiV4.Contract.DataClasses;

namespace GoogleSheetsApiV4.Contract.Aspects
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CellTypeAttribute:Attribute
    {
        public CellType CellType { get; }

        /// <summary>
        /// Creates a new instance of <see cref="CellTypeAttribute"/>.
        /// </summary>
        /// <param name="cellType">The type of the cell.</param>
        public CellTypeAttribute(CellType cellType)
        {
            CellType = cellType;
        }
    }
}
