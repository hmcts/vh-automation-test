using System.ComponentModel;

namespace UI.PageModels.Extensions;

public static class EnumExtensions
{
    public static string GetDescription<T>(this T enumSrc) where T : Enum
    {
        var fieldInfo = enumSrc.GetType().GetField(enumSrc.ToString());
        if (fieldInfo == null) return enumSrc.ToString();
        var attributes = (DescriptionAttribute[]) fieldInfo
            .GetCustomAttributes(typeof(DescriptionAttribute), false);
        return attributes.Length > 0 ? attributes[0].Description : enumSrc.ToString();
    }
}