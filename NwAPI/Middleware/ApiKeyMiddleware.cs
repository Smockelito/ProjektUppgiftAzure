namespace NwAPI.Middleware
{
    public class ApiKeyMiddleware
    {
        private const string ApiKeyHeader = "X-Api-Key";
        private readonly RequestDelegate _next;

        public ApiKeyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IConfiguration config)
        {
            // Auth-endpoints ska inte kräva API-nyckel
            if (context.Request.Path.StartsWithSegments("/api/Auth"))
            {
                await _next(context);
                return;
            }

            if (!context.Request.Headers.TryGetValue(ApiKeyHeader, out var receivedKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("API-nyckel saknas.");
                return;
            }

            var validKey = config["ApiKey"];
            if (validKey != receivedKey)
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync("Ogiltig API-nyckel.");
                return;
            }

            await _next(context);
        }
    }
}
