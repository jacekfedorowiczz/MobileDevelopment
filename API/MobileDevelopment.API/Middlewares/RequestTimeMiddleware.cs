using System.Diagnostics;

namespace MobileDevelopment.API.Middlewares
{
    public sealed class RequestTimeMiddleware : IMiddleware
    {
        private const long SlowRequestThresholdMilliseconds = 500;

        private readonly ILogger<RequestTimeMiddleware> _logger;

        public RequestTimeMiddleware(ILogger<RequestTimeMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var stopwatch = Stopwatch.StartNew();

            try
            {
                await next.Invoke(context);
            }
            finally
            {
                stopwatch.Stop();

                if (stopwatch.ElapsedMilliseconds >= SlowRequestThresholdMilliseconds)
                {
                    _logger.LogWarning(
                        "Slow request {Method} {Path} responded {StatusCode} in {ElapsedMilliseconds} ms.",
                        context.Request.Method,
                        context.Request.Path,
                        context.Response.StatusCode,
                        stopwatch.ElapsedMilliseconds);
                }
            }
        }
    }
}
