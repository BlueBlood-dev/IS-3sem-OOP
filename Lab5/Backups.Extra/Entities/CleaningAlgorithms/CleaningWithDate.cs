using Backups.Entities;
using Backups.Extra.Entities.Deleters;
using Backups.Models;

namespace Backups.Extra.Entities.CleaningAlgorithms;

public class CleaningWithDate : ICleaningAlgorithm
{
    private readonly IDeleter _deleter;
    public CleaningWithDate(DateTime date, IDeleter deleter)
    {
        ArgumentNullException.ThrowIfNull(deleter);
        ArgumentNullException.ThrowIfNull(date);
        Date = date;
        _deleter = deleter;
    }

    public DateTime Date { get; }
    public void Clean(IBackup backup)
    {
        var restorePointsToDelete = backup.GetRestorePoints().Where(r => r.CreationTime > Date).ToList();
        _deleter.Delete(restorePointsToDelete, backup);
    }
}