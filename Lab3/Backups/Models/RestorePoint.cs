using Backups.Exceptions;

namespace Backups.Models;

public class RestorePoint
{
    private const int MinRequiredAmountOfStorages = 1;
    private readonly List<Storage> _listOfStorages;

    public RestorePoint(List<Storage> listOfStorages, DateTime creationTime)
    {
        if (listOfStorages.Count < MinRequiredAmountOfStorages)
            throw new RestorePointZeroStoragesException("there must be at least 1 storage in restore point");
        ArgumentNullException.ThrowIfNull(creationTime);
        _listOfStorages = listOfStorages;
        CreationTime = creationTime;
    }

    public DateTime CreationTime { get; }
    public IReadOnlyCollection<Storage> Storages => _listOfStorages.AsReadOnly();

    public void AddStorage(Storage storage)
    {
        ArgumentNullException.ThrowIfNull(storage);
        _listOfStorages.Add(storage);
    }
}