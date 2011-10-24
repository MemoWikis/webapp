using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using TrueOrFalse.Core.Infrastructure;

namespace TrueOrFalse.Core.Web.Uris
{
    public static class UriSegmentFriendlyUser
    {
        public static string Run(string userName)
        {
            return UriSanitizer.Run(userName);
        }
    }
}
