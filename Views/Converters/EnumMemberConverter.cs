using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;
using Avalonia.Data.Converters;

namespace desktop.Views.Converters;

public class EnumMemberConverter : IValueConverter
{
    private readonly Dictionary<Enum, string> _cache = new Dictionary<Enum, string>();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is Enum enumValue)
        {
            if (_cache.ContainsKey(enumValue))
            {
                return _cache[enumValue];
            }

            var field = enumValue.GetType().GetField(enumValue.ToString());
            var attribute = field?.GetCustomAttribute<DescriptionAttribute>();
            var description = attribute?.Description ?? enumValue.ToString();

            _cache[enumValue] = description; 

            return description;
        }
        return value?.ToString() ?? string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
