using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using TrueOrFalse.Infrastructure;

namespace TrueOrFalse.Web.Uris
{
    public static class UriSegmentFriendlyUser
    {
        public static string Run(string userName)
        {
            return UriSanitizer.Run(userName);
        }
    }
}
