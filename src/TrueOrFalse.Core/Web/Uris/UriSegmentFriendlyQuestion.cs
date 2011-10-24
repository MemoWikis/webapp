using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrueOrFalse.Core.Infrastructure;

namespace TrueOrFalse.Core.Web
{
    public static class UriSegmentFriendlyQuestion 
    {
        public static string Run(string text)
        {
            return UriSanitizer.Run(text);
        }

    }
}
