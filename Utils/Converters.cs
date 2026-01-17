using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;
using TaskMateApp.Models;

namespace TaskMateApp.Utils
{
    public class BoolToOpacityConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return boolValue ? 1.0 : 0.5;
            }
            return 1.0;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class DateTimeToStringConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is DateTime dateTime)
            {
                return dateTime.ToString("yyyy/MM/dd HH:mm");
            }
            return string.Empty;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class CompletedBackgroundConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool isCompleted && isCompleted)
            {
                return new SolidColorBrush(Color.Parse("#E8F5E9"));
            }
            return new SolidColorBrush(Color.Parse("#FFFFFF"));
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class CompletedButtonConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool isCompleted && isCompleted)
            {
                return new SolidColorBrush(Color.Parse("#4CAF50"));
            }
            return new SolidColorBrush(Color.Parse("#2196F3"));
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class StringToOpacityConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string str && string.IsNullOrWhiteSpace(str))
            {
                return 0.0;
            }
            return 1.0;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BorderColorConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string colorStr)
            {
                try
                {
                    return new SolidColorBrush(Color.Parse(colorStr));
                }
                catch
                {
                    return new SolidColorBrush(Color.Parse("#2196F3"));
                }
            }
            return new SolidColorBrush(Color.Parse("#2196F3"));
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class CompletedBorderColorConverter : IMultiValueConverter
    {
        public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
        {
            if (values == null || values.Count < 2)
                return new SolidColorBrush(Color.Parse("#2196F3"));

            var isCompleted = values[0] is bool completed && completed;
            var borderColor = values[1] as string;

            if (isCompleted)
            {
                return new SolidColorBrush(Color.Parse("#66BB6A"));
            }

            if (!string.IsNullOrEmpty(borderColor))
            {
                try
                {
                    return new SolidColorBrush(Color.Parse(borderColor));
                }
                catch
                {
                    return new SolidColorBrush(Color.Parse("#2196F3"));
                }
            }

            return new SolidColorBrush(Color.Parse("#2196F3"));
        }

        public object[] ConvertBack(object? value, Type[] targetTypes, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class PriorityConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is Priority priority)
            {
                return priority switch
                {
                    Priority.Low => "低",
                    Priority.Medium => "中",
                    Priority.High => "高",
                    _ => "中"
                };
            }
            return "中";
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string str)
            {
                return str switch
                {
                    "低" => Priority.Low,
                    "中" => Priority.Medium,
                    "高" => Priority.High,
                    _ => Priority.Medium
                };
            }
            return Priority.Medium;
        }
    }

    public class EditTitleConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool isEditMode)
            {
                return isEditMode ? "編輯待辦事項" : "新增待辦事項";
            }
            return "新增待辦事項";
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class PriorityColorConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is Priority priority)
            {
                return priority switch
                {
                    Priority.Low => new SolidColorBrush(Color.Parse("#4CAF50")),
                    Priority.Medium => new SolidColorBrush(Color.Parse("#FF9800")),
                    Priority.High => new SolidColorBrush(Color.Parse("#F44336")),
                    _ => new SolidColorBrush(Color.Parse("#FF9800"))
                };
            }
            return new SolidColorBrush(Color.Parse("#FF9800"));
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
