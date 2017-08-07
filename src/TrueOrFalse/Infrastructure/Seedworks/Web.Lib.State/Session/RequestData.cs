using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;

namespace Seedworks.Web.State
{
	/// <summary>
	/// Enables access to data that should be available in a request scope.
	/// </summary>
	[Serializable]
	public class RequestData
	{
		private readonly IDictionary _requestData;

		public RequestData()
		{
			_requestData = ContextUtil.IsWebContext
			               	? HttpContext.Current.Items
			               	: new Dictionary<string, object>();
		}

		public object this[string key]
		{
			private get { return _requestData[key]; }
			set { _requestData.Add(key, value); }
		}

		/// <summary>
		/// Use only if truly necessary because it adds an unnecessary check if you plan to get the element anyways;
		/// else simply use <see cref="Get{T}(string,System.Func{T})"/>.
		/// </summary>
		public bool Exists(string key)
		{
			return this[key] != null;
		}

		/// <summary>
		/// Returns the untyped item for the given key. May be <b>null</b>.
		/// </summary>
		public object Get(string key)
		{
			return this[key];
		}

		/// <summary>
		/// Returns the typed item for the given key. May be <b>null</b>.
		/// Cannot use this for value types because an exception would be thrown if the value does not exist.
		/// <br/>
		/// Consider using <see cref="Get{T}(string,T)"/> for value types, or e.g. Get(key, (int?) null) for nullables.
		/// </summary>
		public T Get<T>(string key) where T : class
		{
			return (T) this[key];
		}

		/// <summary>
		/// Returns the item for the given key. 
		/// <br/>
		/// If the key does not exist, session will be initialized 
		/// with the given initialValue &amp; the initialValue will 
		/// be returned.
		/// <br/>
		/// Consider using <see cref="Get{T}(string,System.Func{T})"/> for better performance.
		/// </summary>
		public T Get<T>(string key, T initialValue)
		{
			return Get(key, () => initialValue);
		}

		/// <summary>
		/// Returns the item for the given key. 
		/// <br/>
		/// If the key does not exist, session will be initialized with 
		/// the return value of the given initializer &amp; that value will 
		/// be returned.
		/// <br/>
		/// Consider using <see cref="Get{T}(string,T)"/> for value types.
		/// </summary>
		public T Get<T>(string key, Func<T> initializer)
		{
			var val = Get(key);

			if (val != null)
				return (T) val;

			var initialValue = initializer();
			this[key] = initialValue;
			return initialValue;
		}

		public void Clear()
		{
			_requestData.Clear();
		}
	}
}