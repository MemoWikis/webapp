using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Web;
using System.Web.SessionState;

namespace Seedworks.Web.State
{
	public static class RequestResponseExtensions
	{
		/// <summary>
		/// Throws an HTTP Exception to return a 410 HTTP status code.
		/// </summary>
		/// <remarks>
		/// 410 Gone
		/// Indicates that the resource requested is no longer available and will not be available again.
		/// This should be used when a resource has been intentionally removed; however, it is not necessary
		/// to return this code and a 404 Not Found can be issued instead. Upon receiving a 410 status code,
		/// the client should not request the resource again in the future. Clients such as search engines
		/// should remove the resource from their indexes.
		/// </remarks>
		/// <param name="response"></param>
		public static void SetResourceIsGoneAndFinishRequest(this HttpResponse response)
		{
			throw new HttpException(410, "410 Gone");
		}

		/// <summary>
		/// Throws an HTTP Exception to return a 410 HTTP status code.
		/// </summary>
		/// <param name="response"></param>
		public static void SetResourceNotFoundAndFinishRequest(this HttpResponse response)
		{
			throw new HttpException(404, "404 Not Found");
		}

		public static void RedirectToSamePage(this HttpResponse response, HttpRequest request)
		{
			response.Redirect(request.RawUrl);
		}

		public static string GetIpAddress(this HttpRequest request)
		{
			var ip = request.UserHostAddress;

			if (!string.IsNullOrEmpty(ip))
				return ip;

			// See: http://forums.asp.net/p/1053767/1496008.aspx

			ip = request.ServerVariables["HTTP_X_FORWARDED_FOR"];

			// If there is no proxy, get the standard remote address

			if (string.IsNullOrEmpty(ip) || (ip.ToLowerInvariant() == "unknown"))
				ip = request.ServerVariables["REMOTE_ADDR"];

			return ip ?? "unknown";
		}

		/// <summary>
		/// Return the size of the session in Byte. Uses Serialization, so use with care!
		/// </summary>
		/// <param name="session"></param>
		/// <returns></returns>
		public static long GetSize(this HttpSessionState session)
		{
			long totalSessionBytes = 0;
			var b = new BinaryFormatter();
			MemoryStream m;
			foreach (var t in session)
			{
				m = new MemoryStream();
				b.Serialize(m, t);
				totalSessionBytes += m.Length;
			}
			return totalSessionBytes;
		}
	}
}
