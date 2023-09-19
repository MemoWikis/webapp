using System.Net.Http;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace TrueOrFalse.Web.Context
{
    internal static class SessionExtensions
    {
        public static void SetBool(this ISession session, string key, bool value)
        {
            session.Set(key, BitConverter.GetBytes(value));
        }

        public static bool GetBool(this ISession session, string key)
        {
            var data = session.Get(key);
            if (data == null)
            {
                return false;
            }
            return BitConverter.ToBoolean(data, 0);
        }

        public static T? Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ?  default : JsonSerializer.Deserialize<T>(value);
        }

        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        public static void ForceInit(this ISession session)
        {
            if (!session.Keys.Any())
                session.SetString("ForceInit", "true");
        }
    }
}
