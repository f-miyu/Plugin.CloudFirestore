using System;
using System.Reflection;
using Plugin.CloudFirestore.Attributes;
using Plugin.CloudFirestore.Converters;

namespace Plugin.CloudFirestore
{
    internal class MemberDocumentFieldInfo : DocumentFieldInfo
    {
        private readonly MemberInfo _memberInfo;
        private readonly DocumentConverterAttribute? _documentConverterAttribute;

        public MemberDocumentFieldInfo(MemberInfo memberInfo) : base(memberInfo switch
        {
            PropertyInfo propertyInfo => propertyInfo.PropertyType,
            FieldInfo fieldInfo => fieldInfo.FieldType,
            null => throw new ArgumentNullException(nameof(memberInfo)),
            _ => throw new ArgumentException($"{nameof(memberInfo)} must be PropertyInfo or FieldInfo.", nameof(memberInfo))
        })
        {
            _memberInfo = memberInfo;

            var idAttribute = _memberInfo.GetCustomAttribute<IdAttribute>();
            IsId = idAttribute != null;

            var ignoredAttribute = _memberInfo.GetCustomAttribute<IgnoredAttribute>();
            IsIgnored = ignoredAttribute != null;

            var mapToAttribute = _memberInfo.GetCustomAttribute<MapToAttribute>();
            Name = mapToAttribute?.Mapping ?? _memberInfo.Name;
            OriginalName = _memberInfo.Name;

            var serverTimestampAttribute = _memberInfo.GetCustomAttribute<ServerTimestampAttribute>();
            IsServerTimestamp = serverTimestampAttribute != null;
            CanReplaceServerTimestamp = serverTimestampAttribute?.CanReplace ?? false;

            _documentConverterAttribute = _memberInfo.GetCustomAttribute<DocumentConverterAttribute>();
        }

        public bool IsId { get; }
        public bool IsIgnored { get; }
        public string Name { get; }
        public string OriginalName { get; }
        public bool IsServerTimestamp { get; }
        public bool CanReplaceServerTimestamp { get; }

        private DocumentConverter? _documentConverter;
        public DocumentConverter? DocumentConverter
        {
            get
            {
                if (_documentConverterAttribute != null && _documentConverter == null)
                {
                    try
                    {
                        _documentConverter = DocumentConverterCreatorProvider
                            .GetCreator(_documentConverterAttribute.ConverterType)
                            .Invoke(Type, _documentConverterAttribute.ConverterParameters);
                    }
                    catch (Exception e)
                    {
                        throw new InvalidOperationException($"Error creating '{_documentConverterAttribute.ConverterType.FullName}'", e);
                    }
                }
                return _documentConverter;
            }
        }

        public object GetValue(object target)
        {
            var value = _memberInfo switch
            {
                PropertyInfo propertyInfo => propertyInfo.GetValue(target),
                FieldInfo fieldInfo => fieldInfo.GetValue(target),
                _ => throw new InvalidOperationException()
            };
            return value;
        }

        public void SetValue(object target, object? value)
        {
            switch (_memberInfo)
            {
                case PropertyInfo propertyInfo:
                    propertyInfo.SetValue(target, value);
                    break;
                case FieldInfo fieldInfo:
                    fieldInfo.SetValue(target, value);
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        public bool IsServerTimestampValue(object value)
        {
            return IsServerTimestamp
                && (CanReplaceServerTimestamp || value == null
                || (value is DateTime dateTime && dateTime == default)
                || (value is DateTimeOffset dateTimeOffset && dateTimeOffset == default)
                || (value is Timestamp timestamp && timestamp == default));
        }

        public override (bool IsConverted, object? Result) ConvertTo(object? value)
        {
            if (DocumentConverter != null && DocumentConverter.ConvertTo(value, out var result))
            {
                return (true, result);
            }
            return base.ConvertTo(value);
        }

        public override (bool IsConverted, object? Result) ConvertFrom(DocumentObject value)
        {
            if (DocumentConverter != null && DocumentConverter.ConvertFrom(value, out var result))
            {
                return (true, result);
            }
            return base.ConvertFrom(value);
        }
    }
}
