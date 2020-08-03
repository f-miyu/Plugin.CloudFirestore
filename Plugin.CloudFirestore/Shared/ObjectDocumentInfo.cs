using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Plugin.CloudFirestore
{
    internal partial class ObjectDocumentInfo<T> : IDocumentInfo
    {
        private readonly Type _type = typeof(T);
        private Func<object> _creator;

        private IReadOnlyDictionary<string, MemberDocumentFieldInfo> _documentFieldInfos;
        private IReadOnlyDictionary<string, MemberDocumentFieldInfo> DocumentFieldInfos => _documentFieldInfos ??=
            _type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.GetIndexParameters().Length == 0)
                .Cast<MemberInfo>()
                .Concat(_type.GetFields(BindingFlags.Public | BindingFlags.Instance))
                .Select(x => new MemberDocumentFieldInfo(x))
                .Where(x => !x.IsIgnored)
                .ToDictionary(x => x.Name);

        private IReadOnlyDictionary<string, string> _mappingNames;
        private IReadOnlyDictionary<string, string> MappingNames => _mappingNames ??=
            DocumentFieldInfos.Values
                .Where(x => x.OriginalName != x.Name)
                .ToDictionary(x => x.OriginalName, x => x.Name);

        public string GetMappingName(string name)
        {
            if (MappingNames.TryGetValue(name, out var mappingNames))
            {
                return mappingNames;
            }
            return name;
        }

        public object ConvertToFieldObject(object target)
        {
#if NETSTANDARD
            throw new NotImplementedException();
#else
            var targetType = target.GetType();
            if (_type != targetType)
            {
                return ObjectProvider.GetDocumentInfo(targetType).ConvertToFieldObject(target);
            }
            return PlatformConvertToFieldObject(target);
#endif
        }

        public object ConvertToFieldValue(object target)
        {
#if NETSTANDARD
            throw new NotImplementedException();
#else
            var targetType = target.GetType();
            if (_type != targetType)
            {
                return ObjectProvider.GetDocumentInfo(targetType).ConvertToFieldValue(target);
            }
            return PlatformConvertToFieldValue(target);
#endif
        }

        public object Create(object value, ServerTimestampBehavior? serverTimestampBehavior = null)
        {
#if NETSTANDARD
            throw new NotImplementedException();
#else
            return PlatformCreate(value, serverTimestampBehavior);
#endif
        }

        private object Create()
        {
            if (_creator == null)
            {
                _creator = CreatorProvider.GetCreator<T>();
            }
            return _creator.Invoke();
        }
    }
}
