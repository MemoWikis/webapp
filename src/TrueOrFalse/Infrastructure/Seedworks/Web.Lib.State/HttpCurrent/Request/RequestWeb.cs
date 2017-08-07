using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;

namespace Seedworks.Web.State
{
    public class RequestWeb : IRequest
    {
        private readonly HttpRequest _request = HttpContext.Current.Request;

        public Uri Url { get { return _request.Url; } }
        public string[] UserLanguages { get { return _request.UserLanguages; } }
        public HttpCookieCollection Cookies { get { return _request.Cookies; } }
        public NameValueCollection QueryString { get { return _request.QueryString;  } }
		public string Path { get { return _request.Path; } }
		public string FilePath { get { return _request.FilePath; } }
    }
}
