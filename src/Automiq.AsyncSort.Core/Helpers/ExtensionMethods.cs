using System.ComponentModel;

namespace Automiq.AsyncSort.Core.Helpers;

public static class ExtensionMethods
{
    public static string GetDescriptionAttribute<T>(this T source) where T : Enum 
    {
        return source?.GetType()
            .GetField(source.ToString())?
            .GetCustomAttributes(typeof(DescriptionAttribute), false)?
            .Cast<DescriptionAttribute>()?
            .FirstOrDefault()?.Description ?? string.Empty;
    }
}