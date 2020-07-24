using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Plugin.CloudFirestore
{
    internal class DocumentInfo
    {
        public Type DocumentType { get; }
        public IReadOnlyDictionary<string, DocumentFieldInfo> DocumentFieldInfos { get; private set; }

        public DocumentInfo(Type documentType)
        {
            DocumentType = documentType ?? throw new ArgumentNullException(nameof(documentType));

            DocumentFieldInfos = DocumentType.GetProperties(BindingFlags.Public | BindingFlags.Instance).Cast<MemberInfo>()
                .Concat(DocumentType.GetFields(BindingFlags.Public | BindingFlags.Instance))
                .Select(x => new DocumentFieldInfo(x))
                .Where(x => !x.IsIgnored)
                .ToDictionary(x => x.Name);
        }
    }
}
