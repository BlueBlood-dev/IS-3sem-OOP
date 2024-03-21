using Backups.Models;

namespace Backups.Entities;

public interface IBackup
{
    void AddRestorePoint(RestorePoint restorePoint);
    void RemoveRestorePoint(RestorePoint restorePoint);

    IReadOnlyCollection<RestorePoint> GetRestorePoints();
}