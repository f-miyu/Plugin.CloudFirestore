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

                var properties = typeof(T).GetProperties();
                var idProperties = properties.Where(p => Attribute.GetCustomAttribute(p, typeof(IdAttribute)) != null);
                var mappedProperties = properties.Select(p => (Property: p, Attribute: Attribute.GetCustomAttribute(p, typeof(MapToAttribute)) as MapToAttribute))
                                                 .Where(t => t.Attribute != null)
                                                 .ToDictionary(t => t.Attribute.Mapping, t => t.Property);
                var igonoredProperties = properties.Where(p => Attribute.GetCustomAttribute(p, typeof(IgnoredAttribute)) != null);

                var instance = Activator.CreateInstance<T>();
                foreach (var (key, value) in data)
                {
                    try
                    {
                        PropertyInfo property;
                        if (mappedProperties.ContainsKey(key.ToString()))
                        {
                            property = mappedProperties[key.ToString()];
                        }
                        else
                        {
                            property = typeof(T).GetProperty(key.ToString());
                        }

                        if (property != null && !igonoredProperties.Contains(property))
                        {
                            property.SetValue(instance, value.ToFieldValue(property.PropertyType));
                        }
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine($"{key} is invalid: {e.Message}");
                        throw;
                    }
                }

                foreach (var idProperty in idProperties)
                {
                    idProperty.SetValue(instance, source.Id);
                }

                return instance;
            }
            return default;
        }
    }
}
