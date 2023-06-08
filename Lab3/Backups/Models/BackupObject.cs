using Backups.Exceptions;

namespace Backups.Models;

public class BackupObject
{
    public BackupObject(string pathOfBackupObject)
    {
        if (string.IsNullOrWhiteSpace(pathOfBackupObject))
            throw PathException.PathIsNullException();
        PathOfBackupObject = pathOfBackupObject;
    }

    public string PathOfBackupObject { get; }
}