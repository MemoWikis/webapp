using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Seedworks.Web.State
{
	public class ResponseActionFilter : Stream
	{
		public static Regex RegexFormAction = new Regex("(?'pre'<form.*action=\")(?'action'[^\"]*)(?'post'\"[^>]*>)",
		                                                RegexOptions.IgnoreCase | RegexOptions.Compiled);
		public static Regex RegexFormActionPostback = new Regex("(?'pre'\\|formAction\\|\\|)(?'action'[^\\|]*)(?'post'\\|)",
														RegexOptions.IgnoreCase | RegexOptions.Compiled);
		private readonly Stream _sink;
		private readonly HttpServerUtility _server;
		
		public ResponseActionFilter(Stream sink, HttpServerUtility server)
		{
			_sink = sink;
			_server = server;
		}

		public override bool CanRead
		{
			get { return true; }
		}

		public override bool CanSeek
		{
			get { return true; }
		}

		public override bool CanWrite
		{
			get { return true; }
		}

		public override long Length
		{
			get { return 0; }
		}

		public override long Position { get; set; }

		public override long Seek(long offset, SeekOrigin direction)
		{
			return _sink.Seek(offset, direction);
		}

		public override void SetLength(long length)
		{
			_sink.SetLength(length);
		}

		public override void Close()
		{
			_sink.Close();
		}

		public override void Flush()
		{
			_sink.Flush();
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			return _sink.Read(buffer, offset, count);
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			string s = Encoding.UTF8.GetString(buffer, offset, count);
			Match match = RegexFormAction.Match(s);
			if (match.Success)
			{
				var action = match.Groups["action"].Value;
				string stringNew = RegexFormAction.Replace(s, 
					match.Groups["pre"].Value + _server.UrlEncode(action) + match.Groups["post"].Value);
				s = stringNew;
			}
			else
			{
				// Do not encode the formAction field in the Postback because it leads to an exception on the client :-(
				// But this is how it would be done...
//				match = RegexFormActionPostback.Match(s);
//				if (match.Success)
//				{
//					var action = match.Groups["action"].Value;
//					string stringNew = RegexFormActionPostback.Replace(s,
//						match.Groups["pre"].Value + _server.UrlEncode(action) + match.Groups["post"].Value);
//					s = stringNew;
//				}
			}
			byte[] bytes = Encoding.UTF8.GetBytes(s);
			_sink.Write(bytes, 0, bytes.Length);
		}
	}
}
