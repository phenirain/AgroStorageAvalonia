using System;
using System.Collections.Generic;
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
            var attribute = fieldInfo.GetCustomAttribute<EnumMemberAttribute>();
            return attribute?.Value; // Возвращаем значение из атрибута EnumMember
        }).ToList();

        return memberValues;
    }
}
