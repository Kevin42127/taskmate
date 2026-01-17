using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using TaskMateApp.Common;
using TaskMateApp.Models;
using TaskMateApp.Services;
using TaskMateApp.Utils;

namespace TaskMateApp.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly DataService _dataService;
        private string _searchText = string.Empty;
        private string _filterStatus = "全部";
        private string _filterPriority = "全部優先級";

        public ObservableCollection<TodoItem> Tasks { get; } = new();
        public ObservableCollection<TodoItem> FilteredTasks { get; } = new();

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                {
                    ApplyFilters();
                }
            }
        }

        public string FilterStatus
        {
            get => _filterStatus;
            set
            {
                if (SetProperty(ref _filterStatus, value))
                {
                    ApplyFilters();
                }
            }
        }

        public string FilterPriority
        {
            get => _filterPriority;
            set
            {
                if (SetProperty(ref _filterPriority, value))
                {
                    ApplyFilters();
                }
            }
        }

        public ICommand DeleteTaskCommand { get; }
        public ICommand ToggleCompleteCommand { get; }
        public ICommand CompleteAllCommand { get; }
        public ICommand DeleteAllCommand { get; }

        public MainWindowViewModel()
        {
            _dataService = new DataService();
            DeleteTaskCommand = new RelayCommand<TodoItem>(DeleteTask);
            ToggleCompleteCommand = new RelayCommand<TodoItem>(ToggleComplete);
            CompleteAllCommand = new RelayCommand(CompleteAll, () => Tasks.Any(t => !t.IsCompleted));
            DeleteAllCommand = new RelayCommand(DeleteAll, () => Tasks.Count > 0);

            Tasks.CollectionChanged += Tasks_CollectionChanged;

            _ = LoadTasksAsync();
        }

        private async Task LoadTasksAsync()
        {
            var tasks = await _dataService.LoadTasksAsync();
            foreach (var task in tasks)
            {
                if (string.IsNullOrEmpty(task.BorderColor))
                {
                    task.BorderColor = GetRandomBorderColor();
                }
                task.PropertyChanged += Task_PropertyChanged;
                Tasks.Add(task);
            }
            ApplyFilters();
            ((RelayCommand)CompleteAllCommand).RaiseCanExecuteChanged();
            ((RelayCommand)DeleteAllCommand).RaiseCanExecuteChanged();
        }

        public void AddTask(string title, string description, Priority priority = Priority.Medium)
        {
            if (string.IsNullOrWhiteSpace(title))
                return;

            var newTask = new TodoItem
            {
                Title = title.Trim(),
                Description = description.Trim(),
                Priority = priority,
                BorderColor = GetRandomBorderColor()
            };

            newTask.PropertyChanged += Task_PropertyChanged;
            Tasks.Add(newTask);
            ((RelayCommand)CompleteAllCommand).RaiseCanExecuteChanged();
            ((RelayCommand)DeleteAllCommand).RaiseCanExecuteChanged();
            _ = SaveTasksAsync();
        }

        public void EditTask(string taskId, string title, string description, Priority priority)
        {
            var task = Tasks.FirstOrDefault(t => t.Id == taskId);
            if (task == null) return;

            task.Title = title.Trim();
            task.Description = description.Trim();
            task.Priority = priority;

            ((RelayCommand)CompleteAllCommand).RaiseCanExecuteChanged();
            _ = SaveTasksAsync();
        }

        private void DeleteTask(TodoItem? task)
        {
            if (task == null) return;

            task.PropertyChanged -= Task_PropertyChanged;
            Tasks.Remove(task);
            ((RelayCommand)CompleteAllCommand).RaiseCanExecuteChanged();
            ((RelayCommand)DeleteAllCommand).RaiseCanExecuteChanged();
            _ = SaveTasksAsync();
        }

        private void ToggleComplete(TodoItem? task)
        {
            if (task == null) return;

            task.IsCompleted = !task.IsCompleted;
            task.CompletedAt = task.IsCompleted ? DateTime.Now : null;

            ((RelayCommand)CompleteAllCommand).RaiseCanExecuteChanged();
            _ = SaveTasksAsync();
        }

        private string GetRandomBorderColor()
        {
            var random = new Random();
            return Constants.CardBorderColors[random.Next(Constants.CardBorderColors.Length)];
        }

        private void CompleteAll()
        {
            var incompleteTasks = Tasks.Where(t => !t.IsCompleted).ToList();
            foreach (var task in incompleteTasks)
            {
                task.IsCompleted = true;
                task.CompletedAt = DateTime.Now;
            }

            if (incompleteTasks.Any())
            {
                ((RelayCommand)CompleteAllCommand).RaiseCanExecuteChanged();
                ((RelayCommand)DeleteAllCommand).RaiseCanExecuteChanged();
                _ = SaveTasksAsync();
            }
        }

        private void DeleteAll()
        {
            foreach (var task in Tasks)
            {
                task.PropertyChanged -= Task_PropertyChanged;
            }
            Tasks.Clear();
            ((RelayCommand)CompleteAllCommand).RaiseCanExecuteChanged();
            ((RelayCommand)DeleteAllCommand).RaiseCanExecuteChanged();
            _ = SaveTasksAsync();
        }

        private void Tasks_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (TodoItem task in e.NewItems)
                {
                    task.PropertyChanged += Task_PropertyChanged;
                }
            }
            if (e.OldItems != null)
            {
                foreach (TodoItem task in e.OldItems)
                {
                    task.PropertyChanged -= Task_PropertyChanged;
                }
            }
            ApplyFilters();
        }

        private void Task_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(TodoItem.Title) || 
                e.PropertyName == nameof(TodoItem.Description) ||
                e.PropertyName == nameof(TodoItem.IsCompleted) ||
                e.PropertyName == nameof(TodoItem.Priority))
            {
                ApplyFilters();
            }
        }

        private void ApplyFilters()
        {
            FilteredTasks.Clear();

            var filtered = Tasks.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                var searchLower = SearchText.ToLowerInvariant();
                filtered = filtered.Where(t => 
                    t.Title.ToLowerInvariant().Contains(searchLower) ||
                    t.Description.ToLowerInvariant().Contains(searchLower));
            }

            if (FilterStatus != "全部")
            {
                filtered = FilterStatus switch
                {
                    "未完成" => filtered.Where(t => !t.IsCompleted),
                    "已完成" => filtered.Where(t => t.IsCompleted),
                    _ => filtered
                };
            }

            if (FilterPriority != "全部優先級")
            {
                var priority = FilterPriority switch
                {
                    "高" => Priority.High,
                    "中" => Priority.Medium,
                    "低" => Priority.Low,
                    _ => (Priority?)null
                };

                if (priority.HasValue)
                {
                    filtered = filtered.Where(t => t.Priority == priority.Value);
                }
            }

            foreach (var task in filtered)
            {
                FilteredTasks.Add(task);
            }
        }

        private async Task SaveTasksAsync()
        {
            await _dataService.SaveTasksAsync(Tasks.ToList());
        }
    }
}
