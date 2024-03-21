using System.IO.Compression;
using Backups.Models;

namespace Backups.Entities;

public class Archiver
{
    public void CreateArchive(string path, Storage storage)
    {
        if (string.IsNullOrWhiteSpace(path))
            throw new ArgumentException("repository path is invalid");
        ArgumentNullException.ThrowIfNull(storage);
        Archive(path, storage);
    }

    private void Archive(string path, Storage storage)
    {
        string storagePath = Path.Combine(path, storage.Id.ToString());
        string name = Directory.CreateDirectory(storagePath).FullName;
        foreach (BackupObject backUpObject in storage.BackupObjects)
        {
            if (!File.Exists(backUpObject.PathOfBackupObject) && !Directory.Exists(backUpObject.PathOfBackupObject))
                throw new ArgumentException("back up object is not a file or directory");
            using ZipArchive zipArchive = ZipFile.Open(
                Path.Combine(name, Path.GetFileName(backUpObject.PathOfBackupObject) + ".zip"),
                ZipArchiveMode.Update);
            zipArchive.CreateEntryFromFile(
                backUpObject.PathOfBackupObject,
                Path.GetFileName(backUpObject.PathOfBackupObject));
        }
    }
}