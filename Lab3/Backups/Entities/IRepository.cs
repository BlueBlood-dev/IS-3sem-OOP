using Backups.Models;

namespace Backups.Entities;

public interface IRepository
{
    string RepositoryPath { get; }
    void StorageRestorePoint(RestorePoint restorePoint, Archiver archiver);
}