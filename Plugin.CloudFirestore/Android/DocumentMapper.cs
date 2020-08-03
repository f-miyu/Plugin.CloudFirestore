using System;
using Firebase.Firestore;
using System.Collections.Generic;
using System.Linq;

namespace Plugin.CloudFirestore
{
    internal static class DocumentMapper
    {
        public static IDictionary<string, object> Map(DocumentSnapshot source, ServerTimestampBehavior? serverTimestampBehavior = null)
        {
            if (source != null && source.Exists())
            {
                IDictionary<string, Java.Lang.Object> data;
                if (serverTimestampBehavior == null)
                {
                    data = source.Data;
                }
                else
                {
                    data = source.GetData(serverTimestampBehavior.Value.ToNative());
                }
                return data.ToDictionary(pair => pair.Key, pair => pair.Value.ToFieldValue());
            }
            return null;
        }

        public static T Map<T>(DocumentSnapshot source, ServerTimestampBehavior? serverTimestampBehavior = null)
        {
            return (T)ObjectProvider.GetDocumentInfo<T>().Create(source, serverTimestampBehavior);
        }
    }
}
