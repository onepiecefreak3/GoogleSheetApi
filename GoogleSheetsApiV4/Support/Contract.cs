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
        /// Ensure that the given value is not <see langword="null"/>.
        /// </summary>
        /// <param name="value">The value to ensure.</param>
        /// <param name="valueName">Name of the value.</param>
        public static void EnsureNotNull(object value, string valueName)
        {
            if (value == null)
                throw new ArgumentNullException(valueName);
        }

        /// <summary>
        /// Ensures that the given value is part of <typeparamref name="TEnum"/>.
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
        /// Ensures that the given value is <see langword="true" />.
        /// </summary>
        /// <param name="value">The value to ensure.</param>
        /// <param name="valueName">Name of the value.</param>
        public static void EnsureTrue(bool value, string valueName)
        {
            if (!value)
                throw new InvalidOperationException($"The value '{valueName}' was false.");
        }

        /// <summary>
        /// Ensures that the range element has the correct pattern.
        /// </summary>
        /// <param name="range">The range element to ensure.</param>
        /// <param name="rangeName">Name of the range element.</param>
        public static void EnsureRangePattern(string range, string rangeName)
        {
            if (!Regex.IsMatch(range, "[A-Z]+\\d+"))
                throw new InvalidOperationException($"The value '{range}' in '{rangeName}' has an invalid pattern.");
        }
    }
}
