using System.Collections.Generic;
using RestSharp;

namespace GoogleSheetsApiV4.Support
{
    /// <summary>
    /// Client to wrap RestSharp for internal use.
    /// </summary>
    internal class Client
    {
        private readonly string _baseUrl;
        private readonly RestClient _client;

        private readonly List<(string Name, string Value)> _headers = new List<(string Name, string Value)>();
        private readonly List<(string Name, string Value)> _cookies = new List<(string Name, string Value)>();
        private readonly List<(string Name, string Value)> _queryParameters = new List<(string Name, string Value)>();
        private string _jsonBody;

        /// <summary>
        /// Initializes a new instance of <see cref="Client"/>.
        /// </summary>
        /// <param name="baseUrl">The base url to send requests to.</param>
        public Client(string baseUrl)
        {
            Contract.EnsureNotNull(baseUrl, nameof(baseUrl));

            _baseUrl = baseUrl;
            _client = new RestClient(baseUrl);
        }

        /// <summary>
        /// Send a GET request.
        /// </summary>
        /// <param name="resource">The resource to send the request to.</param>
        /// <returns>The response to the given request.</returns>
        public IRestResponse Get(string resource)
        {
            Contract.EnsureNotNull(resource, nameof(resource));

            var request = new RestRequest(resource);
            return Send(request);
        }

        /// <summary>
        /// Send a POST request.
        /// </summary>
        /// <param name="resource">The resource to send the request to.</param>
        /// <returns>The response to the given request.</returns>
        public IRestResponse Post(string resource)
        {
            Contract.EnsureNotNull(resource, nameof(resource));

            var request = new RestRequest(resource, Method.POST);
            return Send(request);
        }

        /// <summary>
        /// Send a PUT request.
        /// </summary>
        /// <param name="resource">The resource to send the request to.</param>
        /// <returns>The response to the given request.</returns>
        public IRestResponse Put(string resource)
        {
            Contract.EnsureNotNull(resource, nameof(resource));

            var request = new RestRequest(resource, Method.PUT);
            return Send(request);
        }

        /// <summary>
        /// Add a parameter to the query.
        /// </summary>
        /// <param name="name">The name of the query parameter.</param>
        /// <param name="value">The value of the query parameter.</param>
        public void AddQueryParameter(string name, string value)
        {
            Contract.EnsureNotNull(name, nameof(name));
            Contract.EnsureNotNull(value, nameof(value));

            _queryParameters.Add((name, value));
        }

        /// <summary>
        /// Add a header to the request.
        /// </summary>
        /// <param name="name">The name of the header.</param>
        /// <param name="value">The value of the header.</param>
        public void AddHeader(string name, string value)
        {
            Contract.EnsureNotNull(name, nameof(name));
            Contract.EnsureNotNull(value, nameof(value));

            _headers.Add((name, value));
        }

        /// <summary>
        /// Add a cookie to the request.
        /// </summary>
        /// <param name="name">The name of the cookie.</param>
        /// <param name="value">The value of the cookie.</param>
        public void AddCookie(string name, string value)
        {
            Contract.EnsureNotNull(name, nameof(name));
            Contract.EnsureNotNull(value, nameof(value));

            _cookies.Add((name, value));
        }

        /// <summary>
        /// Sets the JSON body of the request.
        /// </summary>
        /// <param name="body">The body to set.</param>
        public void SetJsonBody(string body)
        {
            Contract.EnsureNotNull(body, nameof(body));

            _jsonBody = body;
        }

        /// <summary>
        /// Clear all query parameters.
        /// </summary>
        public void ClearQueryParameters()
        {
            _queryParameters.Clear();
        }

        /// <summary>
        /// Clear all headers.
        /// </summary>
        public void ClearHeaders()
        {
            _headers.Clear();
        }

        #region Private methods

        /// <summary>
        /// Sends a compiled <see cref="RestRequest"/>.
        /// </summary>
        /// <param name="request">The request to send.</param>
        /// <returns>The response to the given request.</returns>
        private IRestResponse Send(RestRequest request)
        {
            foreach (var header in _headers)
                request.AddHeader(header.Name, header.Value);

            foreach (var cookie in _cookies)
                request.AddCookie(cookie.Name, cookie.Value);

            foreach (var param in _queryParameters)
                request.AddQueryParameter(param.Name, param.Value);

            if (!string.IsNullOrEmpty(_jsonBody))
                request.AddJsonBody(_jsonBody);

            return _client.Execute(request);
        }

        #endregion

        public string BuildAbsoluteUri(string resource)
        {
            string path;

            if (string.IsNullOrEmpty(resource))
                path = _baseUrl;
            else if (_baseUrl.EndsWith("/") && resource.StartsWith("/"))
                path = _baseUrl + resource.Remove(0, 1);
            else if (!_baseUrl.EndsWith("/") && !resource.StartsWith("/"))
                path = _baseUrl + "/" + resource;
            else
                path = _baseUrl + resource;

            path += "?";
            for (int i = 0; i < _queryParameters.Count; i++)
            {
                path += _queryParameters[i].Name + "=" + _queryParameters[i].Value;
                if (i + 1 != _queryParameters.Count)
                    path += "&";
            }

            return path;
        }
    }
}
