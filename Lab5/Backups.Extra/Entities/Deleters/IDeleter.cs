using Backups.Entities;
using Backups.Models;

namespace Backups.Extra.Entities.Deleters;

public interface IDeleter
{
    void Delete(List<RestorePoint> restorePointsToDelete, IBackup backup);
}