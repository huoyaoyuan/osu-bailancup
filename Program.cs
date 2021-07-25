using System.IO;
using System.Text.Json;
using osu_bailancup;

Settings settings;
using (var settingFile = File.OpenRead("settings.json"))
    settings = await JsonSerializer.DeserializeAsync<Settings>(settingFile);
var manager = new CupManager(settings);
