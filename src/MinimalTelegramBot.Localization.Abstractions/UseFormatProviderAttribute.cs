namespace MinimalTelegramBot.Localization.Abstractions;

/// <summary>
///     Attribute to indicate that a parameter should use a format provider.
/// </summary>
[AttributeUsage(AttributeTargets.Parameter)]
public sealed class UseFormatProviderAttribute : Attribute;
