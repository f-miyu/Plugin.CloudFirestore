using System;
using System.Reflection;
using EnumsNET;
using Plugin.CloudFirestore.Converters;

namespace Plugin.CloudFirestore
{
    internal class DocumentFieldInfo : IDocumentFieldInfo
    {
        public Type Type { get; }
        public Type NullableUnderlyingType { get; }

        private IDocumentInfo _documentInfo;
        public virtual IDocumentInfo DocumentInfo => _documentInfo ??= ObjectProvider.GetDocumentInfo(NullableUnderlyingType);

        public DocumentFieldInfo(Type type)
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));
            NullableUnderlyingType = Nullable.GetUnderlyingType(type) ?? type;
        }

        public virtual (bool IsConverted, object Result) ConvertTo(object value)
        {
            if (value is Enum)
            {
                return (true, Enums.GetUnderlyingValue(value.GetType(), value));
            }
            else if (value is Guid guid)
            {
                return (true, guid.ToString());
            }
            else if (value is DocumentObject documentObject)
            {
                return (true, documentObject.Value);
            }
            return (false, null);
        }

        public virtual (bool IsConverted, object Result) ConvertFrom(DocumentObject value)
        {
            if (NullableUnderlyingType.IsEnum && value.Type == DocumentObjectType.Long)
            {
                return (true, Enums.ToObject(NullableUnderlyingType, value.Long));
            }
            else if (NullableUnderlyingType == typeof(Guid) && value.Type == DocumentObjectType.String)
            {
                return (true, new Guid(value.String));
            }
            else if (NullableUnderlyingType == typeof(DocumentObject))
            {
                return (true, value);
            }
            return (false, null);
        }
    }

    internal class DocumentFieldInfo<T> : DocumentFieldInfo
    {
        public DocumentFieldInfo() : base(typeof(T))
        {
        }
    }
}
