using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Requests.Abstractions;

namespace MinimalTelegramBot;

internal sealed class TelegramBotClientFacade : ITelegramBotClientFacade
{
    private readonly ITelegramBotClient _client;
    private readonly TaskCompletionSource<HttpContent?>? _httpResultTcs;
    private bool _webhookResponseAvailable;

    public TelegramBotClientFacade(ITelegramBotClient client, bool webhookResponseAvailable)
    {
        _client = client;
        _webhookResponseAvailable = webhookResponseAvailable;

        if (_webhookResponseAvailable)
        {
            _httpResultTcs = new TaskCompletionSource<HttpContent?>();
        }
    }

    public Task<HttpContent?> WaitHttpContent()
    {
        return _httpResultTcs!.Task;
    }

    public void FlushHttpContent()
    {
        _httpResultTcs?.TrySetResult(null);
    }

    public Task<TResponse> MakeRequestAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        if (!_webhookResponseAvailable)
        {
            return _client.MakeRequestAsync(request, cancellationToken);
        }

        _webhookResponseAvailable = false;
        request.IsWebhookResponse = true;
        var content = request.ToHttpContent();
        _httpResultTcs!.TrySetResult(content);

        return Task.FromResult<TResponse>(default!);
    }

    public Task<bool> TestApiAsync(CancellationToken cancellationToken = default)
    {
        return _client.TestApiAsync(cancellationToken);
    }

    public Task DownloadFileAsync(string filePath, Stream destination, CancellationToken cancellationToken = default)
    {
        return _client.DownloadFileAsync(filePath, destination, cancellationToken);
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
}
