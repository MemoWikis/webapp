using System.Web.Mvc;

namespace VueApp;

public class Fetch
{
    public static JsonResult Success<T>(T data, bool get = false)
    {
        if (get)
            return new JsonResult
            {
                Data = new
                {
                    success = true,
                    data
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

        return new JsonResult
        {
            Data = new
            {
                success = true,
                data
            }
        };
    }

    public static JsonResult Error(string exceptionMessage, bool get = false)
    {
        if (get)
            return new JsonResult
            {
                Data = new
                {
                    success = false,
                    key = exceptionMessage
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

        return new JsonResult
        {
            Data = new
            {
                success = false,
                key = exceptionMessage
            }
        };
    }
}