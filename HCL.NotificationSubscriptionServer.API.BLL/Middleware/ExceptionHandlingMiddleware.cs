﻿using Confluent.Kafka;
using Confluent.Kafka.Admin;
using HCL.NotificationSubscriptionServer.API.Domain.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.NetworkInformation;
using System.Security.Authentication;

namespace HCL.NotificationSubscriptionServer.API.BLL.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (KeyNotFoundException ex)
            {
                await HandleExceptionAsync(httpContext,
                    ex.Message,
                    (int)HttpStatusCode.NotFound,
                    "Entity not found");
            }
            catch (CreateTopicsException ex)
            {
                await HandleExceptionAsync(httpContext,
                    ex.Results[0].Error.Reason,
                    (int)HttpStatusCode.InternalServerError,
                    "Crete kafka topic error");
            }
            catch (AuthenticationException ex)
            {
                await HandleExceptionAsync(httpContext,
                    ex.Message,
                    (int)HttpStatusCode.Unauthorized,
                    "Authentication error");
            }
            catch (KafkaException ex)
            {
                await HandleExceptionAsync(httpContext,
                    ex.Message,
                    521,
                    "Kafka server error");
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext,
                    ex.Message,
                    (int)HttpStatusCode.InternalServerError,
                    "Internal server error");
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, string exMsg, int httpStatusCode, string message)
        {
            _logger.LogError(exMsg);

            HttpResponse response = context.Response;

            response.ContentType = "application/json";
            response.StatusCode = httpStatusCode;

            ErrorDTO errorDto = new()
            {
                Message = message,
                StatusCode = httpStatusCode
            };
            await response.WriteAsJsonAsync(errorDto);
        }
    }
}
