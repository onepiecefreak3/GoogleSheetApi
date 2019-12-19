using System;
using System.Collections.Generic;
using System.Text;

namespace GoogleSheetsApiV4.Models.Attributes
{
    /// <summary>
    /// Describes the type of a cell.
    /// </summary>
    public enum CellType
    {
        /// <summary>
        /// Describes a string value.
        /// </summary>
        String,

        /// <summary>
        /// Describes a numeric value.
        /// </summary>
        Number,

        /// <summary>
        /// Describes a boolean value.
        /// </summary>
        Boolean,

        /// <summary>
        /// Describes a formula.
        /// </summary>
        Formula
    }
}
