using System;
using System.ComponentModel;
using System.Windows.Markup;

namespace Diploma.MarkupExtensions
{
    [MarkupExtensionReturnType(typeof(Array))]
    internal sealed class EnumBindingSourceExtension : MarkupExtension
    {
        public EnumBindingSourceExtension()
        {
        }

        public EnumBindingSourceExtension(Type enumType)
        {
            EnumType = enumType ?? throw new ArgumentNullException(nameof(enumType));
        }

        [ConstructorArgument("enumType")]
        [DefaultValue(null)]
        public Type EnumType { get; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (EnumType == null)
            {
                throw new InvalidOperationException("EnumType must be filled before calling ProvideValue method");
            }

            var underlyingType = Nullable.GetUnderlyingType(EnumType);
            var isNullable = underlyingType != null;
            var actualType = isNullable ? underlyingType : EnumType;

            if (!actualType.IsEnum)
            {
                throw new InvalidOperationException("EnumType must represent an enumeration.");
            }

            var enumValues = Enum.GetValues(actualType);
            if (!isNullable)
            {
                return enumValues;
            }

            // If type is nullable then we need to add extra 'empty' element to return
            var tempArray = Array.CreateInstance(actualType, enumValues.Length + 1);
            enumValues.CopyTo(tempArray, 1);
            return tempArray;
        }
    }
}
