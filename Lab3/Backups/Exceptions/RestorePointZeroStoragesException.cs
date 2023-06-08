namespace Backups.Exceptions;

public class RestorePointZeroStoragesException : Exception
{
    public RestorePointZeroStoragesException(string message)
        : base(message)
    {
    }
}