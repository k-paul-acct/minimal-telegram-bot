using TelegramBotFramework.Pipeline;

namespace TelegramBotFramework.Extensions;

public static class UsePipeExtensions
{
    public static IBotApplicationBuilder UsePipe<TPipe>(this IBotApplicationBuilder app) where TPipe : IPipe
    {
        return app.UsePipe(typeof(TPipe));
    }

    private static IBotApplicationBuilder UsePipe(this IBotApplicationBuilder app, Type pipeType)
    {
        return app.Use((ctx, next) =>
        {
            var pipeService = (IPipe)ctx.ServiceProvider.GetRequiredService(pipeType);
            return pipeService.InvokeAsync(ctx, next);
        });
    }
}