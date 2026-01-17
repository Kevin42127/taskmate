using System;
using System.Windows.Input;
using TaskMateApp.Common;
using TaskMateApp.Models;
using TaskMateApp.Views;

namespace TaskMateApp.ViewModels
{
    public class AddTaskDialogViewModel : ViewModelBase
    {
        private readonly AddTaskDialog _dialog;
        private string _title = string.Empty;
        private string _description = string.Empty;
        private Priority _priority = Priority.Medium;
        private bool _isEditMode;
        private string? _editTaskId;

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public Priority Priority
        {
            get => _priority;
            set => SetProperty(ref _priority, value);
        }

        public bool IsEditMode
        {
            get => _isEditMode;
            set => SetProperty(ref _isEditMode, value);
        }

        public string? EditTaskId
        {
            get => _editTaskId;
            set => SetProperty(ref _editTaskId, value);
        }

        public ICommand ConfirmCommand { get; }
        public ICommand CancelCommand { get; }

        public AddTaskDialogViewModel(AddTaskDialog dialog)
        {
            _dialog = dialog;
            ConfirmCommand = new RelayCommand(Confirm, () => !string.IsNullOrWhiteSpace(Title));
            CancelCommand = new RelayCommand(Cancel);

            PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(Title))
                {
                    ((RelayCommand)ConfirmCommand).RaiseCanExecuteChanged();
                }
            };
        }

        public void LoadTask(TodoItem task)
        {
            IsEditMode = true;
            EditTaskId = task.Id;
            Title = task.Title;
            Description = task.Description;
            Priority = task.Priority;
        }

        private void Confirm()
        {
            _dialog.TitleResult = Title.Trim();
            _dialog.DescriptionResult = Description.Trim();
            _dialog.PriorityResult = Priority;
            _dialog.EditTaskId = EditTaskId;
            _dialog.Close(true);
        }

        private void Cancel()
        {
            _dialog.Close(false);
        }
    }
}
