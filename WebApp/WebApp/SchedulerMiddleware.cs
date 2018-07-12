using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using WebApp.Utils;

namespace WebApp
{
    //// You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    //public class SchedulerMiddleware
    //{
    //    private readonly RequestDelegate _next;

    //    public SchedulerMiddleware(RequestDelegate next)
    //    {
    //        _next = next;
    //    }

    //    public async Task Invoke(HttpContext httpContext, IRep rep)
    //    {
    //        httpContext.Response.ContentType = "text/html;charset=utf-8";
    //        await httpContext.Response.WriteAsync($"Рез-т{rep.GetSch()}");
    //    }
    //}

    //// Extension method used to add the middleware to the HTTP request pipeline.
    //public static class SchedulerMiddlewareExtensions
    //{
    //    public static IApplicationBuilder UseSchedulerMiddleware(this IApplicationBuilder builder)
    //    {
    //        return builder.UseMiddleware<SchedulerMiddleware>();
    //    }
    //}
}
