using System;
using System.Reflection;

namespace Plugin.CloudFirestore
{
    public class DocumentFieldInfo : IDocumentFieldInfo
    {
        public Type FieldType { get; }

        private IDocumentInfo _documentInfo;
        public IDocumentInfo DocumentInfo => _documentInfo ??= ObjectProvider.GetDocumentInfo(FieldType);

        public DocumentFieldInfo(Type fieldType)
        {
            FieldType = Nullable.GetUnderlyingType(fieldType) ?? fieldType;
        }
    }

    public class DocumentFieldInfo<T> : IDocumentFieldInfo
    {
        public Type FieldType { get; }

        private IDocumentInfo _documentInfo;
        public IDocumentInfo DocumentInfo => _documentInfo ??= ObjectProvider.GetDocumentInfo(FieldType);

        public DocumentFieldInfo()
        {
            var fieldType = typeof(T);
            FieldType = Nullable.GetUnderlyingType(fieldType) ?? fieldType;
        }
    }
}
