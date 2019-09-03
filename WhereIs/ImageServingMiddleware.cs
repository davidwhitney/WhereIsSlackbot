using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace WhereIs
{
    public class ImageServingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            await next(context);
        }
    }
}