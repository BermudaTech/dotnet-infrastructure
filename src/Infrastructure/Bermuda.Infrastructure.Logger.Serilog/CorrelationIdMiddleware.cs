namespace Bermuda.Infrastructure.Logger.Serilog;

using Bermuda.Core.Logger;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
public class CorrelationIdMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger logger;

    public CorrelationIdMiddleware(RequestDelegate next, ILogger logger)
    {
        _next = next;
        this.logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        // 1. İstekten X-Correlation-ID al, yoksa yeni oluştur
        var correlationId = context.Request.Headers["X-Correlation-ID"].FirstOrDefault()
            ?? Guid.NewGuid().ToString();

        // 2. Response header'a da yaz (izlenebilirlik için)
        context.Response.Headers["X-Correlation-ID"] = correlationId;

        // 3. Scope başlat: bu scope içindeki tüm loglarda correlationId yer alacak
        using (logger.GenerateCorrelationId(correlationId))
        {
            await _next(context);
        }
    }
}