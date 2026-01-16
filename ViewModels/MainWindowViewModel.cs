using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows.Input;
using TodoApp.Models;
using TodoApp.Services;
using TodoApp.Common;

namespace TodoApp.ViewModels;

public enum FilterType
{
    All,
    Pending,
    Completed
}

public class MainWindowViewModel : ViewModelBase
{
    private readonly ConfigService _configService;
    private readonly NotificationService _notificationService;
    private string _statusText = "就緒";
    private string _searchText = string.Empty;
    private FilterType _filterType = FilterType.All;
    private Timer? _autoSaveTimer;
    private Timer? _searchDebounceTimer;
    private bool _isDirty = false;
    
    private int? _cachedTotalCount;
    private int? _cachedCompletedCount;
    private int? _cachedPendingCount;
    private bool? _cachedHasTodos;

    public MainWindowViewModel(ConfigService? configService = null, NotificationService? notificationService = null)
    {
        _configService = configService ?? new ConfigService();
        _notificationService = notificationService ?? new NotificationService();
        Todos = new ObservableCollection<TodoItem>();
        FilteredTodos = new ObservableCollection<TodoItem>();
        
        LoadTodos();
        
        DeleteCommand = new RelayCommand<TodoItem>((item) => DeleteTodo(item));
        ToggleCompleteCommand = new RelayCommand<TodoItem>((item) => ToggleComplete(item));
        DeleteAllCommand = new RelayCommand(() => DeleteAllTodos(), () => HasTodos);
        EditCommand = new RelayCommand<TodoItem>((item) => EditTodo(item));
        
        _autoSaveTimer = new Timer(AutoSaveCallback, null, Timeout.Infinite, 2000);
        _searchDebounceTimer = new Timer(SearchDebounceCallback, null, Timeout.Infinite, 300);
        
        Todos.CollectionChanged += (s, e) => InvalidateCache();
    }

    public ObservableCollection<TodoItem> Todos { get; }
    public ObservableCollection<TodoItem> FilteredTodos { get; }

    public string StatusText
    {
        get => _statusText;
        set
        {
            _statusText = value;
            OnPropertyChanged();
        }
    }

    public string SearchText
    {
        get => _searchText;
        set
        {
            _searchText = value;
            OnPropertyChanged();
            if (_searchDebounceTimer != null)
            {
                _searchDebounceTimer.Change(300, Timeout.Infinite);
            }
        }
    }

    public FilterType FilterType
    {
        get => _filterType;
        set
        {
            _filterType = value;
            OnPropertyChanged();
            ApplyFilter();
        }
    }

    public int TotalCount
    {
        get
        {
            if (!_cachedTotalCount.HasValue)
            {
                _cachedTotalCount = Todos.Count;
            }
            return _cachedTotalCount.Value;
        }
    }

    public int CompletedCount
    {
        get
        {
            if (!_cachedCompletedCount.HasValue)
            {
                _cachedCompletedCount = Todos.Count(t => t.IsCompleted);
            }
            return _cachedCompletedCount.Value;
        }
    }

    public int PendingCount
    {
        get
        {
            if (!_cachedPendingCount.HasValue)
            {
                _cachedPendingCount = Todos.Count(t => !t.IsCompleted);
            }
            return _cachedPendingCount.Value;
        }
    }

    public bool HasTodos
    {
        get
        {
            if (!_cachedHasTodos.HasValue)
            {
                _cachedHasTodos = Todos.Count > 0;
            }
            return _cachedHasTodos.Value;
        }
    }

    public int FilteredCount => FilteredTodos.Count;

    public ICommand DeleteCommand { get; }
    public ICommand ToggleCompleteCommand { get; }
    public ICommand DeleteAllCommand { get; }
    public ICommand EditCommand { get; }

    private void InvalidateCache()
    {
        _cachedTotalCount = null;
        _cachedCompletedCount = null;
        _cachedPendingCount = null;
        _cachedHasTodos = null;
    }

    private void SearchDebounceCallback(object? state)
    {
        Avalonia.Threading.Dispatcher.UIThread.Post(() => ApplyFilter(), Avalonia.Threading.DispatcherPriority.Normal);
    }

    private void LoadTodos()
    {
        try
        {
            var todos = _configService.LoadTodos();
            Todos.Clear();
            foreach (var todo in todos.OrderByDescending(t => t.CreatedAt))
            {
                Todos.Add(todo);
            }
            ApplyFilter();
            UpdateStatus();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"載入待辦事項失敗: {ex.Message}");
            _notificationService?.ShowError("載入待辦事項失敗");
        }
    }

    public void SaveTodos()
    {
        try
        {
            _configService.SaveTodos(Todos.ToList());
            _isDirty = false;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"保存待辦事項失敗: {ex.Message}");
            _notificationService?.ShowError("保存待辦事項失敗");
        }
    }

    public void MarkDirty()
    {
        _isDirty = true;
        if (_autoSaveTimer != null)
        {
            _autoSaveTimer.Change(2000, Timeout.Infinite);
        }
    }

    private void AutoSaveCallback(object? state)
    {
        if (_isDirty)
        {
            SaveTodos();
        }
    }

    public void ApplyFilter()
    {
        var query = Todos.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            var searchLower = SearchText.ToLowerInvariant();
            query = query.Where(t => t.Title.ToLowerInvariant().Contains(searchLower) ||
                                   (t.Tags != null && t.Tags.Any(tag => tag.ToLowerInvariant().Contains(searchLower))));
        }

        query = FilterType switch
        {
            FilterType.Pending => query.Where(t => !t.IsCompleted),
            FilterType.Completed => query.Where(t => t.IsCompleted),
            _ => query
        };

        var newFilteredItems = query.OrderByDescending(t => t.CreatedAt).ToList();
        
        var itemsToRemove = FilteredTodos.Where(item => !newFilteredItems.Any(newItem => newItem.Id == item.Id)).ToList();
        var itemsToAdd = newFilteredItems.Where(newItem => !FilteredTodos.Any(item => item.Id == newItem.Id)).ToList();
        var itemsToUpdate = FilteredTodos.Where(item => newFilteredItems.Any(newItem => newItem.Id == item.Id && 
            (newItem.Title != item.Title || newItem.IsCompleted != item.IsCompleted))).ToList();

        foreach (var item in itemsToRemove)
        {
            FilteredTodos.Remove(item);
        }

        foreach (var newItem in itemsToAdd)
        {
            var insertIndex = newFilteredItems.IndexOf(newItem);
            if (insertIndex < FilteredTodos.Count)
            {
                FilteredTodos.Insert(insertIndex, newItem);
            }
            else
            {
                FilteredTodos.Add(newItem);
            }
        }

        foreach (var item in itemsToUpdate)
        {
            var newItem = newFilteredItems.First(n => n.Id == item.Id);
            var index = FilteredTodos.IndexOf(item);
            FilteredTodos[index] = newItem;
        }
        
        OnPropertyChanged(nameof(FilteredCount));
    }

    public void UpdateStatus()
    {
        InvalidateCache();
        StatusText = $"總計: {TotalCount} | 已完成: {CompletedCount} | 待完成: {PendingCount}";
        OnPropertyChanged(nameof(TotalCount));
        OnPropertyChanged(nameof(CompletedCount));
        OnPropertyChanged(nameof(PendingCount));
        OnPropertyChanged(nameof(HasTodos));
        if (DeleteAllCommand is RelayCommand cmd)
        {
            cmd.RaiseCanExecuteChanged();
        }
    }

    private void DeleteTodo(TodoItem? item)
    {
        if (item == null)
        {
            return;
        }

        Todos.Remove(item);
        ApplyFilter();
        MarkDirty();
        UpdateStatus();
        _notificationService?.ShowSuccess("待辦事項已刪除");
    }

    private void ToggleComplete(TodoItem? item)
    {
        if (item == null)
        {
            return;
        }

        item.IsCompleted = !item.IsCompleted;
        item.CompletedAt = item.IsCompleted ? DateTime.Now : null;
        ApplyFilter();
        MarkDirty();
        UpdateStatus();
    }

    public void EditTodo(TodoItem? item)
    {
        if (item == null)
        {
            return;
        }

        EditTodoRequested?.Invoke(item);
    }

    public void UpdateTodo(TodoItem item, string newTitle, List<string>? tags = null)
    {
        if (string.IsNullOrWhiteSpace(newTitle))
        {
            return;
        }

        item.Title = newTitle.Trim();
        if (tags != null)
        {
            item.Tags = tags;
        }
        ApplyFilter();
        MarkDirty();
        _notificationService?.ShowSuccess("待辦事項已更新");
    }

    public event Action<TodoItem>? EditTodoRequested;

    public void DeleteAllTodos()
    {
        if (Todos.Count == 0)
        {
            return;
        }

        Todos.Clear();
        ApplyFilter();
        MarkDirty();
        UpdateStatus();
        _notificationService?.ShowSuccess("所有待辦事項已刪除");
    }

    public void Dispose()
    {
        _autoSaveTimer?.Dispose();
        _searchDebounceTimer?.Dispose();
    }
}
