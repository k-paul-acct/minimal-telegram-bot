using Microsoft.AspNetCore.Builder;

namespace MinimalTelegramBot.Builder;

internal sealed class PollingBuilder : IPollingBuilder
{
    private Action<WebApplication>? _staticFilesAction;
    private string? _url;

    public IPollingBuilder UseStaticFiles(string url)
    {
        ArgumentNullException.ThrowIfNull(url);
        _url = url;
        _staticFilesAction = app => app.UseStaticFiles();
        return this;
    }

    public IPollingBuilder UseStaticFiles(string url, StaticFileOptions options)
    {
        ArgumentNullException.ThrowIfNull(url);
        ArgumentNullException.ThrowIfNull(options);

        _url = url;
        _staticFilesAction = app => app.UseStaticFiles(options);
        return this;
    }

    public PollingConfiguration Build()
    {
        return new PollingConfiguration
        {
            Url = _url,
            StaticFilesAction = _staticFilesAction,
        };
    }
}
