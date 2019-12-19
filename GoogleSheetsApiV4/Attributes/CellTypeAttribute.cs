using System;
using GoogleSheetsApiV4.Models.Attributes;

namespace GoogleSheetsApiV4.Attributes
{
    /// <summary>
    /// Describes the cell type for update operations.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class CellTypeAttribute : Attribute
    {
        /// <summary>
        /// The cell type for this property.
        /// </summary>
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
