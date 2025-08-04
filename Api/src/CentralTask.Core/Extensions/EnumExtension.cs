using System.ComponentModel;

namespace CentralTask.Core.Extensions
{
    public static class EnumExtension
    {
        public static bool BeAValidEnumValue<TEnum>(TEnum value) where TEnum : struct, Enum
        {
            return Enum.IsDefined(typeof(TEnum), value);
        }

        public static string GetDescription(this Enum value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());
            var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }
    }
}
