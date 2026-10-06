using System.IO;
using System.Text.Json;
using ModVault.Models;

namespace ModVault.Services
{
    public class ConfigService
    {
        private readonly string _configFilePath;

        public ConfigService()
        {
            // Store config file in AppData/Local/ModVault
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string appFolder = Path.Combine(appDataPath, "ModVault");

            Directory.CreateDirectory(appFolder);
            _configFilePath = Path.Combine(appFolder, "config.json");
        }

        public AppConfig LoadConfig()
        {
            if (!File.Exists(_configFilePath))
            {
                return new AppConfig();
            }

            try
            {
                string json = File.ReadAllText(_configFilePath);
                return JsonSerializer.Deserialize<AppConfig>(json) ?? new AppConfig();
            }
            catch
            {
                return new AppConfig();
            }
        }

        public void SaveConfig(AppConfig config)
        {
            string json = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_configFilePath, json);
        }
    }
}