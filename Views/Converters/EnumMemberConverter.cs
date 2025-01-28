using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;
using Avalonia.Data.Converters;

namespace desktop.Views.Converters;

public class EnumMemberConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is Enum enumValue)
        {
            var field = enumValue.GetType().GetField(enumValue.ToString());
            var attribute = field?.GetCustomAttribute<DescriptionAttribute>();
            return attribute?.Description ?? enumValue.ToString();
        }
        return value?.ToString() ?? string.Empty;
    }

    object? IValueConverter.ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return ConvertBack(value, targetType, parameter, culture);
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        return 1;
    }

}
