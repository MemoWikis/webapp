using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;

namespace Seedworks.Web.State
{
    [Serializable]
    public class SessionBase
    {
        private static HttpSessionState SessionState{ get { return HttpContext.Current.Session; } }
        protected SessionData Data = new SessionData();

        public void Kill()
        {
            SessionState.Abandon();
            HttpContext.Current.Response.Redirect(HttpContext.Current.Request.Url.AbsoluteUri);
        }

        public bool IsSessionExpired()
        {
            if (SessionState != null)
            {
                if (SessionState.IsNewSession)
                {
                    string cookieHeader = HttpContext.Current.Request.Headers["Cookie"];
                }
            }
            return true;
        }

		/// <summary>
		/// Calls Clear() on the encapsulated SessionData object.
		/// </summary>
		public void Clear()
		{
			Data.Clear();
		}
    }
}
