namespace Backups.Extra.Entities.Logger;

public class FileLogger : ILogger
{
    private readonly string _loggerFilePath;

    public FileLogger(string loggerFilePath)
    {
        if (!File.Exists(loggerFilePath))
            throw new ArgumentException("path to logger's file should point to file");
        _loggerFilePath = loggerFilePath;
    }

    public void CreateLog(string logMessage)
    {
        if (string.IsNullOrWhiteSpace(logMessage))
            throw new ArgumentException("log message must contain information");
        File.AppendAllText(_loggerFilePath, logMessage);
    }
}