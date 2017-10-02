using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace Diploma.Converters
{
    public class EnumDescriptionTypeConverter : EnumConverter
    {
        public EnumDescriptionTypeConverter(Type type)
            : base(type)
        {
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType != typeof(string))
            {
                return base.ConvertTo(context, culture, value, destinationType);
            }

            if (value == null)
            {
                return string.Empty;
            }

            var enumValue = value.ToString();

            var enumType = value.GetType();

            var fieldInfo = enumType.GetField(enumValue);
            if (fieldInfo == null)
            {
                return enumValue;
            }

            var attributes = fieldInfo.GetCustomAttribute<DescriptionAttribute>(false);
            return !string.IsNullOrEmpty(attributes.Description) ? attributes.Description : enumValue;
        }
    }
}
