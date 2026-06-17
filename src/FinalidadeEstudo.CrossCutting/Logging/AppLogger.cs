using FinalidadeEstudo.Domain.Interfaces;
using Serilog;

namespace FinalidadeEstudo.CrossCutting.Logging;

public class AppLogger : IAppLogger
{
    public void LogInformation(string message, params object[] args) =>
        Log.Information(message, args);

    public void LogWarning(string message, params object[] args) =>
        Log.Warning(message, args);

    public void LogError(string message, params object[] args) =>
        Log.Error(message, args);

    public void LogError(Exception ex, string message, params object[] args) =>
        Log.Error(ex, message, args);

    public void LogDebug(string message, params object[] args) =>
        Log.Debug(message, args);
}