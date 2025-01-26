using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;
using Avalonia.Data.Converters;

namespace desktop.Views.Converters;

public class EnumMemberConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        if (value == null)
            return null;

        // Получаем поле enum
        var enumType = value.GetType();
        var enumValue = enumType.GetField(value.ToString());

        // Ищем атрибут EnumMember для этого поля
        var attribute = enumValue.GetCustomAttribute<EnumMemberAttribute>();

        // Если атрибут найден, возвращаем его значение, иначе возвращаем обычное имя
        return attribute?.Value ?? value.ToString();
    }

    object? IValueConverter.ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return ConvertBack(value, targetType, parameter, culture);
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        throw new NotImplementedException();
    }

}
