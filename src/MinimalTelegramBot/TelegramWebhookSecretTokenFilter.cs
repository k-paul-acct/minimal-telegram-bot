namespace MinimalTelegramBot;
using HttpResults = Microsoft.AspNetCore.Http.Results;

internal sealed class TelegramWebhookSecretTokenFilter : IEndpointFilter
{
    private const string SecretTokenHeaderName = "X-Telegram-Bot-Api-Secret-Token";

    private readonly string? _secretToken;

    public TelegramWebhookSecretTokenFilter(string? secretToken)
    {
        _secretToken = secretToken;
    }

    public ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        if (_secretToken is not null)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(SecretTokenHeaderName, out var headerValue))
            {
                return new ValueTask<object?>(HttpResults.StatusCode(StatusCodes.Status401Unauthorized));
            }

            if (headerValue != _secretToken)
            {
                return new ValueTask<object?>(HttpResults.StatusCode(StatusCodes.Status403Forbidden));
            }
        }

        return next(context);
    }
}
