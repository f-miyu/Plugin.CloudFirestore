using System;
using System.Reflection;
using Plugin.CloudFirestore.Attributes;

namespace Plugin.CloudFirestore
{
    internal class MemberDocumentFieldInfo : IDocumentFieldInfo
    {
        private readonly MemberInfo _memberInfo;

        public bool IsId { get; }
        public bool IsIgnored { get; }
        public string Name { get; }
        public string OriginalName { get; }
        public bool IsServerTimestamp { get; }
        public bool CanReplaceServerTimestamp { get; }
        public Type FieldType { get; }

        private IDocumentInfo _documentInfo;
        public IDocumentInfo DocumentInfo => _documentInfo ??= ObjectProvider.GetDocumentInfo(FieldType);

        public MemberDocumentFieldInfo(MemberInfo memberInfo)
        {
            _memberInfo = memberInfo ?? throw new ArgumentNullException(nameof(memberInfo));

            if (!(_memberInfo is PropertyInfo) && !(_memberInfo is FieldInfo))
            {
                throw new ArgumentException($"{nameof(memberInfo)} must be PropertyInfo or FieldInfo.", nameof(memberInfo));
            }

            var type = _memberInfo switch
            {
                PropertyInfo propertyInfo => propertyInfo.PropertyType,
                FieldInfo fieldInfo => fieldInfo.FieldType,
                _ => throw new InvalidOperationException()
            };

            FieldType = Nullable.GetUnderlyingType(type) ?? type;

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
        }

        public object GetValue(object target)
        {
            return _memberInfo switch
            {
                PropertyInfo propertyInfo => propertyInfo.GetValue(target),
                FieldInfo fieldInfo => fieldInfo.GetValue(target),
                _ => throw new InvalidOperationException()
            };
        }

        public void SetValue(object target, object value)
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
            return IsServerTimestamp &&
                (CanReplaceServerTimestamp || value == null ||
                (value is DateTime dateTime && dateTime == default) ||
                (value is DateTimeOffset dateTimeOffset && dateTimeOffset == default) ||
                (value is Timestamp timestamp && timestamp == default));
        }
    }
}
