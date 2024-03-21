namespace Backups.Exceptions;

public class StorageZeroArchivesException : Exception
{
    public StorageZeroArchivesException(string message)
        : base(message)
    {
    }
}