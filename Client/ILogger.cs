namespace Copier.Client
{
    public interface ILogger
    {
        void LogInfo(string message);
        void LogError(string message);
        void LogWarning(string message);
        void LogDebug(string message);
    }
}