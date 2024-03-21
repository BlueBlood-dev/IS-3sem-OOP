using Backups.Exceptions;

namespace Backups.Models;

public class Storage
{
    private const int MinRequiredAmountOfArchivesInStorage = 1;
    private readonly List<BackupObject> _objects;
    public Storage(List<BackupObject> objects)
    {
        if (objects.Count < MinRequiredAmountOfArchivesInStorage)
            throw new StorageZeroArchivesException("amount of archives in storages should not be zero");
        _objects = objects;
        Id = Guid.NewGuid();
    }

    public Guid Id { get; }
    public IReadOnlyCollection<BackupObject> BackupObjects => _objects.AsReadOnly();
}