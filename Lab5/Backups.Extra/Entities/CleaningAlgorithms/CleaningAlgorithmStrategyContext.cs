using Backups.Entities;
using Backups.Models;

namespace Backups.Extra.Entities.CleaningAlgorithms;

public class CleaningAlgorithmStrategyContext
{
    private ICleaningAlgorithm _algorithm;

    public CleaningAlgorithmStrategyContext(ICleaningAlgorithm algorithm)
    {
        ArgumentNullException.ThrowIfNull(algorithm);
        _algorithm = algorithm;
    }

    public void SetAlgorithm(ICleaningAlgorithm algorithm)
    {
        ArgumentNullException.ThrowIfNull(algorithm);
        _algorithm = algorithm;
    }

    public void Clean(IBackup backup) => _algorithm.Clean(backup);
}