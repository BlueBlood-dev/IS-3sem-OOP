using Backups.Entities;
using Backups.Models;

namespace Backups.Extra.Entities.CleaningAlgorithms;

public interface ICleaningAlgorithm
{
    void Clean(IBackup backup);
}