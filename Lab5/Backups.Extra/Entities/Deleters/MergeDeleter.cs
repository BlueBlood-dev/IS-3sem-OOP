using Backups.Algorithms;
using Backups.Entities;
using Backups.Extra.Entities.CleaningAlgorithms;
using Backups.Models;

namespace Backups.Extra.Entities.Deleters;

public class MergeDeleter : IDeleter
{
    private const int SingleStorageAlgorithmCondition = 1;
    private readonly IStorageAlgorithm _storageAlgorithm;

    public MergeDeleter(IStorageAlgorithm storageAlgorithm)
    {
        ArgumentNullException.ThrowIfNull(storageAlgorithm);
        _storageAlgorithm = storageAlgorithm;
    }

    public void Delete(List<RestorePoint> restorePointsToDelete, IBackup backup)
    {
        for (int i = 0; i < restorePointsToDelete.Count - 1; i++)
        {
            if (_storageAlgorithm is SingleStorage && restorePointsToDelete[i].Storages.Count == SingleStorageAlgorithmCondition)
            {
                backup.RemoveRestorePoint(restorePointsToDelete[i]);
                continue;
            }

            var storagesToAdd = restorePointsToDelete[i].Storages.Where(s => !restorePointsToDelete[i + 1].Storages.Contains(s)).ToList();
            storagesToAdd.ForEach(restorePointsToDelete[i + 1].AddStorage);
            backup.RemoveRestorePoint(restorePointsToDelete[i]);
        }
    }
}