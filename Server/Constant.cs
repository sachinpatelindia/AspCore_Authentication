using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server
{
    public static class Constants
    {
        public const string Audience = "https://localhost:44305/";
        public const string Issuer = "https://localhost:44305/";
        public const string Secret = "not_too_short_secret";
    }
}
