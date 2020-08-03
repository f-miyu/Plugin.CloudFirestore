using System;
using Foundation;
using Firebase.CloudFirestore;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Plugin.CloudFirestore.Attributes;

namespace Plugin.CloudFirestore
{
    internal static class DocumentMapper
    {
        public static IDictionary<string, object> Map(DocumentSnapshot source, ServerTimestampBehavior? serverTimestampBehavior = null)
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
                    data = source.GetData(serverTimestampBehavior.Value.ToNative());
                }

                var instance = new Dictionary<string, object>();

                foreach (var (key, value) in data)
                {
                    instance[key.ToString()] = value.ToFieldValue();
                }

                return instance;
            }
            return null;
        }

        public static T Map<T>(DocumentSnapshot source, ServerTimestampBehavior? serverTimestampBehavior = null)
        {
            return (T)ObjectProvider.GetDocumentInfo<T>().Create(source, serverTimestampBehavior);
        }
    }
}
