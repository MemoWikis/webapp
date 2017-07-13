using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace Seedworks.Web.State
{
    public interface IResponse
    {
        void Redirect(string url);
        void Redirect(string url, bool endResponse);

        void SetCookie(HttpCookie cookie);

        List<string> Redirections { get; }
        List<HttpCookie> CookiesCreated { get; }
    }
}
