using System;
using System.Collections.Generic;
using System.Text;

namespace GoogleSheetsApiV4.Models.Update
{
    /// <summary>
    /// Describes the current error in a cell.
    /// </summary>
    enum ErrorType
    {
        /// <summary>
        /// DEfault error value.
        /// </summary>
        /// <remarks>Do not use.</remarks>
        ERROR_TYPE_UNSPECIFIED,

        /// <summary>
        /// Corresponds to #ERROR.
        /// </summary>
        ERROR,

        /// <summary>
        /// Corresponds to #NULL.
        /// </summary>
        NULL_VALUE,

        /// <summary>
        /// Corresponds to #DIV/0.
        /// </summary>
        DIVIDE_BY_ZERO,

        /// <summary>
        /// Corresponds to #VALUE.
        /// </summary>
        VALUE,

        /// <summary>
        /// Corresponds to #REF.
        /// </summary>
        REF,

        /// <summary>
        /// Corresponds to #NAME?.
        /// </summary>
        NAME,

        /// <summary>
        /// Corresponds to #NUM.
        /// </summary>
        NUM,

        /// <summary>
        /// Corresponds to #N/A.
        /// </summary>
        N_A,

        /// <summary>
        /// Describes a loading formula.
        /// </summary>
        LOADING
    }
}
