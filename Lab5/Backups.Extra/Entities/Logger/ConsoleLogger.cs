namespace Backups.Extra.Entities.Logger;

public class ConsoleLogger : ILogger
{
    public void CreateLog(string logMessage)
    {
        if (string.IsNullOrWhiteSpace(logMessage))
            throw new ArgumentException("log message must contain information");
        Console.WriteLine(logMessage);
    }
}