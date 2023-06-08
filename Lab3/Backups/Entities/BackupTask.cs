using Backups.Algorithms;
using Backups.Models;

namespace Backups.Entities;

public class BackupTask
{
    private readonly AlgorithmStrategyContext _algorithmStrategyContext;
    private readonly List<BackupObject> _objects = new List<BackupObject>();

    public BackupTask(IStorageAlgorithm algorithm, IRepository repository, string backupJobName, IBackup backup)
    {
        ArgumentNullException.ThrowIfNull(algorithm);
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(backup);
        _algorithmStrategyContext =
            new AlgorithmStrategyContext(algorithm);
        Repository = repository;
        BackupJobName = backupJobName;
        Backup = backup;
    }

    public string BackupJobName { get; }
    public IReadOnlyCollection<RestorePoint> RestorePoints => Backup.GetRestorePoints();
    public IReadOnlyCollection<BackupObject> Objects => _objects.AsReadOnly();
    protected IRepository Repository { get; }
    protected IBackup Backup { get; }

    public void AddBackUpObject(BackupObject backupObject)
    {
        ArgumentNullException.ThrowIfNull(backupObject);
        _objects.Add(backupObject);
    }

    public void DeleteBackUpObject(BackupObject backupObject)
    {
        ArgumentNullException.ThrowIfNull(backupObject);
        BackupObject objectToDelete =
            _objects.SingleOrDefault(o => o.PathOfBackupObject == backupObject.PathOfBackupObject) ??
            throw new ArgumentException("object to delete not found");
        _objects.Remove(objectToDelete);
    }

    public void BackUp()
    {
        RestorePoint restorePoint = _algorithmStrategyContext.ExecuteAlgorithm(_objects);
        Repository.StorageRestorePoint(restorePoint, new Archiver());
        Backup.AddRestorePoint(restorePoint);
    }
}