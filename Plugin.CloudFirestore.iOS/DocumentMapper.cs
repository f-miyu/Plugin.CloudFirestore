using System;
using Foundation;
using Firebase.CloudFirestore;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Plugin.CloudFirestore.Attributes;

namespace Plugin.CloudFirestore
{
    public static class DocumentMapper
    {
        public static IDictionary<string, object> Map(DocumentSnapshot source)
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

        public static T Map<T>(DocumentSnapshot source) where T : class
        {
            if (source != null && source.Exists)
            {
                NSDictionary<NSString, NSObject> data;
                if (source is QueryDocumentSnapshot queryDocumentSnapshot)
                {
                    data = queryDocumentSnapshot.Data;
                }
                else
                {
                    data = source.Data;
                }

                var properties = typeof(T).GetProperties();
                var idProperties = properties.Where(p => Attribute.GetCustomAttribute(p, typeof(IdAttribute)) != null);
                var mappedProperties = properties.Select(p => (Property: p, Attribute: Attribute.GetCustomAttribute(p, typeof(MapToAttribute)) as MapToAttribute))
                                                 .Where(t => t.Attribute != null)
                                                 .ToDictionary(t => t.Attribute.Mapping, t => t.Property);
                var igonoredProperties = properties.Where(p => Attribute.GetCustomAttribute(p, typeof(IgnoredAttribute)) != null);

                var instance = Activator.CreateInstance<T>();
                if (source != null)
                {
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
                }

                foreach (var idProperty in idProperties)
                {
                    idProperty.SetValue(instance, source.Id);
                }

                return instance;
            }
            return null;
        }
    }
}
