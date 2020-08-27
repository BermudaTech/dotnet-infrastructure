using Bermuda.Core.Contract.Service;
using Bermuda.Core.Logger;
using Bermuda.Core.Serialization;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Bermuda.Core.WebApi.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly ILogger logger;
        private readonly IJsonSerializer jsonSerializer;
        private readonly RequestDelegate next;

        public ErrorHandlingMiddleware(
            ILogger logger,
            IJsonSerializer jsonSerializer,
            RequestDelegate next)
        {
            this.logger = logger;
            this.jsonSerializer = jsonSerializer;
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                logger.Write(LogType.Error, $"ErrorMiddlewareExLog: {ex.Message}", ex);
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            string message = "Unexpected error occurred!";

            if (exception is BusinessException)
            {
                message = exception.Message;
            }

            return context.Response.WriteAsync(jsonSerializer.Serialize(new ResponseBase()
            {
                IsSuccess = false,
                Message = message

            }, CaseStyleType.CamelCase));
        }
    }
}
