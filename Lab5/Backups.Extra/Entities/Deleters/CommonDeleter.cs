using Backups.Entities;
using Backups.Extra.Entities.CleaningAlgorithms;
using Backups.Models;

namespace Backups.Extra.Entities.Deleters;

public class CommonDeleter : IDeleter
{
    public void Delete(List<RestorePoint> restorePointsToDelete, IBackup backup) =>
        restorePointsToDelete.ForEach(backup.RemoveRestorePoint);
}