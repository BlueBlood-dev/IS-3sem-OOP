using Backups.Models;

namespace Backups.Algorithms;

public class AlgorithmStrategyContext
{
    private IStorageAlgorithm _algorithm;

    public AlgorithmStrategyContext(IStorageAlgorithm algorithm)
    {
        ArgumentNullException.ThrowIfNull(algorithm);
        _algorithm = algorithm;
    }

    public void SetAlgorithm(IStorageAlgorithm algorithm)
    {
        ArgumentNullException.ThrowIfNull(algorithm);
        _algorithm = algorithm;
    }

    public RestorePoint ExecuteAlgorithm(List<BackupObject> backUpObjects) =>
        _algorithm.CreateRestorePoint(backUpObjects);
}