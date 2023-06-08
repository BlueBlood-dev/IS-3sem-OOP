using Backups.Extra.Entities;

namespace Backups.Extra.Savers.ObjectsToSave;

public class SettingsDto
{
    private readonly List<BackupTaskExtra> _backupTaskExtras;

    public SettingsDto(List<BackupTaskExtra> backupTaskExtras)
    {
        ArgumentNullException.ThrowIfNull(backupTaskExtras);
        _backupTaskExtras = backupTaskExtras;
    }
}