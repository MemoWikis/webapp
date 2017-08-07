using System;
using System.Web;
using System.Collections.Generic;

namespace Seedworks.Web.State
{
	/// <summary>
	/// Ermöglicht einen verallgemeinerten Zugriff auch Benutzerdaten, 
	/// sowohl für den Web- als auch für einen allgemeinen Awendungskontext.
	/// </summary>
	[Serializable]
	public class SessionData
	{
        private readonly HashSet<string> _appDomainInsertedKeys = new HashSet<string>();

		public object this[string key]
		{
			get
			{
				if (ContextUtil.IsWebContext)
				{
					if (HttpContext.Current.Session == null)
						throw new NullReferenceException("Probably you access session data to late or to early in the page life cycle.");

					return HttpContext.Current.Session[key];
				}

				return AppDomain.CurrentDomain.GetData(key);
			}
			set
			{
				if (ContextUtil.IsWebContext)
				{
					HttpContext.Current.Session[key] = value;
				}
				else
				{
					AppDomain.CurrentDomain.SetData(key, value);
				}
				_appDomainInsertedKeys.Add(key);
			}
		}

		/// <summary>
		/// Use only if truly necessary; else simply use <see cref="Get{T}(string,System.Func{T})"/>.
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
			if (ContextUtil.IsWebContext)
				foreach (var key in _appDomainInsertedKeys)
					HttpContext.Current.Session.Remove(key);
			else
				foreach (var key in _appDomainInsertedKeys)
					AppDomain.CurrentDomain.SetData(key, null);

			_appDomainInsertedKeys.Clear();
		}

		public void Remove(string key)
		{
			if (ContextUtil.IsWebContext)
			{
				HttpContext.Current.Session.Remove(key);
			}
			else
			{
				AppDomain.CurrentDomain.SetData(key, null);
				_appDomainInsertedKeys.Remove(key);
			}
		}
	}
}