using Backups.Models;

namespace Backups.Algorithms;

public interface IStorageAlgorithm
{
    RestorePoint CreateRestorePoint(List<BackupObject> backUpObjects);
}