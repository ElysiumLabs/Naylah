using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

#if NETCOREAPP3_0_OR_GREATER
using Microsoft.Extensions.Hosting;
#endif

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Naylah
{
    public abstract partial class Service<TOptions>
    {
        protected virtual void ConfigureProblemDetails(ProblemDetailsOptions options,
#if NETCOREAPP3_0_OR_GREATER
        IHostEnvironment environment
#endif

#if NETCOREAPP2_1
        IHostingEnvironment environment
#endif
            )
        {
            // This is the default behavior; only include exception details in a development environment.
            options.IncludeExceptionDetails = (ctx, exception) => environment.IsDevelopment();

            // This will map NotImplementedException to the 501 Not Implemented status code.
            options.Map<NotImplementedException>(ex => new ExceptionProblemDetails(ex, StatusCodes.Status501NotImplemented));

            // This will map HttpRequestException to the 503 Service Unavailable status code.
            options.Map<HttpRequestException>(ex => new ExceptionProblemDetails(ex, StatusCodes.Status400BadRequest));

            // Because exceptions are handled polymorphically, this will act as a "catch all" mapping, which is why it's added last.
            // If an exception other than NotImplementedException and HttpRequestException is thrown, this will handle it.
            options.Map<Exception>((ex) =>
            {
                var problemDetailsGen = ex as IProblemDetailsGenerator;
                TraceProblemDetails problemDetails = null;

                if (problemDetailsGen != null)
                {
                    problemDetails = problemDetailsGen.GetProblemDetails();
                }
                else
                {
                    problemDetails = ExceptionToProblemDetail(ex);
                }

                LogProblemDetails(problemDetails);

                return problemDetails;
            });
        }

        protected virtual void LogProblemDetails(TraceProblemDetails problemDetails)
        {
        }

        protected virtual TraceProblemDetails ExceptionToProblemDetail(Exception exception)
        {
            return new TraceProblemDetails(StatusCodes.Status400BadRequest)
            {
                TraceId = $"{Guid.NewGuid()}",
                Date = DateTimeOffset.UtcNow,
                Title = exception.Message,
                Status = 400,
                Detail = exception.InnerException?.Message,
            };
        }
    }

    public interface IProblemDetailsGenerator
    {
        TraceProblemDetails GetProblemDetails();

    }

    public class TraceProblemDetails : StatusCodeProblemDetails
    {
        public TraceProblemDetails(int statusCode) : base(statusCode)
        {
        }

        public string TraceId { get; set; }
        public DateTimeOffset Date { get; set; }
    }
}
