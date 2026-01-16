using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace TodoApp.Utils;

public class RoleToAlignmentConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string role)
        {
            return role switch
            {
                "user" => Avalonia.Layout.HorizontalAlignment.Right,
                "assistant" => Avalonia.Layout.HorizontalAlignment.Left,
                "system" => Avalonia.Layout.HorizontalAlignment.Center,
                _ => Avalonia.Layout.HorizontalAlignment.Left
            };
        }
        return Avalonia.Layout.HorizontalAlignment.Left;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class RoleToBackgroundConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string role)
        {
            return role switch
            {
                "user" => new SolidColorBrush(Color.FromRgb(74, 144, 226)),
                "assistant" => new SolidColorBrush(Color.FromRgb(45, 45, 45)),
                "system" => new SolidColorBrush(Color.FromRgb(255, 107, 107)),
                _ => new SolidColorBrush(Color.FromRgb(45, 45, 45))
            };
        }
        return new SolidColorBrush(Color.FromRgb(45, 45, 45));
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class BoolInverseConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
        {
            return !boolValue;
        }
        return true;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
        {
            return !boolValue;
        }
        return false;
    }
}

public class TodoItemBackgroundConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isCompleted)
        {
            return isCompleted 
                ? new SolidColorBrush(Color.FromRgb(245, 245, 245))
                : new SolidColorBrush(Color.FromRgb(255, 255, 255));
        }
        return new SolidColorBrush(Color.FromRgb(255, 255, 255));
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class TodoItemBorderConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isCompleted)
        {
            return new SolidColorBrush(Color.FromRgb(224, 224, 224));
        }
        return new SolidColorBrush(Color.FromRgb(224, 224, 224));
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class TodoItemTextColorConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isCompleted)
        {
            return isCompleted 
                ? new SolidColorBrush(Color.FromRgb(158, 158, 158))
                : new SolidColorBrush(Color.FromRgb(33, 33, 33));
        }
        return new SolidColorBrush(Color.FromRgb(33, 33, 33));
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class BoolToTextDecorationsConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isCompleted && isCompleted)
        {
            return Avalonia.Media.TextDecorations.Strikethrough;
        }
        return null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

