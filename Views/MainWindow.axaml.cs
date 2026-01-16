using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using TodoApp.Models;
using TodoApp.Services;
using TodoApp.ViewModels;

namespace TodoApp.Views;

public partial class MainWindow : Window
{
    private readonly NotificationService _notificationService;

    public MainWindow()
    {
        InitializeComponent();
        _notificationService = new NotificationService();
        _notificationService.Initialize(this);
    }

    private void InitializeComponent()
    {
        Avalonia.Markup.Xaml.AvaloniaXamlLoader.Load(this);
        var vm = new MainWindowViewModel();
        DataContext = vm;
        
        vm.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(MainWindowViewModel.FilterType))
            {
                UpdateFilterButtons();
            }
        };
        
        this.Opened += (s, e) => UpdateFilterButtons();
        
        var searchTextBox = this.FindControl<TextBox>("SearchTextBox");
        if (searchTextBox != null)
        {
            System.Action updateWatermark = () =>
            {
                if (!string.IsNullOrWhiteSpace(searchTextBox.Text))
                {
                    searchTextBox.Watermark = null;
                }
                else
                {
                    searchTextBox.Watermark = "搜尋待辦事項...";
                }
            };

            searchTextBox.GotFocus += (s, e) =>
            {
                searchTextBox.Watermark = null;
            };

            searchTextBox.TextInput += (s, e) =>
            {
                searchTextBox.Watermark = null;
            };

            searchTextBox.KeyDown += (s, e) =>
            {
                if (e.Key != Avalonia.Input.Key.Enter && 
                    e.Key != Avalonia.Input.Key.Tab && 
                    e.Key != Avalonia.Input.Key.Escape)
                {
                    searchTextBox.Watermark = null;
                }
            };

            searchTextBox.TextChanged += (s, e) => updateWatermark();
            searchTextBox.LostFocus += (s, e) => updateWatermark();
        }
    }
    
    private void UpdateFilterButtons()
    {
        if (DataContext is not MainWindowViewModel vm) return;
        
        var filterButtons = this.FindControl<StackPanel>("FilterButtons");
        if (filterButtons == null) return;
        
        foreach (var child in filterButtons.Children.OfType<Button>())
        {
            if (child.Tag is string tag)
            {
                var isActive = tag switch
                {
                    "All" => vm.FilterType == FilterType.All,
                    "Pending" => vm.FilterType == FilterType.Pending,
                    "Completed" => vm.FilterType == FilterType.Completed,
                    _ => false
                };
                
                if (isActive)
                {
                    child.Classes.Add("active");
                    child.Background = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.FromRgb(227, 242, 253));
                    child.Foreground = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.FromRgb(25, 118, 210));
                    child.FontWeight = Avalonia.Media.FontWeight.SemiBold;
                }
                else
                {
                    child.Classes.Remove("active");
                    child.Background = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.FromRgb(245, 245, 245));
                    child.Foreground = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.FromRgb(117, 117, 117));
                    child.FontWeight = Avalonia.Media.FontWeight.Normal;
                }
            }
        }
    }

    private void OnAddClick(object? sender, RoutedEventArgs e)
    {
        ShowAddTodoDialog();
    }

    private void OnDeleteAllClick(object? sender, RoutedEventArgs e)
    {
        if (DataContext is MainWindowViewModel vm)
        {
            vm.DeleteAllCommand.Execute(null);
        }
    }

    private void ShowAddTodoDialog()
    {
        if (DataContext is not MainWindowViewModel vm) return;

        var dialog = new Window
        {
            Title = "新增待辦事項",
            Width = 500,
            Height = 240,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            Background = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.FromRgb(255, 255, 255))
        };

        var textBox = new TextBox
        {
            Watermark = "請輸入待辦事項",
            Margin = new Avalonia.Thickness(24, 24, 24, 12),
            Height = 44,
            FontSize = 14,
            Background = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.FromRgb(255, 255, 255)),
            BorderBrush = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.FromRgb(224, 224, 224)),
            BorderThickness = new Avalonia.Thickness(1),
            Padding = new Avalonia.Thickness(12),
            Text = string.Empty,
            SelectionBrush = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.FromRgb(33, 150, 243)),
            SelectionForegroundBrush = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.FromRgb(255, 255, 255)),
            CaretBrush = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.FromRgb(33, 150, 243))
        };

        var tagsTextBox = new TextBox
        {
            Watermark = "標籤（用逗號分隔）",
            Margin = new Avalonia.Thickness(24, 0, 24, 12),
            Height = 40,
            FontSize = 14,
            Padding = new Avalonia.Thickness(12),
            Background = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.FromRgb(255, 255, 255)),
            BorderBrush = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.FromRgb(224, 224, 224)),
            BorderThickness = new Avalonia.Thickness(1)
        };

        System.Action updateWatermark = () =>
        {
            if (!string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Watermark = null;
            }
            else
            {
                textBox.Watermark = "請輸入待辦事項";
            }
        };

        textBox.GotFocus += (s, e) => textBox.Watermark = null;
        textBox.TextInput += (s, e) => textBox.Watermark = null;
        textBox.KeyDown += (s, e) =>
        {
            if (e.Key != Avalonia.Input.Key.Enter && 
                e.Key != Avalonia.Input.Key.Tab && 
                e.Key != Avalonia.Input.Key.Escape)
            {
                textBox.Watermark = null;
            }
        };
        textBox.TextChanged += (s, e) => updateWatermark();
        textBox.LostFocus += (s, e) => updateWatermark();

        var buttonPanel = new StackPanel
        {
            Orientation = Avalonia.Layout.Orientation.Horizontal,
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right,
            Margin = new Avalonia.Thickness(24, 0, 24, 24),
            Spacing = 12
        };

        var saveButton = new Button
        {
            Content = "確定",
            MinWidth = 100,
            Padding = new Avalonia.Thickness(20, 10),
            HorizontalContentAlignment = Avalonia.Layout.HorizontalAlignment.Center,
            VerticalContentAlignment = Avalonia.Layout.VerticalAlignment.Center,
            Background = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.FromRgb(33, 150, 243)),
            Foreground = Avalonia.Media.Brushes.White,
            BorderThickness = new Avalonia.Thickness(0),
            CornerRadius = new Avalonia.CornerRadius(6),
            FontSize = 14,
            FontWeight = Avalonia.Media.FontWeight.SemiBold,
            Cursor = new Avalonia.Input.Cursor(Avalonia.Input.StandardCursorType.Hand)
        };

        var cancelButton = new Button
        {
            Content = "取消",
            MinWidth = 100,
            Padding = new Avalonia.Thickness(20, 10),
            HorizontalContentAlignment = Avalonia.Layout.HorizontalAlignment.Center,
            VerticalContentAlignment = Avalonia.Layout.VerticalAlignment.Center,
            Background = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.FromRgb(245, 245, 245)),
            Foreground = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.FromRgb(33, 33, 33)),
            BorderThickness = new Avalonia.Thickness(0),
            CornerRadius = new Avalonia.CornerRadius(6),
            FontSize = 14,
            FontWeight = Avalonia.Media.FontWeight.SemiBold,
            Cursor = new Avalonia.Input.Cursor(Avalonia.Input.StandardCursorType.Hand)
        };

        saveButton.Click += (s, e) =>
        {
            var title = textBox.Text?.Trim() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(title))
            {
                var tagsText = tagsTextBox.Text?.Trim() ?? string.Empty;
                var tags = string.IsNullOrWhiteSpace(tagsText) 
                    ? new List<string>() 
                    : tagsText.Split(',').Select(t => t.Trim()).Where(t => !string.IsNullOrEmpty(t)).ToList();

                var todo = new TodoItem
                {
                    Title = title,
                    IsCompleted = false,
                    CreatedAt = DateTime.Now,
                    Tags = tags
                };
                vm.Todos.Insert(0, todo);
                vm.ApplyFilter();
                vm.MarkDirty();
                vm.UpdateStatus();
                dialog.Close();
            }
        };

        cancelButton.Click += (s, e) => dialog.Close();

        textBox.KeyDown += (s, e) =>
        {
            if (e.Key == Avalonia.Input.Key.Enter && e.KeyModifiers == Avalonia.Input.KeyModifiers.None)
            {
                saveButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                e.Handled = true;
            }
        };

        buttonPanel.Children.Add(cancelButton);
        buttonPanel.Children.Add(saveButton);

        var panel = new StackPanel
        {
            Margin = new Avalonia.Thickness(0)
        };
        panel.Children.Add(textBox);
        panel.Children.Add(tagsTextBox);
        panel.Children.Add(buttonPanel);

        dialog.Content = panel;
        dialog.ShowDialog(this);
        textBox.Focus();
    }

    private void OnCheckBoxChanged(object? sender, RoutedEventArgs e)
    {
        if (sender is CheckBox checkBox && checkBox.DataContext is TodoItem item && DataContext is MainWindowViewModel vm)
        {
            if (item.IsCompleted != checkBox.IsChecked)
            {
                vm.ToggleCompleteCommand.Execute(item);
            }
        }
    }

    private void OnDeleteClick(object? sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.DataContext is TodoItem item && DataContext is MainWindowViewModel vm)
        {
            vm.DeleteCommand.Execute(item);
        }
        e.Handled = true;
    }

    private void OnFilterClick(object? sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is string filterTag && DataContext is MainWindowViewModel vm)
        {
            var filterType = filterTag switch
            {
                "All" => FilterType.All,
                "Pending" => FilterType.Pending,
                "Completed" => FilterType.Completed,
                _ => FilterType.All
            };
            vm.FilterType = filterType;
            UpdateFilterButtons();
        }
        e.Handled = true;
    }

    private DateTime _lastClickTime = DateTime.MinValue;
    private TodoItem? _lastClickedItem = null;

    private void OnMainWindowPointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        HandleExternalClick(e.Source);
    }
    
    private void OnMainWindowPointerReleased(object? sender, Avalonia.Input.PointerReleasedEventArgs e)
    {
        HandleExternalClick(e.Source);
    }
    
    private void HandleExternalClick(object? source)
    {
        var searchTextBox = this.FindControl<TextBox>("SearchTextBox");
        if (searchTextBox == null) return;
        
        if (source == searchTextBox)
        {
            return;
        }
        
        var searchContainer = searchTextBox.Parent;
        if (source is Control control)
        {
            if (control == searchContainer)
            {
                return;
            }
            
            var parent = control.Parent;
            while (parent != null)
            {
                if (parent == searchTextBox || parent == searchContainer)
                {
                    return;
                }
                parent = parent.Parent;
            }
        }
        
        if (source is Button || source is CheckBox || source is TextBox)
        {
            return;
        }
        
        if (searchTextBox.IsFocused)
        {
            Avalonia.Threading.Dispatcher.UIThread.Post(() =>
            {
                var mainGrid = this.Content as Avalonia.Controls.Grid;
                if (mainGrid != null && mainGrid.Focusable)
                {
                    mainGrid.Focus();
                }
                else
                {
                    this.Focus();
                }
            }, Avalonia.Threading.DispatcherPriority.Normal);
        }
    }
    
    private bool IsControlOrAncestor(Control? target, Control? check)
    {
        if (target == null || check == null) return false;
        if (check == target) return true;
        
        var parent = check.Parent;
        while (parent != null)
        {
            if (parent == target) return true;
            parent = parent.Parent;
        }
        return false;
    }

    private void OnCardPointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        if (sender is Border border && border.DataContext is TodoItem item)
        {
            var now = DateTime.Now;
            var timeSinceLastClick = (now - _lastClickTime).TotalMilliseconds;
            
            if (_lastClickedItem == item && timeSinceLastClick < 500)
            {
                if (DataContext is MainWindowViewModel vm)
                {
                    ShowEditTodoDialog(item);
                }
                _lastClickTime = DateTime.MinValue;
                _lastClickedItem = null;
            }
            else
            {
                _lastClickTime = now;
                _lastClickedItem = item;
            }
        }
    }

    private void ShowEditTodoDialog(TodoItem item)
    {
        if (DataContext is not MainWindowViewModel vm) return;

        var dialog = new Window
        {
            Title = "編輯待辦事項",
            Width = 500,
            Height = 240,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            Background = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.FromRgb(255, 255, 255))
        };

        var textBox = new TextBox
        {
            Text = item.Title,
            Margin = new Avalonia.Thickness(24, 24, 24, 12),
            Height = 44,
            FontSize = 14,
            Background = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.FromRgb(255, 255, 255)),
            BorderBrush = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.FromRgb(224, 224, 224)),
            BorderThickness = new Avalonia.Thickness(1),
            Padding = new Avalonia.Thickness(12),
            SelectionBrush = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.FromRgb(33, 150, 243)),
            SelectionForegroundBrush = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.FromRgb(255, 255, 255)),
            CaretBrush = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.FromRgb(33, 150, 243))
        };

        var tagsTextBox = new TextBox
        {
            Text = item.Tags != null ? string.Join(", ", item.Tags) : string.Empty,
            Watermark = "標籤（用逗號分隔）",
            Margin = new Avalonia.Thickness(24, 0, 24, 12),
            Height = 40,
            FontSize = 14,
            Padding = new Avalonia.Thickness(12),
            Background = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.FromRgb(255, 255, 255)),
            BorderBrush = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.FromRgb(224, 224, 224)),
            BorderThickness = new Avalonia.Thickness(1)
        };

        var buttonPanel = new StackPanel
        {
            Orientation = Avalonia.Layout.Orientation.Horizontal,
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right,
            Margin = new Avalonia.Thickness(24, 0, 24, 24),
            Spacing = 12
        };

        var saveButton = new Button
        {
            Content = "確定",
            MinWidth = 100,
            Padding = new Avalonia.Thickness(20, 10),
            HorizontalContentAlignment = Avalonia.Layout.HorizontalAlignment.Center,
            VerticalContentAlignment = Avalonia.Layout.VerticalAlignment.Center,
            Background = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.FromRgb(33, 150, 243)),
            Foreground = Avalonia.Media.Brushes.White,
            BorderThickness = new Avalonia.Thickness(0),
            CornerRadius = new Avalonia.CornerRadius(6),
            FontSize = 14,
            FontWeight = Avalonia.Media.FontWeight.SemiBold,
            Cursor = new Avalonia.Input.Cursor(Avalonia.Input.StandardCursorType.Hand)
        };

        var cancelButton = new Button
        {
            Content = "取消",
            MinWidth = 100,
            Padding = new Avalonia.Thickness(20, 10),
            HorizontalContentAlignment = Avalonia.Layout.HorizontalAlignment.Center,
            VerticalContentAlignment = Avalonia.Layout.VerticalAlignment.Center,
            Background = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.FromRgb(245, 245, 245)),
            Foreground = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.FromRgb(33, 33, 33)),
            BorderThickness = new Avalonia.Thickness(0),
            CornerRadius = new Avalonia.CornerRadius(6),
            FontSize = 14,
            FontWeight = Avalonia.Media.FontWeight.SemiBold,
            Cursor = new Avalonia.Input.Cursor(Avalonia.Input.StandardCursorType.Hand)
        };

        saveButton.Click += (s, e) =>
        {
            var title = textBox.Text?.Trim() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(title))
            {
                var tagsText = tagsTextBox.Text?.Trim() ?? string.Empty;
                var tags = string.IsNullOrWhiteSpace(tagsText) 
                    ? new List<string>() 
                    : tagsText.Split(',').Select(t => t.Trim()).Where(t => !string.IsNullOrEmpty(t)).ToList();

                vm.UpdateTodo(item, title, tags);
                dialog.Close();
            }
            else
            {
                _notificationService.ShowError("請輸入待辦事項");
            }
        };

        cancelButton.Click += (s, e) => dialog.Close();

        textBox.KeyDown += (s, e) =>
        {
            if (e.Key == Avalonia.Input.Key.Enter && e.KeyModifiers == Avalonia.Input.KeyModifiers.None)
            {
                saveButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                e.Handled = true;
            }
        };

        buttonPanel.Children.Add(cancelButton);
        buttonPanel.Children.Add(saveButton);

        var panel = new StackPanel
        {
            Margin = new Avalonia.Thickness(0)
        };
        panel.Children.Add(textBox);
        panel.Children.Add(tagsTextBox);
        panel.Children.Add(buttonPanel);

        dialog.Content = panel;
        dialog.ShowDialog(this);
        textBox.Focus();
        textBox.SelectAll();
    }
}
