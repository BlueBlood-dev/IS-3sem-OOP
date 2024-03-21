using Backups.Models;

namespace Backups.Algorithms;

public class SplitStorage : IStorageAlgorithm
{
    private const int MinimalAmountOfObjectsToArchive = 1;

    public RestorePoint CreateRestorePoint(List<BackupObject> backUpObjects)
    {
        if (CheckIfNullObjectExists(backUpObjects) || backUpObjects.Count < MinimalAmountOfObjectsToArchive)
            throw new ArgumentException("list of objects to backUp must be correct");

        var listOfStorages = new List<Storage>();
        backUpObjects.ForEach(backUpObject =>
            listOfStorages.Add(new Storage(new List<BackupObject> { backUpObject })));
        return new RestorePoint(listOfStorages, DateTime.Now);
    }

    private bool CheckIfNullObjectExists(IEnumerable<BackupObject> backUpObjects) =>
        backUpObjects.Any(o => false);
}