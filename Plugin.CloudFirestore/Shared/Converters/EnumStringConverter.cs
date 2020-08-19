using System;

namespace Plugin.CloudFirestore.Converters
{
    public class EnumStringConverter : DocumentConverter
    {
        public EnumStringConverter(Type targetType) : base(targetType)
        {
        }

        public override (bool IsConverted, object Result) ConvertTo(object value)
        {
            if (value is null) return (false, null);

            var type = value.GetType();

            if (type.IsEnum)
            {
                return (true, value.ToString());
            }
            return (false, null);
        }

        public override (bool IsConverted, object Result) ConvertFrom(DocumentObject value)
        {
            var type = Nullable.GetUnderlyingType(TargetType) ?? TargetType;
            if (!type.IsEnum || value.Type != DocumentObjectType.String)
            {
                return (false, null);
            }
            return (true, Enum.Parse(type, value.String));
        }
    }
}
