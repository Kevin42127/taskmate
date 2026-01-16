using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using TodoApp.Models;
using TodoApp.Utils;

namespace TodoApp.Services;

public class ConfigService
{
    private readonly string _configPath;

    public ConfigService()
    {
        var appDataPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "AIChatApp");
        Directory.CreateDirectory(appDataPath);
        _configPath = Path.Combine(appDataPath, Constants.ConfigFileName);
    }

    public AppConfig LoadConfig()
    {
        if (!File.Exists(_configPath))
        {
            return new AppConfig();
        }

        try
        {
            var json = File.ReadAllText(_configPath);
            var config = JsonSerializer.Deserialize<AppConfig>(json);
            if (config == null)
            {
                return new AppConfig();
            }
            
            if (config.Todos != null)
            {
                foreach (var todo in config.Todos)
                {
                    if (todo.Tags == null)
                    {
                        todo.Tags = new List<string>();
                    }
                }
            }
            
            return config;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"載入配置失敗: {ex.Message}");
            return new AppConfig();
        }
    }

    public void SaveConfig(AppConfig config)
    {
        try
        {
            var json = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
            var tempPath = _configPath + ".tmp";
            File.WriteAllText(tempPath, json);
            File.Move(tempPath, _configPath, true);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"保存配置失敗: {ex.Message}");
            throw;
        }
    }

    public List<TodoItem> LoadTodos()
    {
        var config = LoadConfig();
        return config.Todos ?? new List<TodoItem>();
    }

    public void SaveTodos(List<TodoItem> todos)
    {
        try
        {
            var config = LoadConfig();
            config.Todos = todos;
            SaveConfig(config);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"保存待辦事項失敗: {ex.Message}");
            throw;
        }
    }
}

public class AppConfig
{
    public List<TodoItem>? Todos { get; set; }
}

