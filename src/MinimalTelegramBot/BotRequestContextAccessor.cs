namespace MinimalTelegramBot;

internal sealed class BotRequestContextAccessor : IBotRequestContextAccessor
{
    private static readonly AsyncLocal<BotRequestContextHolder> BotRequestContextCurrent = new();

    public BotRequestContext? BotRequestContext
    {
        get => BotRequestContextCurrent.Value?.Context;
        set
        {
            var holder = BotRequestContextCurrent.Value;
            if (holder is not null)
            {
                holder.Context = null;
            }

            if (value is not null)
            {
                BotRequestContextCurrent.Value = new BotRequestContextHolder { Context = value, };
            }
        }
    }

    private sealed class BotRequestContextHolder
    {
        public BotRequestContext? Context;
    }
}
