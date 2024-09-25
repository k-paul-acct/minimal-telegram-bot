using Microsoft.AspNetCore.Builder;

namespace MinimalTelegramBot.Builder;

public interface IPollingBuilder
{
    IPollingBuilder UseStaticFiles(string url);
    IPollingBuilder UseStaticFiles(string url, StaticFileOptions options);
}
