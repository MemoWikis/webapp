using Microsoft.AspNetCore.Mvc;

namespace VueApp;

public class Fetch
{
    public static JsonResult Success<T>(T data)
    {
            return new JsonResult(new
            {
                success = true,
                data
            });
    }

    public static JsonResult Error(string exceptionMessage)
    {
            return new JsonResult(new
            {
                success = false,
                key = exceptionMessage
            });
    }
}