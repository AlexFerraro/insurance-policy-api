using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Caching.Memory;

namespace insurance_policy_api.Middlewares;

public class CacheMiddleware
{
    private readonly IMemoryCache _cache;
    private readonly RequestDelegate _next;

    public CacheMiddleware(IMemoryCache cache, RequestDelegate next) =>
        (_cache, _next) = (cache, next);

    public async Task Invoke(HttpContext context)
    {
        if (ShouldBypassCache(context))
        {
            await _next(context);
            return;
        }

        var cacheKey = GetCacheKey(context.Request);

        if (_cache.TryGetValue(cacheKey, out var response))
        {
            await WriteCachedResponse(context, response);
        }
        else
        {
            var cachedResponse = await HandleRequestAndCacheResponse(context);
            await WriteCachedResponse(context, cachedResponse);
        }
    }

    private bool ShouldBypassCache(HttpContext context) =>
        !context.Request.Method.Equals(HttpMethod.Get)
                || context.Request.GetTypedHeaders().CacheControl?.NoStore == true;

    private string GetCacheKey(HttpRequest request) =>
        request.GetEncodedPathAndQuery();

    private async Task<object> HandleRequestAndCacheResponse(HttpContext context)
    {
        var originalBodyStream = context.Response.Body;
        object response;

        try
        {
            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;
                await _next(context);
                context.Response.Body.Seek(0, SeekOrigin.Begin);
                response = await new StreamReader(context.Response.Body).ReadToEndAsync();
                context.Response.Body.Seek(0, SeekOrigin.Begin);
                await responseBody.CopyToAsync(originalBodyStream);
            }

            var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(3));
            _cache.Set(GetCacheKey(context.Request), response, cacheEntryOptions);
        }
        finally
        {
            context.Response.Body = originalBodyStream;
        }

        return response;
    }

    private async Task WriteCachedResponse(HttpContext context, object response)
    {
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(response.ToString());
    }
}