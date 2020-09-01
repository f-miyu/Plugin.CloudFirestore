using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Plugin.CloudFirestore
{
    internal partial class ObjectDocumentInfo<T> : IDocumentInfo
    {
        private readonly Type _type = typeof(T);
        private Func<object>? _creator;

        public ObjectDocumentInfo()
        {
            _documentFieldInfos = new Lazy<IReadOnlyDictionary<string, MemberDocumentFieldInfo>>(() =>
                _type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(x => x.GetIndexParameters().Length == 0)
                    .Cast<MemberInfo>()
                    .Concat(_type.GetFields(BindingFlags.Public | BindingFlags.Instance))
                    .Select(x => new MemberDocumentFieldInfo(x))
                    .Where(x => !x.IsIgnored)
                    .ToDictionary(x => x.Name), System.Threading.LazyThreadSafetyMode.PublicationOnly);

            _mappingNames = new Lazy<IReadOnlyDictionary<string, string>>(() =>
                DocumentFieldInfos.Values
                    .Where(x => x.OriginalName != x.Name)
                    .ToDictionary(x => x.OriginalName, x => x.Name), System.Threading.LazyThreadSafetyMode.PublicationOnly);
        }

        private readonly Lazy<IReadOnlyDictionary<string, MemberDocumentFieldInfo>> _documentFieldInfos;
        private IReadOnlyDictionary<string, MemberDocumentFieldInfo> DocumentFieldInfos => _documentFieldInfos.Value;

        private readonly Lazy<IReadOnlyDictionary<string, string>> _mappingNames;
        private IReadOnlyDictionary<string, string> MappingNames => _mappingNames.Value;

        public string GetMappingName(string name)
        {
            if (MappingNames.TryGetValue(name, out var mappingNames))
            {
                return mappingNames;
            }
            return name;
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
