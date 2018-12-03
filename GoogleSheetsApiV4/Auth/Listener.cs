using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace GoogleSheetsApiV4.Auth
{
    internal class BasicListener : IDisposable
    {
        private string _consentUrl;
        private HttpListener _listener;

        public BasicListener(string consentUrl)
        {
            _consentUrl = consentUrl;

            _listener = new HttpListener();
            _listener.Prefixes.Add(consentUrl);
        }

        public void Dispose()
        {
            _listener.Stop();
            _listener = null;
        }

        public BasicListener Start()
        {
            _listener.Start();
            return this;
        }

        public HttpListenerRequest AwaitRequest()
        {
            return _listener.GetContext().Request;
        }
    }
}
