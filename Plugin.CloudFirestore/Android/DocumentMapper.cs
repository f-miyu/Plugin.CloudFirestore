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
                return data.ToDictionary(pair => pair.Key, pair => pair.Value.ToFieldValue(typeof(object)));
            }
            return null;
        }

        public static T Map<T>(DocumentSnapshot source, ServerTimestampBehavior? serverTimestampBehavior = null)
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

                var instance = (T)CreatorProvider.GetCreator(typeof(T)).Invoke();

                var fieldInfos = DocumentInfoProvider.GetDocumentInfo(typeof(T)).DocumentFieldInfos.Values;

                foreach (var fieldInfo in fieldInfos)
                {
                    try
                    {
                        if (fieldInfo.IsId)
                        {
                            fieldInfo.SetValue(instance, source.Id);
                        }
                        else if (data.TryGetValue(fieldInfo.Name, out var value))
                        {
                            fieldInfo.SetValue(instance, value.ToFieldValue(fieldInfo.FieldType));
                        }
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine($"{fieldInfo.Name} is invalid: {e.Message}");
                        throw;
                    }
                }

                return instance;
            }
            return default;
        }
    }
}
