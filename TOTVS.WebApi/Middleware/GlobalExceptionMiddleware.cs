using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace TOTVS.WebApi.Middleware
{
    public class GlobalExceptionMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch(Exception ex)
            {
                context.Response.StatusCode = 500;
                await context.Response.WriteAsJsonAsync(new { Mensagem = ex.Message });
            }
        }
    }
}
