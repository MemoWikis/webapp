using System.Web;
using System.Web.SessionState;

namespace Seedworks.Web.State
{
    [Serializable]
    public class SessionBase
    {
        /// <summary>
		/// Calls Clear() on the encapsulated SessionDataLegacy object.
		/// </summary>
		public static void Clear() => SessionDataLegacy.Clear();
    }
}
