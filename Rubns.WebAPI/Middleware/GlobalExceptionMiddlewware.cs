namespace Rubns.WebAPI.Middleware
{
    public class GlobalExceptionMiddlewware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {

            switch (exception)
            {
                case NotFoundException e:
                    await WriteProblemDetails(context, StatusCodes.Status404NotFound, e.Title, exception.Message);
                    return;
                case ResourceExistException e:
                    await WriteProblemDetails(context, StatusCodes.Status400BadRequest, e.Title, exception.Message);
                    return;
                case UnauthorizedException e:
                    await WriteProblemDetails(context, StatusCodes.Status401Unauthorized, e.Title, exception.Message);
                    return;
                case TokenInvalidException e:
                    await WriteProblemDetails(context, StatusCodes.Status401Unauthorized, e.Title, exception.Message);
                    return;
                case ArgumentException e:
                    await WriteProblemDetails(context, StatusCodes.Status400BadRequest, "Datos no validos", exception.Message);
                    return;
                default:
                    await WriteProblemDetails(context, StatusCodes.Status500InternalServerError, "Error interno", "Ocurrió un error inesperado. Intente nuevamente más tarde.");
                    return;
            }

        }

        private static async Task WriteProblemDetails(HttpContext context, int statusCode, string title, string detail)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var problem = new ProblemDetails
            {
                Title = title,
                Detail = detail,
                Status = statusCode,
                Type = $"https://developer.mozilla.org/en-US/docs/Web/HTTP/Reference/Status/{statusCode}"
            };

            await context.Response.WriteAsJsonAsync(problem);
        }
    }
}
