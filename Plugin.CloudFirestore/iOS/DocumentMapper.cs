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
                var instance = new Dictionary<string, object>();

                foreach (var (key, value) in source.Data)
                {
                    instance[key.ToString()] = value.ToFieldValue(typeof(object));
                }

                return instance;
            }
            return null;
        }

        public static T Map<T>(DocumentSnapshot source, ServerTimestampBehavior? serverTimestampBehavior = null)
        {
            if (source != null && source.Exists)
            {
                NSDictionary<NSString, NSObject> data;
                if (source is QueryDocumentSnapshot queryDocumentSnapshot)
                {
                    if (serverTimestampBehavior == null)
                    {
                        data = queryDocumentSnapshot.Data;
                    }
                    else
                    {
                        data = queryDocumentSnapshot.GetData(serverTimestampBehavior.Value.ToNative());
                    }
                }
                else
                {
                    if (serverTimestampBehavior == null)
                    {
                        data = source.Data;
                    }
                    else
                    {
                        data = source.GetData(serverTimestampBehavior.Value.ToNative());
                    }
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
                        else if (data.TryGetValue(new NSString(fieldInfo.Name), out var value))
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
