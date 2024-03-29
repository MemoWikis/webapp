﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace TrueOrFalse.Frontend.Web1.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            await _next(httpContext);  

            if (httpContext.Response.StatusCode == 404)
            {
                Logg.Error(new NotFoundException("Ressource Not Found"));
            }
            else if (httpContext.Response.StatusCode == 500)
            {
                Logg.Error(new NotFoundException("Internal Error"));
            }
            else if (httpContext.Response.StatusCode == 503)
            {
                Logg.Error(new NotFoundException("Server unavailable"));
            }

        }
    }
}
public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}