using System.Collections.Immutable;
using System.Diagnostics.Contracts;
using System.Reflection;
using System.Runtime.InteropServices.JavaScript;

namespace Domain.Extensions;

public static class TypeExtensions
{
    [Pure]
    public static ImmutableArray<FieldInfo> FindAllConstantFields(this Type type)
    {
        return type
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(fieldInfo => fieldInfo is { IsLiteral: true, IsInitOnly: false })
            .ToImmutableArray();
    }

    [Pure]
    public static ImmutableArray<string> GetConstantFieldsValues(this ImmutableArray<FieldInfo> fieldInfos)
    {
        var values = new string[fieldInfos.Length];
        for (var index = 0; index < fieldInfos.Length; index++)
        {
            var fieldInfo = fieldInfos[index];
            values[index] = fieldInfo
                .GetValue(null)?
                .ToString()!;
        }

        return values.ToImmutableArray();
    }
}