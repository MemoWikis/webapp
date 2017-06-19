using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Seedworks.Web.State
{
    public class ResponseNoWeb : Response, IResponse
    {
        public void Redirect(string url)
        {
            _redirections.Add(url);
        }

        public void Redirect(string url, bool endResponse)
        {
            _redirections.Add(url);
        }

        public void SetCookie(HttpCookie cookie)
        {
        	_cookiesCreated.RemoveAll(cook => cook.Name == cookie.Name);
            _cookiesCreated.Add(cookie);
        }
    }
}
