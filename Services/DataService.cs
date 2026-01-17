using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using TaskMateApp.Models;
using TaskMateApp.Utils;

namespace TaskMateApp.Services
{
    public class DataService
    {
        private readonly string _dataFilePath;

        public DataService()
        {
            var appDataPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                Constants.AppName
            );
            Directory.CreateDirectory(appDataPath);
            _dataFilePath = Path.Combine(appDataPath, Constants.DataFileName);
        }

        public async Task<List<TodoItem>> LoadTasksAsync()
        {
            try
            {
                if (!File.Exists(_dataFilePath))
                    return new List<TodoItem>();

                var json = await File.ReadAllTextAsync(_dataFilePath);
                if (string.IsNullOrWhiteSpace(json))
                    return new List<TodoItem>();

                var tasks = JsonSerializer.Deserialize<List<TodoItem>>(json);
                return tasks ?? new List<TodoItem>();
            }
            catch
            {
                return new List<TodoItem>();
            }
        }

        public async Task SaveTasksAsync(List<TodoItem> tasks)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };
                var json = JsonSerializer.Serialize(tasks, options);
                await File.WriteAllTextAsync(_dataFilePath, json);
            }
            catch
            {
            }
        }
    }
}
