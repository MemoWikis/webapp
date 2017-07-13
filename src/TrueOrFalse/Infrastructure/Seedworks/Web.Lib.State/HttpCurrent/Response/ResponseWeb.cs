using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Seedworks.Web.State
{
    public class ResponseWeb : Response, IResponse
    { 
        static HttpResponse _response { get { return HttpContext.Current.Response; } }

        public void Redirect(string url)
        {
            _redirections.Add(url);
            _response.Redirect(url);
        }

        public void Redirect(string url, bool endResponse)
        {
            _redirections.Add(url);
            _response.Redirect(url, endResponse);
        }

        public void SetCookie(HttpCookie cookie)
        {
            _cookiesCreated.Add(cookie);
            _response.SetCookie(cookie);
        }
    }
}
