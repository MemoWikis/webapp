using System.Web;
using System.Collections.Generic;

namespace Seedworks.Web.State
{
	/// <summary>
	/// Ermöglicht einen verallgemeinerten Zugriff auch Benutzerdaten, 
	/// sowohl für den Web- als auch für einen allgemeinen Awendungskontext.
	/// </summary>
	[Serializable]
	public class SessionData : IRegisterAsInstancePerLifetime
	{
        private readonly HttpContext _httpContext;
        private readonly HashSet<string> _appDomainInsertedKeys = new();

        public SessionData(HttpContext httpContext)
        {
            _httpContext = httpContext;
        }

        /// <summary>
        /// Returns the untyped item for the given key. May be <b>null</b>.
        /// </summary>
		public object Get(string key)
		{
            if (ContextUtil.IsWebContext)
			{
				if (_httpContext.Session == null)
					throw new NullReferenceException("Probably you access session data too late or too early in the page life cycle.");

				return _httpContext.Session[key];
			}

            return AppDomain.CurrentDomain.GetData(key);
        }

		public void Set(string key, object value)
        {
            if (ContextUtil.IsWebContext)
                _httpContext.Session[key] = value;
            else
                AppDomain.CurrentDomain.SetData(key, value);

            _appDomainInsertedKeys.Add(key);
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
		public T Get<T>(string key, T initialValue) => Get(key, () => initialValue);

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
            Set(key, initialValue);
			return initialValue;
		}

		public void Clear()
		{
			if (ContextUtil.IsWebContext)
				foreach (var key in _appDomainInsertedKeys)
					_httpContext.Session.Remove(key);
			else
				foreach (var key in _appDomainInsertedKeys)
					AppDomain.CurrentDomain.SetData(key, null);

			_appDomainInsertedKeys.Clear();
		}
    }
}