using System;
using System.Reflection;
using Plugin.CloudFirestore.Attributes;

namespace Plugin.CloudFirestore
{
    internal class DocumentFieldInfo
    {
        private readonly MemberInfo _memberInfo;

        public bool IsId { get; private set; }
        public bool IsIgnored { get; private set; }
        public string Name { get; private set; }
        public string OriginalName { get; private set; }
        public bool IsServerTimestamp { get; private set; }
        public bool CanReplaceServerTimestamp { get; private set; }

        public Type FieldType => _memberInfo switch
        {
            PropertyInfo propertyInfo => propertyInfo.PropertyType,
            FieldInfo fieldInfo => fieldInfo.FieldType,
            _ => throw new InvalidOperationException("Can not get field type.")
        };

        public DocumentFieldInfo(MemberInfo memberInfo)
        {
            _memberInfo = memberInfo ?? throw new ArgumentNullException(nameof(memberInfo));

            if (!(_memberInfo is PropertyInfo) && !(_memberInfo is FieldInfo))
            {
                throw new ArgumentException($"{nameof(memberInfo)} must be PropertyInfo or FieldInfo.", nameof(memberInfo));
            }

            var idAttribute = Attribute.GetCustomAttribute(_memberInfo, typeof(IdAttribute));
            IsId = idAttribute != null;

            var ignoredAttribute = Attribute.GetCustomAttribute(_memberInfo, typeof(IgnoredAttribute));
            IsIgnored = ignoredAttribute != null;

            var mapToAttribute = (MapToAttribute)Attribute.GetCustomAttribute(_memberInfo, typeof(MapToAttribute));
            Name = mapToAttribute?.Mapping ?? _memberInfo.Name;
            OriginalName = _memberInfo.Name;

            var serverTimestampAttribute = (ServerTimestampAttribute)Attribute.GetCustomAttribute(_memberInfo, typeof(ServerTimestampAttribute));
            IsServerTimestamp = serverTimestampAttribute != null;
            CanReplaceServerTimestamp = serverTimestampAttribute?.CanReplace ?? false;
        }

        public object GetValue(object target)
        {
            return _memberInfo switch
            {
                PropertyInfo propertyInfo => propertyInfo.GetValue(target),
                FieldInfo fieldInfo => fieldInfo.GetValue(target),
                _ => throw new InvalidOperationException("Can not get value.")
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
                    throw new InvalidOperationException("Can not set value.");
            }
        }
    }
}
