using System.IO.Compression;
using Backups.Algorithms;
using Backups.Entities;
using Backups.Extra.Entities.CleaningAlgorithms;
using Backups.Extra.Entities.Logger;
using Backups.Models;

namespace Backups.Extra.Entities;

public class BackupTaskExtra : BackupTask
{
    private readonly ILogger _logger;
    private readonly CleaningAlgorithmStrategyContext _cleaningAlgorithm;

    public BackupTaskExtra(
        IStorageAlgorithm algorithm,
        IRepository repository,
        string backupJobName,
        IBackup backup,
        ILogger logger,
        CleaningAlgorithmStrategyContext cleaningAlgorithm)
        : base(algorithm, repository, backupJobName, backup)
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(cleaningAlgorithm);
        _logger = logger;
        _cleaningAlgorithm = cleaningAlgorithm;
    }

    public void BackupExtra()
    {
        BackUp();
        _logger.CreateLog("restore point was created and stored to repository");
        _cleaningAlgorithm.Clean(Backup);
    }

    public void AddBackupObjectExtra(BackupObject backupObject)
    {
        AddBackUpObject(backupObject);
        _logger.CreateLog("Backup object was created");
    }

    public void DeleteBackupObjectExtra(BackupObject backupObject)
    {
        AddBackUpObject(backupObject);
        _logger.CreateLog("Backup object was removed");
    }

    public void RestoreFromPoint(RestorePoint restorePoint, string restoringPath)
    {
        ArgumentNullException.ThrowIfNull(restorePoint);
        string repositoryPath = Repository.RepositoryPath;

        foreach (Storage storage in restorePoint.Storages)
        {
            foreach (BackupObject storageBackupObject in storage.BackupObjects)
            {
                using ZipArchive zipArchive = ZipFile.OpenRead(Path.Combine(Path.Combine(repositoryPath, storage.Id.ToString()), Path.GetFileName(storageBackupObject.PathOfBackupObject) + ".zip"));
                string? path;
                if (string.IsNullOrWhiteSpace(restoringPath))
                {
                    if (File.Exists(storageBackupObject.PathOfBackupObject))
                        File.Delete(storageBackupObject.PathOfBackupObject);
                    path = storageBackupObject.PathOfBackupObject;
                }
                else
                {
                     path = Path.Combine(restoringPath, Path.GetFileName(storageBackupObject.PathOfBackupObject));
                }

                zipArchive
                    .Entries
                    .FirstOrDefault(entry => entry.Name == Path.GetFileName(storageBackupObject.PathOfBackupObject))?
                    .ExtractToFile(path);
                _logger.CreateLog("successfully restored!");
            }
        }
    }
}