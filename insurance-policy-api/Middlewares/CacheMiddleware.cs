using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Caching.Memory;

namespace insurance_policy_api.Middlewares;

public class CacheMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IMemoryCache _cache;

    public CacheMiddleware(RequestDelegate next, IMemoryCache cache)
    {
        _next = next;
        _cache = cache;
    }

    public async Task Invoke(HttpContext context)
    {
         if (context.Request.Method.Equals("GET"))
         {
            var cacheKey = context.Request.GetEncodedPathAndQuery();

            //if (context.Request.Headers.TryGetValue("Cache-Control", out var cacheControl) 
            //        && cacheControl.ToString().Contains("no-cache"))
            //{
            //    await _next(context);
            //    return;
            //}

            if (_cache.TryGetValue(cacheKey, out var response))
            {
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(response.ToString());

                return;
            }
            else
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(15));
                var originalBodyStream = context.Response.Body;

                using (var responseBody = new MemoryStream())
                {
                    context.Response.Body = responseBody;
                    await _next(context);
                    context.Response.Body.Seek(0, SeekOrigin.Begin);
                    response = await new StreamReader(context.Response.Body).ReadToEndAsync();
                    context.Response.Body.Seek(0, SeekOrigin.Begin);
                    await responseBody.CopyToAsync(originalBodyStream);
                }

                _cache.Set(cacheKey, response, cacheEntryOptions);
            }
        }
        else
        {
            await _next(context);
        }
    }
}
