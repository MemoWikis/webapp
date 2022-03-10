using System;
using System.Web;
using System.Web.SessionState;

namespace Seedworks.Web.State
{
    [Serializable]
    public class SessionBase
    {
        private static HttpSessionState SessionState => HttpContext.Current.Session;

        public static void Kill()
        {
            SessionState.Abandon();
            HttpContext.Current.Response.Redirect(HttpContext.Current.Request.Url.AbsoluteUri);
        }

		/// <summary>
		/// Calls Clear() on the encapsulated SessionData object.
		/// </summary>
		public static void Clear() => SessionData.Clear();
    }
}
