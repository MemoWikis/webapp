using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrueOrFalse.Infrastructure;

namespace TrueOrFalse.Web
{
    public static class UriSegmentFriendlyQuestion 
    {
        public static string Run(string text)
        {
            return UriSanitizer.Run(text);
        }

    }
}
