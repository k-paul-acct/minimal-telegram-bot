using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Requests.Abstractions;

namespace MinimalTelegramBot.Client;

internal sealed class WebhookTelegramBotClient : ITelegramBotClient
{
    private readonly ITelegramBotClient _client;
    private readonly TaskCompletionSource<HttpContent?> _httpContentTcs;
    private bool _webhookResponseUsed;

    public WebhookTelegramBotClient(ITelegramBotClient client)
    {
        _client = client;
        _httpContentTcs = new TaskCompletionSource<HttpContent?>(TaskCreationOptions.RunContinuationsAsynchronously);
    }

    public Task<TResponse> SendRequest<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        if (_webhookResponseUsed || !request.IsWebhookResponseAvailable())
        {
            return _client.SendRequest(request, cancellationToken);
        }

        _webhookResponseUsed = true;
        request.IsWebhookResponse = true;
        var content = request.ToHttpContent();
        _httpContentTcs.TrySetResult(content);

        return Task.FromResult<TResponse>(default!);
    }

    public Task<TResponse> MakeRequest<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        return SendRequest(request, cancellationToken);
    }

    public Task<TResponse> MakeRequestAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        return SendRequest(request, cancellationToken);
    }

    public Task<bool> TestApi(CancellationToken cancellationToken = default)
    {
        return _client.TestApi(cancellationToken);
    }

    public Task DownloadFile(string filePath, Stream destination, CancellationToken cancellationToken = default)
    {
        return _client.DownloadFile(filePath, destination, cancellationToken);
    }

    public bool LocalBotServer => _client.LocalBotServer;
    public long BotId => _client.BotId;

    public TimeSpan Timeout
    {
        get => _client.Timeout;
        set => _client.Timeout = value;
    }

    public IExceptionParser ExceptionsParser
    {
        get => _client.ExceptionsParser;
        set => _client.ExceptionsParser = value;
    }

    public event AsyncEventHandler<ApiRequestEventArgs>? OnMakingApiRequest
    {
        add => _client.OnMakingApiRequest += value;
        remove => _client.OnMakingApiRequest -= value;
    }

    public event AsyncEventHandler<ApiResponseEventArgs>? OnApiResponseReceived
    {
        add => _client.OnApiResponseReceived += value;
        remove => _client.OnApiResponseReceived -= value;
    }

    public Task<HttpContent?> WaitHttpContent()
    {
        return _httpContentTcs.Task;
    }

    public void AbortWaiting()
    {
        _httpContentTcs.TrySetResult(null);
    }
}
