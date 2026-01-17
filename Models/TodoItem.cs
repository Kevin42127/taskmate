using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TaskMateApp.Models
{
    public enum Priority
    {
        Low = 0,
        Medium = 1,
        High = 2
    }

    public class TodoItem : INotifyPropertyChanged
    {
        private string _id = Guid.NewGuid().ToString();
        private string _title = string.Empty;
        private bool _isCompleted;
        private DateTime _createdAt = DateTime.Now;
        private DateTime? _completedAt;
        private string _borderColor = "#2196F3";
        private Priority _priority = Priority.Medium;

        public event PropertyChangedEventHandler? PropertyChanged;

        public string Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public bool IsCompleted
        {
            get => _isCompleted;
            set => SetProperty(ref _isCompleted, value);
        }

        public DateTime CreatedAt
        {
            get => _createdAt;
            set => SetProperty(ref _createdAt, value);
        }

        public DateTime? CompletedAt
        {
            get => _completedAt;
            set => SetProperty(ref _completedAt, value);
        }

        public string BorderColor
        {
            get => _borderColor;
            set => SetProperty(ref _borderColor, value);
        }

        public Priority Priority
        {
            get => _priority;
            set => SetProperty(ref _priority, value);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
