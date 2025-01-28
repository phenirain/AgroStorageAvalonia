using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace desktop.Support;

public class ProgramHelper
{
    public static List<string> GetEnumMemberValues<T>()
    {
        var enumType = typeof(T);
        var values = Enum.GetValues(enumType).Cast<Enum>();

        var memberValues = values.Select(value =>
        {
            var fieldInfo = enumType.GetField(value.ToString());
            var attribute = fieldInfo.GetCustomAttribute<DescriptionAttribute>();
            return attribute?.Description.ToString(); // Возвращаем значение из атрибута EnumMember
        }).ToList();

        return memberValues;
    }

    public static TEnum? GetEnumValueFromEnumMember<TEnum>(string value) where TEnum : struct
    {
        var enumType = typeof(TEnum);

        foreach (var field in enumType.GetFields())
        {
            var enumMember = field.GetCustomAttributes(typeof(EnumMemberAttribute), false)
                .Cast<EnumMemberAttribute>()
                .FirstOrDefault();

            if (enumMember != null && enumMember.Value == value)
            {
                return (TEnum?)field.GetValue(null);
            }
        }

        return null;
    }
}
