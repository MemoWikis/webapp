using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using Seedworks.Lib.Web;

namespace Seedworks.Web.State
{
    public class RequestNoWeb : IRequest
    {
        public Uri Url { get; set; }
        public string[] UserLanguages { get; set; }
        public HttpCookieCollection Cookies { get; set; }
        public NameValueCollection QueryString { get { return UriUtils.GetNameValueCollectionFromQuery(Url.Query); } }
		public string Path { get { return Url.PathAndQuery.Split('?')[0]; } }

		/// <summary>
		/// TODO: Use UrlRewriting
		/// </summary>
		public string FilePath { get { return Url.PathAndQuery.Split('?')[0]; } }

    	public RequestNoWeb()
        {
            UserLanguages = new string[]{};
            Cookies = new HttpCookieCollection();
        }
        
        public void Clear()
        {
            throw new NotImplementedException();
        }
    }
}
