using System;
using System.Collections.Concurrent;

namespace Plugin.CloudFirestore
{
    internal static class DocumentInfoProvider
    {
        private static readonly ConcurrentDictionary<Type, DocumentInfo> _documentInfos = new ConcurrentDictionary<Type, DocumentInfo>();

        public static DocumentInfo GetDocumentInfo(Type documentType)
        {
            return _documentInfos.GetOrAdd(documentType, _ => new DocumentInfo(documentType));
        }
    }
}
