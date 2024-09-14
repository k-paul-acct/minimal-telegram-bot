using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Requests.Abstractions;

namespace MinimalTelegramBot.Client;

internal sealed class PollingTelegramBotClient : ITelegramBotClient
{
    private readonly ITelegramBotClient _client;

    public PollingTelegramBotClient(ITelegramBotClient client)
    {
        _client = client;
    }

    public Task<TResponse> MakeRequestAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        return _client.MakeRequestAsync(request, cancellationToken);
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
