using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Reihan.Client.Extensions
{
    public static class EnumExtensions
    {
        public static List<string> GetDisplayNames<T>() where T : Enum
        {
               return Enum.GetValues(typeof(T))
                .Cast<T>().Select(s => s.GetDisplayName())
                .ToList();
        }

        public static string GetDisplayName(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = field?.GetCustomAttribute<DisplayAttribute>();
            return attribute?.Name ?? value.ToString();
        }

        public static TEnum? GetEnumFromDisplayName<TEnum>(string displayName) where TEnum : struct, Enum
        {
            return Enum.GetValues(typeof(TEnum))
                .Cast<TEnum>()
                .FirstOrDefault(e => e.GetDisplayName() == displayName);
        }
    }
}
