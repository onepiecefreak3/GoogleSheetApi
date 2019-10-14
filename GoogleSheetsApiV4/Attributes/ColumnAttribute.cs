using System;
using GoogleSheetsApiV4.Support;

namespace GoogleSheetsApiV4.Attributes
{
    /// <summary>
    /// Marks a property as a parseable column in the model.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnAttribute : Attribute
    {
        /// <summary>
        /// The letter designation of the column.
        /// </summary>
        public string ColumnLetter { get; }

        /// <summary>
        /// Creates a new instance of <see cref="ColumnAttribute"/>.
        /// </summary>
        /// <param name="columnLetter">The letter designation of the column.</param>
        public ColumnAttribute(string columnLetter)
        {
            Contract.EnsureNotNull(columnLetter, nameof(columnLetter));

            ColumnLetter = columnLetter;
        }
    }
}
