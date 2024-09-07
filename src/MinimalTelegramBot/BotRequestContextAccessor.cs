namespace MinimalTelegramBot;

internal sealed class BotRequestContextAccessor : IBotRequestContextAccessor
{
    private static readonly AsyncLocal<BotRequestContextHolder> _botRequestContextCurrent = new();

    public BotRequestContext? BotRequestContext
    {
        get => _botRequestContextCurrent.Value?.Context;
        set
        {
            var holder = _botRequestContextCurrent.Value;
            if (holder is not null)
            {
                holder.Context = null;
            }

            if (value is not null)
            {
                _botRequestContextCurrent.Value = new BotRequestContextHolder { Context = value, };
            }
        }
    }

    private sealed class BotRequestContextHolder
    {
        public BotRequestContext? Context;
    }
}
