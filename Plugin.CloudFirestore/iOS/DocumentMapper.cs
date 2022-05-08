using System;
using Foundation;
using Firebase.CloudFirestore;
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
                NSDictionary<NSString, NSObject> data;
                if (serverTimestampBehavior == null)
                {
                    data = source.Data;
                }
                else
                {
                    data = source.GetData(serverTimestampBehavior.Value.ToNative())!;
                }

                var instance = new Dictionary<string, object?>();
                var fieldInfo = new DocumentFieldInfo<object>();

                foreach (var (key, value) in data)
                {
                    instance[key.ToString()] = value.ToFieldValue(fieldInfo);
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
