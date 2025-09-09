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

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var problemDetails = new ProblemDetails
            {
                Title = "Error interno",
                Detail = "Ocurrió un error inesperado. Intente nuevamente más tarde.",
                Status = StatusCodes.Status500InternalServerError,
                Type = "https://httpstatuses.com/500"
            };

            switch (exception)
            {
                case NotFoundException e:
                    problemDetails.Title = e.Title;
                    problemDetails.Detail = exception.Message;
                    problemDetails.Status = StatusCodes.Status404NotFound;
                    problemDetails.Type = "https://httpstatuses.com/404";
                    break;
                case ResourceExistException e:
                    problemDetails.Title = e.Title;
                    problemDetails.Detail = exception.Message;
                    problemDetails.Status = StatusCodes.Status400BadRequest;
                    problemDetails.Type = "https://httpstatuses.com/404";
                    break;
                case UnauthorizedException e:
                    problemDetails.Title = e.Title;
                    problemDetails.Detail = exception.Message;
                    problemDetails.Status = StatusCodes.Status401Unauthorized;
                    problemDetails.Type = "https://httpstatuses.com/401";
                    break;
                case TokenInvalidException e:
                    problemDetails.Title = e.Title;
                    problemDetails.Detail = exception.Message;
                    problemDetails.Status = StatusCodes.Status400BadRequest;
                    problemDetails.Type = "https://httpstatuses.com/400";
                    break;
                case ArgumentException e:
                    problemDetails.Title = "Datos no validos";
                    problemDetails.Detail = exception.Message;
                    problemDetails.Status = StatusCodes.Status400BadRequest;
                    problemDetails.Type = "https://httpstatuses.com/400";
                    break;
            }


            context.Response.ContentType = "application/json";
            context.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;

            var result = JsonSerializer.Serialize(problemDetails);
            return context.Response.WriteAsync(result);
        }

    }
}
