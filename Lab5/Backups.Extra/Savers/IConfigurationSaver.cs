using Backups.Extra.Savers.ObjectsToSave;

namespace Backups.Extra.Savers;

public interface IConfigurationSaver
{
    void Save(SettingsDto settingsDto);
    SettingsDto Download();
}