namespace RecrutamentoDiversidade.Middleware;

using System.Net;
using System.Text.Json;
using RecrutamentoDiversidade.ViewModels.Response;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IHostEnvironment _environment;

    public ExceptionMiddleware(
        RequestDelegate next,
        ILogger<ExceptionMiddleware> logger,
        IHostEnvironment environment)
    {
        _next = next;
        _logger = logger;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro não tratado: {Message}", ex.Message);

            await TratarExcecaoAsync(context, ex);
        }
    }

    private async Task TratarExcecaoAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var (statusCode, erro, mensagem) = exception switch
        {
            UnauthorizedAccessException =>
                (HttpStatusCode.Unauthorized,
                 "Não autorizado",
                 exception.Message),

            KeyNotFoundException =>
                (HttpStatusCode.NotFound,
                 "Recurso não encontrado",
                 exception.Message),

            InvalidOperationException =>
                (HttpStatusCode.UnprocessableEntity,
                 "Operação não permitida",
                 exception.Message),

            ArgumentException =>
                (HttpStatusCode.BadRequest,
                 "Dados inválidos",
                 exception.Message),

            _ =>
                (HttpStatusCode.InternalServerError,
                 "Erro interno",
                 _environment.IsDevelopment()
                     ? exception.Message
                     : "Ocorreu um erro inesperado. Tente novamente mais tarde.")
        };

        context.Response.StatusCode = (int)statusCode;

        var errorResponse = new ErrorResponse
        {
            Status = (int)statusCode,
            Erro = erro,
            Mensagem = mensagem,
            Timestamp = DateTime.UtcNow
        };

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var json = JsonSerializer.Serialize(errorResponse, options);
        await context.Response.WriteAsync(json);
    }
}