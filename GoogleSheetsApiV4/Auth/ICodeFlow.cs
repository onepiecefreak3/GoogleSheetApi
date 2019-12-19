using GoogleSheetsApiV4.Models;
using RestSharp;

namespace GoogleSheetsApiV4.Auth
{
    /// <summary>
    /// Describes an authentication flow for authenticated requests.
    /// </summary>
    public interface ICodeFlow
    {
        /// <summary>
        /// Gets the scope of this authorization flow.
        /// </summary>
        Scope Scope { get; }

        /// <summary>
        /// Are requests allowed to read.
        /// </summary>
        bool CanRead { get; }

        /// <summary>
        /// Are requests allowed to write.
        /// </summary>
        bool CanWrite { get; }

        /// <summary>
        /// Retrieves an authenticated request to retrieve Sheet information.
        /// </summary>
        /// <param name="relativeUri">The relative Uri to the target sheet.</param>
        /// <returns>An authenticated request.</returns>
        IRestRequest CreateRequest(string relativeUri);
    }
}
