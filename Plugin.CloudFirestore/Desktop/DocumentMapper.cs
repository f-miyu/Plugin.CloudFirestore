using Google.Cloud.Firestore;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Plugin.CloudFirestore
{
    internal static class DocumentMapper
    {
        public static IDictionary<string, object?>? Map(DocumentSnapshot source, ServerTimestampBehavior? serverTimestampBehavior = null)
        {
            if (source != null && source.Exists)
            {
                Dictionary<string, object> data;
                if (serverTimestampBehavior == null)
                {
                    data = source.ToDictionary();
                }
                else
                {
                    throw new System.NotImplementedException();
                }

                var instance = new Dictionary<string, object?>();
                var fieldInfo = new DocumentFieldInfo<object>();

                foreach (var tuple in data)
                {
                    instance[tuple.Key.ToString()] = tuple.Value.ToFieldValue(fieldInfo);
                }

                return instance;
            }
            return null;
        }

        [return: MaybeNull]
        public static T Map<T>(DocumentSnapshot source, ServerTimestampBehavior? serverTimestampBehavior = null)
        {
            var value = ObjectProvider.GetDocumentInfo<T>().Create(source, serverTimestampBehavior);
            return value is not null ? (T)value : default;
        }
    }
}
