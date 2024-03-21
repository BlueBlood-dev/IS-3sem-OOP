using Backups.Entities;
using Backups.Extra.Entities.Deleters;

namespace Backups.Extra.Entities.CleaningAlgorithms;

public class CleaningWithAmountLimit : ICleaningAlgorithm
{
    private const int MinAllowedLimit = 1;
    private readonly IDeleter _deleter;
    public CleaningWithAmountLimit(int limit, IDeleter deleter)
    {
        ArgumentNullException.ThrowIfNull(deleter);
        if (limit < MinAllowedLimit)
            throw new ArgumentException("limit must be greater than zero");
        Limit = limit;
        _deleter = deleter;
    }

    public int Limit { get; }
    public void Clean(IBackup backup)
    {
        var restorePointsToDelete = backup.GetRestorePoints().Take(Limit).ToList();
        _deleter.Delete(restorePointsToDelete, backup);
        restorePointsToDelete.ForEach(backup.RemoveRestorePoint);
    }
}