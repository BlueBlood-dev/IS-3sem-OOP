using Backups.Algorithms;
using Backups.Entities;
using Backups.Extra.Entities.Deleters;
using Backups.Models;

namespace Backups.Extra.Entities.CleaningAlgorithms;

public class CleaningWithMerge : ICleaningAlgorithm
{
    private readonly IDeleter _deleter;
    public CleaningWithMerge(IDeleter deleter)
    {
        ArgumentNullException.ThrowIfNull(deleter);
        _deleter = deleter;
    }

    public void Clean(IBackup backup)
    {
        _deleter.Delete(backup.GetRestorePoints().ToList(), backup);
    }
}