using System;
using System.Reflection;
using Plugin.CloudFirestore.Converters;

namespace Plugin.CloudFirestore
{
    internal class DocumentFieldInfo : IDocumentFieldInfo
    {
        public DocumentFieldInfo(Type type)
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));
            NullableUnderlyingType = Nullable.GetUnderlyingType(type) ?? type;
        }

        public Type Type { get; }
        public Type NullableUnderlyingType { get; }

        private IDocumentInfo? _documentInfo;
        public virtual IDocumentInfo DocumentInfo => _documentInfo ??= ObjectProvider.GetDocumentInfo(NullableUnderlyingType);

        public virtual (bool IsConverted, object? Result) ConvertTo(object? value)
        {
            if (value is Enum)
            {
                var typeCode = Type.GetTypeCode(Enum.GetUnderlyingType(value.GetType()));
                object underlyingValue = typeCode switch
                {
                    TypeCode.Byte => (byte)value,
                    TypeCode.SByte => (sbyte)value,
                    TypeCode.Int16 => (short)value,
                    TypeCode.UInt16 => (ushort)value,
                    TypeCode.Int32 => (int)value,
                    TypeCode.UInt32 => (uint)value,
                    TypeCode.Int64 => (long)value,
                    TypeCode.UInt64 => (ulong)value,
                    _ => throw new InvalidOperationException(),
                };
                return (true, underlyingValue);
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

        public virtual (bool IsConverted, object? Result) ConvertFrom(DocumentObject value)
        {
            if (NullableUnderlyingType.IsEnum && value.Type == DocumentObjectType.Int64)
            {
                return (true, Enum.ToObject(NullableUnderlyingType, value.Int64));
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
