using System.Net;

namespace NZWalks.API.Middlewares
{
    public class GlobalExceptionHandlerMiddleware(RequestDelegate _next, ILogger<GlobalExceptionHandlerMiddleware> logger)
    {
        public async Task InvokeAsync(HttpContext httpContext)
        {
			try
			{
				await _next(httpContext);
			}
			catch (Exception ex)
			{
                var Id = Guid.NewGuid();

				logger.LogError(ex, $"Id : {Id} - {ex.Message}");

                httpContext.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                httpContext.Response.ContentType = "application/json";

                var error = new
                {
                    Id = Id,
                    ErrorMessage = "Something went wrong- Please try later!"
                };

                await httpContext.Response.WriteAsJsonAsync(error);

            }

        }
    }
}
