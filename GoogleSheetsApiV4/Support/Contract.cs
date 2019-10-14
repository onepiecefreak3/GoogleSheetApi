using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using GoogleSheetsApiV4.Models;

namespace GoogleSheetsApiV4.Support
{
    /// <summary>
    /// General functions to ensure behaviors and contents.
    /// </summary>
    static class Contract
    {
        /// <summary>
        /// Ensure that the value is not null.
        /// </summary>
        /// <param name="value">The value to ensure.</param>
        /// <param name="valueName">Name of the value.</param>
        public static void EnsureNotNull(object value, string valueName)
        {
            if (value == null)
                throw new ArgumentNullException(valueName);
        }

        /// <summary>
        /// Ensures that given value is part of <typeparamref name="TEnum"/>.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <param name="value">The value to ensure.</param>
        /// <param name="valueName">Name of the value.</param>
        public static void EnsureMemberOfEnum<TEnum>(TEnum value, string valueName) where TEnum : Enum
        {
            var type = typeof(TEnum);
            if (!Enum.IsDefined(type, value))
                throw new InvalidEnumArgumentException(valueName, Convert.ToInt32(value), type);
        }

        /// <summary>
        /// Ensure that the access mode is allowed in the scope.
        /// </summary>
        /// <param name="accessMode">The access mode to ensure.</param>
        /// <param name="scope">The scope to ensure the access mode in.</param>
        public static void EnsureAccessModeAllowed(AccessMode accessMode, Scope scope)
        {
            if (accessMode == AccessMode.Write && scope == Scope.ReadOnly)
                throw new InvalidOperationException("Operation needs write permission, but the scope is read-only.");
        }

        /// <summary>
        /// Ensures that the range element has the correct pattern.
        /// </summary>
        /// <param name="range">The range element to ensure.</param>
        /// <param name="rangeName">Name of the range element.</param>
        public static void EnsureRangePattern(string range, string rangeName)
        {
            var regexPattern = "[A-Z]+\\d+";
            if (!Regex.IsMatch(range, regexPattern))
                throw new InvalidOperationException($"The value '{range}' in '{rangeName}' has an invalid pattern.");
        }
    }
}
