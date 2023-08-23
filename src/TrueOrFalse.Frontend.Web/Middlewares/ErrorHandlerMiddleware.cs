using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace TrueOrFalse.Frontend.Web1.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ErrorHandlerMiddleware(RequestDelegate next, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment)
        {
            _next = next;
            _httpContextAccessor = httpContextAccessor;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            await _next(httpContext);  

            if (httpContext.Response.StatusCode == 404)
            {
                new Logg(_httpContextAccessor, _webHostEnvironment).Error(new NotFoundException("Ressource Not Found"));
            }
            else if (httpContext.Response.StatusCode == 500)
            {
                new Logg(_httpContextAccessor, _webHostEnvironment).Error(new NotFoundException("Internal Error"));

            }
            else if (httpContext.Response.StatusCode == 503)
            {
                new Logg(_httpContextAccessor, _webHostEnvironment).Error(new NotFoundException("Server unavailable"));
            }

        }
    }
}
public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}