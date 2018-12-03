using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleSheetsV4.Support
{
    internal class Client
    {
        private RestClient _client;

        private List<(string Name, string Value)> _headers;
        private List<(string Name, string Value)> _cookies;
        private List<(string Name, string Value)> _queryParams;
        private string _jsonBody;

        public Client(string baseUrl)
        {
            _client = new RestClient(baseUrl);

            _headers = new List<(string, string)>();
            _cookies = new List<(string, string)>();
            _queryParams = new List<(string, string)>();
        }

        public IRestResponse Get(string resource)
        {
            var request = new RestRequest(resource, Method.GET);
            return Send(request);
        }

        public IRestResponse Post(string resource)
        {
            var request = new RestRequest(resource, Method.GET);
            return Send(request);
        }

        public IRestResponse Put(string resource)
        {
            var request = new RestRequest(resource, Method.PUT);
            return Send(request);
        }

        private IRestResponse Send(RestRequest request)
        {
            foreach (var header in _headers)
                request.AddHeader(header.Name, header.Value);

            foreach (var cookie in _cookies)
                request.AddCookie(cookie.Name, cookie.Value);

            foreach (var param in _queryParams)
                request.AddQueryParameter(param.Name, param.Value);

            request.AddJsonBody(_jsonBody);

            return _client.Execute(request);
        }

        public void AddQueryParameter(string name, string value)
        {
            _queryParams.Add((name, value));
        }

        public void SetJsonBody(string body)
        {
            _jsonBody = body;
        }

        public void AddHeader(string name, string value)
        {
            _headers.Add((name, value));
        }

        public void AddCookie(string name, string value)
        {
            _cookies.Add((name, value));
        }

        public void ClearQueryParameters()
        {
            _queryParams.Clear();
        }
    }
}
