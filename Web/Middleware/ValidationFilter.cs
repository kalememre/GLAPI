using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Results;
using static Web.Middleware.ExceptionMiddleware;

namespace Web.Middleware
{
    public class ValidationFilter : IFluentValidationAutoValidationResultFactory
    {
        private readonly ILogger<ValidationFilter> _logger;

        public ValidationFilter(ILogger<ValidationFilter> logger)
        {
            _logger = logger;
        }
        public IActionResult CreateActionResult(ActionExecutingContext context, ValidationProblemDetails? validationProblemDetails)
        {
            ArgumentNullException.ThrowIfNull(validationProblemDetails);
            return CreateExceptionActionResult(context.HttpContext, validationProblemDetails);
        }

        public IActionResult CreateExceptionActionResult(HttpContext context, ValidationProblemDetails ex)
        {
            // Log validation errors
            _logger.LogWarning("Validation failed: {Errors}", string.Join(", ", ex.Errors.Select(e => e.Value.First())));

            context.Response.ContentType = MediaTypeNames.Application.Json;
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            var errorModel = new ErrorModel(context.Response.StatusCode, ex.Errors, ex.Title);
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            var json = JsonSerializer.Serialize(errorModel, options);
            return new ContentResult
            {
                Content = json,
                ContentType = MediaTypeNames.Application.Json,
                StatusCode = (int)HttpStatusCode.BadRequest
            };
        }
    }
}