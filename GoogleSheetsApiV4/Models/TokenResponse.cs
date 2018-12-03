using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleSheetsApiV4.Models
{
    internal class TokenResponse
    {
        public string Access_token { get; set; }
        public int Expires_in { get; set; }
        public string Refresh_token { get; set; }
        public string Scope { get; set; }
        public string Token_type { get; set; }
        public string Id_token { get; set; }
    }
}
