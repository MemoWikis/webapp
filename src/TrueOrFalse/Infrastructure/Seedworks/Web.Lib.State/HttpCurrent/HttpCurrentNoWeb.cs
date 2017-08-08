using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;

namespace Seedworks.Web.State
{
    public class HttpCurrentNoWeb : IHttpCurrent
    {
        IResponse IHttpCurrent.Response { get { return Response; } }
        IRequest IHttpCurrent.Request { get { return Request; } }
        IDictionary IHttpCurrent.ContextItems { get { return ContextItems; } }

        public RequestNoWeb Request { get; set; }
        public ResponseNoWeb Response { get; set; }
        public IDictionary ContextItems { get; set; }
		public string SessionId { get; set; }

		public HttpCurrentNoWeb()
		{
			Reset();
		}

        public void Reset()
        {
        	Request = new RequestNoWeb();
        	Response = new ResponseNoWeb();
        	ContextItems = new Dictionary<string, object>();
        	SessionId = DateTime.Now.ToString("s");
        }
    }
}
