using Backups.Models;

namespace Backups.Algorithms;

public class SingleStorage : IStorageAlgorithm
{
    private const int MinimalAmountOfObjectsToArchive = 1;

    public RestorePoint CreateRestorePoint(List<BackupObject> backUpObjects)
    {
        if (CheckIfNullObjectExists(backUpObjects) || backUpObjects.Count < MinimalAmountOfObjectsToArchive)
            throw new ArgumentException("list of objects to backUp must be correct");
        var singleStorageList = new List<Storage>();
        singleStorageList.Add(new Storage(backUpObjects));
        return new RestorePoint(singleStorageList, DateTime.Now);
    }

    private bool CheckIfNullObjectExists(IEnumerable<BackupObject> backUpObjects) =>
        backUpObjects.Any(o => false);
}