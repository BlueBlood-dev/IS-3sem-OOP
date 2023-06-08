using Backups.Entities;
using Backups.Extra.Entities.Deleters;

namespace Backups.Extra.Entities.CleaningAlgorithms;

public class CleaningWithMultipleConditions : ICleaningAlgorithm
{
    private readonly IDeleter _deleter;
    public CleaningWithMultipleConditions(bool bothConditions, DateTime date, int limit, IDeleter deleter)
    {
        BothConditions = bothConditions;
        ArgumentNullException.ThrowIfNull(date);
        ArgumentNullException.ThrowIfNull(deleter);
        if (limit < 0)
            throw new ArgumentException("limit should be greater than zero");
        Date = date;
        Limit = limit;
        _deleter = deleter;
    }

    public bool BothConditions { get; }
    public DateTime Date { get; }
    public int Limit { get; }

    public void Clean(IBackup backup)
    {
        if (BothConditions)
        {
            var restorePointsToDelete = backup
                .GetRestorePoints()
                .Take(Limit)
                .Where(r => r.CreationTime > Date)
                .ToList();
            _deleter.Delete(restorePointsToDelete, backup);
        }
        else
        {
            var restorePointsToDeleteByLimit = backup
                .GetRestorePoints()
                .Take(Limit)
                .ToList();
            var restorePointsToDeleteByDate = backup
                .GetRestorePoints()
                .TakeLast(backup.GetRestorePoints().Count - Limit)
                .Where(r => r.CreationTime > Date)
                .ToList();
            _deleter.Delete(restorePointsToDeleteByDate, backup);
            _deleter.Delete(restorePointsToDeleteByLimit, backup);
        }
    }
}