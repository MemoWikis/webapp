using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace Seedworks.Web.State
{
    public class Response
    {
        protected readonly List<string> _redirections = new List<string>();
        protected readonly List<HttpCookie> _cookiesCreated = new List<HttpCookie>();

        public List<string> Redirections { get { return _redirections; } }
        public List<HttpCookie> CookiesCreated { get { return _cookiesCreated; } }
    }
}
