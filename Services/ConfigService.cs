using System;
using System.IO;
using System.Text.Json;
using TaskMateApp.Utils;

namespace TaskMateApp.Services
{
    public class ConfigService
    {
        private readonly string _configFilePath;
        private ConfigData _config;

        public ConfigService()
        {
            var appDataPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                Constants.AppName
            );
            Directory.CreateDirectory(appDataPath);
            _configFilePath = Path.Combine(appDataPath, Constants.ConfigFileName);
            _config = LoadConfig();
        }

        private ConfigData LoadConfig()
        {
            try
            {
                if (!File.Exists(_configFilePath))
                    return new ConfigData();

                var json = File.ReadAllText(_configFilePath);
                if (string.IsNullOrWhiteSpace(json))
                    return new ConfigData();

                return JsonSerializer.Deserialize<ConfigData>(json) ?? new ConfigData();
            }
            catch
            {
                return new ConfigData();
            }
        }

        public void SaveConfig()
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };
                var json = JsonSerializer.Serialize(_config, options);
                File.WriteAllText(_configFilePath, json);
            }
            catch
            {
            }
        }

        public bool WindowMaximized
        {
            get => _config.WindowMaximized;
            set
            {
                _config.WindowMaximized = value;
                SaveConfig();
            }
        }

        private class ConfigData
        {
            public bool WindowMaximized { get; set; } = true;
        }
    }
}
