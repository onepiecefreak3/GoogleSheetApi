using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleSheetsApiV4.Models
{
    public enum KeyType
    {
        ApiKey,
        OAuth2
    }

    public enum Scope
    {
        ReadOnly,
        ReadWrite
    }
}
