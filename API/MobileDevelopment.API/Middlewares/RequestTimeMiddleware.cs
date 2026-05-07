using System.Diagnostics;

namespace MobileDevelopment.API.Middlewares
{
    public sealed class RequestTimeMiddleware : IMiddleware
    {
        private readonly Stopwatch _stopwatch;
        private readonly ILogger _logger;

        public RequestTimeMiddleware(ILogger<RequestTimeMiddleware> logger)
        {
            _stopwatch = new Stopwatch();
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            _stopwatch.Start();
            await next.Invoke(context);
            _stopwatch.Stop();

            var elapsedMiliseconds = _stopwatch.ElapsedMilliseconds;

            if (elapsedMiliseconds / 100  > 4)
            {
                var msg = $"Request [{context.Request.Method}] at {context.Request.Path} took {elapsedMiliseconds} ms.";
                _logger.LogInformation(msg);
            }
        }
    }
}
