using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using TaskMateApp.ViewModels;
using System;
using System.Threading.Tasks;

namespace TaskMateApp.Views
{
    public partial class AddTaskDialog : Window
    {
        public AddTaskDialog()
        {
            InitializeComponent();
            DataContext = new AddTaskDialogViewModel(this);
            this.Opened += AddTaskDialog_Opened;
        }

        private async void AddTaskDialog_Opened(object? sender, System.EventArgs e)
        {
            await Task.Delay(10);
            this.Opacity = 1.0;
            if (this.RenderTransform is Avalonia.Media.ScaleTransform scaleTransform)
            {
                scaleTransform.ScaleX = 1.0;
                scaleTransform.ScaleY = 1.0;
            }
        }

        public string? TitleResult { get; internal set; }
        public string? DescriptionResult { get; internal set; }
        public Models.Priority PriorityResult { get; internal set; } = Models.Priority.Medium;
        public string? EditTaskId { get; internal set; }

        private void TitleTextBox_GotFocus(object? sender, GotFocusEventArgs e)
        {
            if (sender is TextBox textBox && string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Watermark = string.Empty;
            }
        }

        private void TitleTextBox_TextInput(object? sender, TextInputEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                textBox.Watermark = string.Empty;
            }
        }

        private void TitleTextBox_TextChanged(object? sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    if (!textBox.IsFocused)
                    {
                        textBox.Watermark = "輸入待辦事項標題...";
                    }
                }
                else
                {
                    textBox.Watermark = string.Empty;
                }

                if (DataContext is AddTaskDialogViewModel viewModel)
                {
                    viewModel.Title = textBox.Text ?? string.Empty;
                }
            }
        }

        private void DescriptionTextBox_GotFocus(object? sender, GotFocusEventArgs e)
        {
            if (sender is TextBox textBox && string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Watermark = string.Empty;
            }
        }

        private void DescriptionTextBox_TextInput(object? sender, TextInputEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                textBox.Watermark = string.Empty;
            }
        }

        private void DescriptionTextBox_TextChanged(object? sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    if (!textBox.IsFocused)
                    {
                        textBox.Watermark = "輸入描述（選填）...";
                    }
                }
                else
                {
                    textBox.Watermark = string.Empty;
                }

                if (DataContext is AddTaskDialogViewModel viewModel)
                {
                    viewModel.Description = textBox.Text ?? string.Empty;
                }
            }
        }

        private void TitleTextBox_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && DataContext is AddTaskDialogViewModel viewModel)
            {
                if (viewModel.ConfirmCommand.CanExecute(null))
                {
                    viewModel.ConfirmCommand.Execute(null);
                    e.Handled = true;
                }
            }
        }

        private void DescriptionTextBox_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && e.KeyModifiers == KeyModifiers.Control && DataContext is AddTaskDialogViewModel viewModel)
            {
                if (viewModel.ConfirmCommand.CanExecute(null))
                {
                    viewModel.ConfirmCommand.Execute(null);
                    e.Handled = true;
                }
            }
        }
    }
}
