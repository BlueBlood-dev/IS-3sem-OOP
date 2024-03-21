using Backups.Extra.Entities;
using Backups.Extra.Entities.Logger;
using Backups.Extra.Exceptions;
using Backups.Extra.Savers.ObjectsToSave;
using Newtonsoft.Json;

namespace Backups.Extra.Savers;

public class Saver : IConfigurationSaver
{
    private const string Name = "Tasks.json";
    private readonly JsonSerializerSettings _settings;
    private readonly ILogger _logger;
    private readonly string _path;
    public Saver(string path, ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(logger);
        _settings = new JsonSerializerSettings();
        _settings.Formatting = Formatting.Indented;
        _settings.TypeNameHandling = TypeNameHandling.Auto;
        if (string.IsNullOrWhiteSpace(path))
            throw new ArgumentException("path to json mustn't be null");
        _path = path;
        _logger = logger;
    }

    public void Save(SettingsDto settingsDto)
    {
        string data = JsonConvert.SerializeObject(settingsDto, _settings);
        string filePath = Path.Combine(_path, Name);
        File.WriteAllText(filePath, data);
        _logger.CreateLog("Backup task was saved");
    }

    public SettingsDto Download()
    {
        string filePath = Path.Combine(_path, Name);
        SettingsDto? settings = JsonConvert.DeserializeObject<SettingsDto>(File.ReadAllText(filePath), _settings) ??
                             throw new BackupExtraException("couldn't download backup tasks settings");
        _logger.CreateLog("settings were downloaded");
        return settings;
    }
}