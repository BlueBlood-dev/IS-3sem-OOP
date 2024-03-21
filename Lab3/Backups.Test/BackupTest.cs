using Backups.Algorithms;
using Backups.Entities;
using Backups.Models;
using Xunit;

namespace Backups.Test;

public class BackupTest
{
    [Fact]
    public void BackUpTaskCreated_TwoStoragesAnd1RestorePointCreated()
    {
        var repository = new LocalRepository(@"/home/runner/work/BlueBlood-dev/Repo");
        var job = new BackupTask(new SplitStorage(), repository, "BackUpJob Name", new Backup());

        const string file1 = @"/bin/ssh";
        const string file2 = @"/bin/apt";
        const string file3 = @"/bin/size";
        var obj1 = new BackupObject(file1);
        var obj2 = new BackupObject(file2);
        var obj3 = new BackupObject(file3);
        job.AddBackUpObject(obj1);
        job.AddBackUpObject(obj2);
        job.AddBackUpObject(obj3);
        job.DeleteBackUpObject(obj2);
        job.BackUp();
        var storagesAmount = job.RestorePoints.ToArray()[0].Storages.Count;
        Assert.Equal(2, storagesAmount);
        Assert.Equal(1, job.RestorePoints.Count);
    }

    [Fact]
    public void BackUpTaskCreated_AllFilesAndDirectoriesWereCreated()
    {
        var repository = new LocalRepository(@"/home/runner/work/BlueBlood-dev/Repo1");
        var job = new BackupTask(new SplitStorage(), repository, "BackUpJob Name", new Backup());

        // linux roots
        const string file1 = @"/bin/ssh";
        const string file2 = @"/bin/apt";
        const string file3 = @"/bin/size";
        var obj1 = new BackupObject(file1);
        var obj2 = new BackupObject(file2);
        var obj3 = new BackupObject(file3);
        job.AddBackUpObject(obj1);
        job.AddBackUpObject(obj2);
        job.AddBackUpObject(obj3);
        job.BackUp();
        Assert.Equal(3, new DirectoryInfo(@"/home/runner/work/BlueBlood-dev/Repo1").GetDirectories().Length);
        Assert.Single(new DirectoryInfo(@"/home/runner/work/BlueBlood-dev/Repo1").GetDirectories()[0].GetFiles());
        Assert.Single(new DirectoryInfo(@"/home/runner/work/BlueBlood-dev/Repo1").GetDirectories()[1].GetFiles());
        Assert.Single(new DirectoryInfo(@"/home/runner/work/BlueBlood-dev/Repo1").GetDirectories()[2].GetFiles());
    }
}