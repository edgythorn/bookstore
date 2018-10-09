using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace BooksStore.Host.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private const bool RESPONSE_EXCEPTIONS_DETAILS = false;

        private readonly IDictionary<string, HttpStatusCode> _exceptionToStatusCodeMapping = new Dictionary<string, HttpStatusCode>();

        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;
        private readonly IHostingEnvironment _environment;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger, IHostingEnvironment environment)
        {
            _next = next;
            _logger = logger;
            _environment = environment;

            InitializeStatusCodeMapping();
        }

        private void InitializeStatusCodeMapping()
        {
            var errs = _exceptionToStatusCodeMapping;

            errs.Add(typeof(KeyNotFoundException   ).FullName, HttpStatusCode.NotFound      );
            errs.Add(typeof(ArgumentException      ).FullName, HttpStatusCode.BadRequest    );
            errs.Add(typeof(ArgumentNullException  ).FullName, HttpStatusCode.BadRequest    );
            errs.Add(typeof(NotSupportedException  ).FullName, HttpStatusCode.BadRequest    );
            errs.Add(typeof(NotImplementedException).FullName, HttpStatusCode.NotImplemented);
            errs.Add(typeof(TimeoutException       ).FullName, HttpStatusCode.RequestTimeout);
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                if (!_exceptionToStatusCodeMapping.TryGetValue(ex.GetType().FullName, out HttpStatusCode code))
                {
                    if (_environment.IsDevelopment())
                    {
                        throw;
                    }
                    else
                    {
                        code = HttpStatusCode.InternalServerError;
                    }
                }

                await WriteExceptionAsync(context, ex, code).ConfigureAwait(false);
            }
        }


        private async Task WriteExceptionAsync(HttpContext context, Exception exception, HttpStatusCode code)
        {
            var response = context.Response;

            response.ContentType = "application/json";
            response.StatusCode = (int)code;

            object content = null;
            if (RESPONSE_EXCEPTIONS_DETAILS)
            {
                content = exception;
            }
            else
            {
                content = new
                {
                    error = new
                    {
                        message = exception.Message
                    }
                };
            }

            await response.WriteAsync(JsonConvert.SerializeObject(content)).ConfigureAwait(false);
        }
    }
}
