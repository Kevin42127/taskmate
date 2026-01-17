using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Input.Platform;
using Avalonia.Media;
using Avalonia.Styling;
using TaskMateApp.Services;
using TaskMateApp.ViewModels;

namespace TaskMateApp.Views
{
    public partial class MainWindow : Window
    {
        private readonly NotificationService _notificationService;
        private readonly ConfigService _configService;

        public MainWindow()
        {
            InitializeComponent();
            _notificationService = new NotificationService();
            _configService = new ConfigService();

            try
            {
                var iconPath = System.IO.Path.Combine(AppContext.BaseDirectory, "Assets", "app.ico");
                if (System.IO.File.Exists(iconPath))
                {
                    this.Icon = new WindowIcon(iconPath);
                }
            }
            catch
            {
            }
            
            this.Opened += MainWindow_Opened;
        }

        private void MainWindow_Opened(object? sender, System.EventArgs e)
        {
            _notificationService.Initialize(this);

            if (_configService.WindowMaximized)
            {
                WindowState = WindowState.Maximized;
            }

            this.PropertyChanged += MainWindow_PropertyChanged;
        }

        private async void TaskCard_AttachedToVisualTree(object? sender, VisualTreeAttachmentEventArgs e)
        {
            if (sender is Border border)
            {
                await Task.Delay(30);
                border.Opacity = 1.0;
                if (border.RenderTransform is ScaleTransform scaleTransform)
                {
                    scaleTransform.ScaleX = 1.0;
                    scaleTransform.ScaleY = 1.0;
                }
            }
        }

        private void MainWindow_PropertyChanged(object? sender, Avalonia.AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Property == Window.WindowStateProperty)
            {
                _configService.WindowMaximized = WindowState == WindowState.Maximized;
            }
        }

        private void MainGrid_PointerPressed(object? sender, PointerPressedEventArgs e)
        {
            if (e.Source is Control source)
            {
                if (!(source is TextBox) && !IsDescendantOfTextBox(source))
                {
                    if (sender is Grid grid)
                    {
                        grid.Focus();
                    }
                }
            }
        }

        private bool IsDescendantOfTextBox(Control control)
        {
            var parent = control.Parent;
            while (parent != null)
            {
                if (parent is TextBox)
                {
                    return true;
                }
                parent = parent.Parent;
            }
            return false;
        }

        private void ToggleCompleteButton_Click(object? sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is Models.TodoItem item && DataContext is ViewModels.MainWindowViewModel viewModel)
            {
                viewModel.ToggleCompleteCommand.Execute(item);
            }
        }

        private void DeleteButton_Click(object? sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is Models.TodoItem item && DataContext is ViewModels.MainWindowViewModel viewModel)
            {
                viewModel.DeleteTaskCommand.Execute(item);
            }
        }

        private void AddTaskButton_PointerPressed(object? sender, PointerPressedEventArgs e)
        {
            var dialog = new AddTaskDialog
            {
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };

            _ = ShowDialogAsync(dialog);
        }

        private void EditButton_Click(object? sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is Models.TodoItem item)
            {
                var dialog = new AddTaskDialog
                {
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                };

                _ = ShowDialogAsync(dialog, item);
            }
        }

        private async Task ShowDialogAsync(AddTaskDialog dialog, Models.TodoItem? editTask = null)
        {
            if (editTask != null && dialog.DataContext is ViewModels.AddTaskDialogViewModel viewModel)
            {
                viewModel.LoadTask(editTask);
            }

            var result = await dialog.ShowDialog<bool>(this);
            
            if (result && !string.IsNullOrWhiteSpace(dialog.TitleResult) && DataContext is ViewModels.MainWindowViewModel mainViewModel)
            {
                if (editTask != null && !string.IsNullOrEmpty(dialog.EditTaskId))
                {
                    mainViewModel.EditTask(dialog.EditTaskId, dialog.TitleResult, dialog.DescriptionResult ?? string.Empty, dialog.PriorityResult);
                }
                else
                {
                    mainViewModel.AddTask(dialog.TitleResult, dialog.DescriptionResult ?? string.Empty, dialog.PriorityResult);
                }
            }
        }

        private void SearchTextBox_GotFocus(object? sender, GotFocusEventArgs e)
        {
            if (sender is TextBox textBox && string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Watermark = string.Empty;
            }
        }

        private void SearchTextBox_TextInput(object? sender, TextInputEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                textBox.Watermark = string.Empty;
            }
        }

        private void SearchTextBox_TextChanged(object? sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    if (!textBox.IsFocused)
                    {
                        textBox.Watermark = "搜尋任務...";
                    }
                }
                else
                {
                    textBox.Watermark = string.Empty;
                }

                if (DataContext is ViewModels.MainWindowViewModel viewModel)
                {
                    viewModel.SearchText = textBox.Text ?? string.Empty;
                }
            }
        }

    }
}
