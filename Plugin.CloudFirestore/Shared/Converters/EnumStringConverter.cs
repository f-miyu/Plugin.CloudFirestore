using System;

namespace Plugin.CloudFirestore.Converters
{
    public class EnumStringConverter : DocumentConverter
    {
        public EnumStringConverter(Type targetType) : base(targetType)
        {
        }

        public override bool ConvertTo(object? value, out object? result)
        {
            if (value is Enum)
            {
                result = value.ToString();
                return true;
            }

            result = null;
            return false;
        }

        public override bool ConvertFrom(DocumentObject value, out object? result)
        {
            var type = Nullable.GetUnderlyingType(TargetType) ?? TargetType;
            if (!type.IsEnum || value.Type != DocumentObjectType.String)
            {
                result = null;
                return false;
            }
            result = Enum.Parse(type, value.String);
            return true;
        }
    }
}
