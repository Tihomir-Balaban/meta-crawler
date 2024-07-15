using Serilog;

namespace Meta.Crawler.Utilities;

internal static class Logger
{
    public static void Init() =>
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

    public static void LogInformation(string message)
        => Log.Information(message);

    public static void LogWarning(string message)
        => Log.Warning(message);

    public static void LogError(string message)
        => Log.Error(message);
}
