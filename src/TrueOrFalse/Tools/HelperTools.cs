using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Globalization;

namespace TrueOrFalse.Tools
{
    public class HelperTools
    {
        public static bool IsLocal()
        {
            var request = HttpContext.Current.Request;

            if (!request.IsLocal)
                return false;
            return true;
        }
    }
}
