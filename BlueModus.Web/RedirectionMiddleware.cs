using RedirectorService;
namespace BlueModus.Web
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class RedirectionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IRedirectorService _redirectorService;
        private readonly ILogger<RedirectionMiddleware> _logger;

        public RedirectionMiddleware(RequestDelegate next, IRedirectorService redirectorService, ILogger<RedirectionMiddleware> logger)
        {
            _next = next;
            _redirectorService = redirectorService;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            //get a tuple from the redirectorService with the targetUrl and redirectType
            var (targetPath, redirectType) = await _redirectorService.CheckForRedirect(httpContext.Request.Path);
            if (targetPath != httpContext.Request.Path)
                httpContext.Response.Redirect(targetPath, redirectType == 301 ? true : false);
            else
                await _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class RedirectionMiddlewareExtensions
    {
        public static IApplicationBuilder UseRedirectionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RedirectionMiddleware>();
        }
    }
}
