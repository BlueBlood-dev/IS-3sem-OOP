using Backups.Models;

namespace Backups.Entities;

public class LocalRepository : IRepository
{
    public LocalRepository(string repositoryPath)
    {
        if (string.IsNullOrWhiteSpace(repositoryPath))
            throw new ArgumentException("repository path is invalid");
        RepositoryPath = repositoryPath;
    }

    public string RepositoryPath { get; }

    public void StorageRestorePoint(RestorePoint restorePoint, Archiver archiver)
    {
        ArgumentNullException.ThrowIfNull(restorePoint);
        ArgumentNullException.ThrowIfNull(archiver);
        if (!Directory.Exists(RepositoryPath))
                Directory.CreateDirectory(RepositoryPath);
        restorePoint.Storages.ToList()
            .ForEach(storage => archiver.CreateArchive(RepositoryPath, storage));
    }
}