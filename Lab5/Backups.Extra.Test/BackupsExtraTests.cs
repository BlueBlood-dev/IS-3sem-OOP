using Backups.Algorithms;
using Backups.Entities;
using Backups.Extra.Entities;
using Backups.Extra.Entities.CleaningAlgorithms;
using Backups.Extra.Entities.Deleters;
using Backups.Extra.Entities.Logger;
using Backups.Models;
using Xunit;

namespace Backups.Extra.Test;

public class BackupsExtraTests
{
    [Fact]
    public void CleaningWithAmountLimit_ZeroPointsRemaining()
    {
        var strategy =
            new CleaningAlgorithmStrategyContext(new CleaningWithDate(DateTime.Now.AddDays(90), new CommonDeleter()));
        var task = new BackupTaskExtra(
            new SplitStorage(),
            new LocalRepository(@"/home/runner/work/BlueBlood-dev/Repo"),
            "job",
            new Backup(),
            new ConsoleLogger(),
            strategy);
        var backupObject = new BackupObject(@"/bin/ssh");
        var backupObject1 = new BackupObject(@"/bin/size");
        var backupObject2 = new BackupObject(@"/bin/apt");
        task.AddBackupObjectExtra(backupObject);
        task.AddBackupObjectExtra(backupObject1);
        task.AddBackupObjectExtra(backupObject2);
        task.BackupExtra();
        Assert.Equal(1, task.RestorePoints.Count);
    }

    [Fact]
    public void CleaningWithDateTime_ZeroRestorePointsDeleted()
    {
        var strategy =
            new CleaningAlgorithmStrategyContext(new CleaningWithAmountLimit(5, new CommonDeleter()));
        var task = new BackupTaskExtra(
            new SplitStorage(),
            new LocalRepository(@"/home/runner/work/BlueBlood-dev/Repo"),
            "job",
            new Backup(),
            new ConsoleLogger(),
            strategy);
        var backupObject = new BackupObject(@"/bin/ssh");
        var backupObject1 = new BackupObject(@"/bin/size");
        var backupObject2 = new BackupObject(@"/bin/apt");
        task.AddBackupObjectExtra(backupObject);
        task.AddBackupObjectExtra(backupObject1);
        task.AddBackupObjectExtra(backupObject2);
        task.BackupExtra();
        Assert.Equal(0, task.RestorePoints.Count);
    }

    [Fact]
    public void CleaningWithMultipleConditions_ZeroRestorePointsRemaining()
    {
        var strategy =
            new CleaningAlgorithmStrategyContext(
                new CleaningWithMultipleConditions(false, DateTime.Now.AddDays(23), 9, new CommonDeleter()));
        var task = new BackupTaskExtra(
            new SplitStorage(),
            new LocalRepository(@"/home/runner/work/BlueBlood-dev/Repo"),
            "job",
            new Backup(),
            new ConsoleLogger(),
            strategy);
        var backupObject = new BackupObject(@"/bin/ssh");
        var backupObject1 = new BackupObject(@"/bin/size");
        var backupObject2 = new BackupObject(@"/bin/apt");
        task.AddBackupObjectExtra(backupObject);
        task.AddBackupObjectExtra(backupObject1);
        task.AddBackupObjectExtra(backupObject2);
        task.BackupExtra();
        Assert.Equal(0, task.RestorePoints.Count);
    }

    [Fact]
    public void CleaningWithMultipleConditions_OneRestorePointsRemaining()
    {
        var strategy =
            new CleaningAlgorithmStrategyContext(
                new CleaningWithMultipleConditions(true, DateTime.Now.AddDays(23), 9, new CommonDeleter()));
        var task = new BackupTaskExtra(
            new SplitStorage(),
            new LocalRepository(@"/home/runner/work/BlueBlood-dev/Repo"),
            "job",
            new Backup(),
            new ConsoleLogger(),
            strategy);
        var backupObject = new BackupObject(@"/bin/ssh");
        var backupObject1 = new BackupObject(@"/bin/size");
        var backupObject2 = new BackupObject(@"/bin/apt");
        task.AddBackupObjectExtra(backupObject);
        task.AddBackupObjectExtra(backupObject1);
        task.AddBackupObjectExtra(backupObject2);
        task.BackupExtra();
        Assert.Equal(1, task.RestorePoints.Count);
    }

    [Fact]
    public void Restore_Restored()
    {
        Directory.CreateDirectory(@"/home/runner/work/BlueBlood-dev/restored");
        string restoringPath = @"/home/runner/work/BlueBlood-dev/restored";
        var strategy =
            new CleaningAlgorithmStrategyContext(new CleaningWithDate(DateTime.Now.AddDays(90), new CommonDeleter()));
        var task = new BackupTaskExtra(
            new SplitStorage(),
            new LocalRepository(@"/home/runner/work/BlueBlood-dev/Repo"),
            "job",
            new Backup(),
            new ConsoleLogger(),
            strategy);
        var backupObject1 = new BackupObject(@"/bin/size");
        var backupObject = new BackupObject(@"/bin/ls");
        var backupObject2 = new BackupObject(@"/bin/apt");
        task.AddBackupObjectExtra(backupObject1);
        task.AddBackupObjectExtra(backupObject);
        task.AddBackupObjectExtra(backupObject2);
        task.BackupExtra();
        RestorePoint restorePoint = task.RestorePoints.ToList()[0];
        task.RestoreFromPoint(restorePoint, restoringPath);
        FileInfo[] files = new DirectoryInfo(restoringPath).GetFiles();
        Assert.Equal(3, files.Length);
        foreach (FileInfo file in files)
        {
            File.Delete(file.FullName);
        }
    }
}