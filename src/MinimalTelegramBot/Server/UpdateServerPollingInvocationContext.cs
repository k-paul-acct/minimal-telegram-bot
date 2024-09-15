using Microsoft.Extensions.DependencyInjection;

namespace MinimalTelegramBot.Server;

internal sealed class UpdateServerPollingInvocationContext : UpdateServerInvocationContext
{
    public UpdateServerPollingInvocationContext(AsyncServiceScope serviceScope, BotRequestContext botRequestContext)
        : base(serviceScope, botRequestContext)
    {
    }
}
