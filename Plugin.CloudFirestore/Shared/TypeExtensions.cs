using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Plugin.CloudFirestore
{
    internal static class TypeExtensions
    {
        public static bool TryGetImplementingGenericType(this Type type, out Type implementingType, params Type[] targetTypes)
        {
            if (type.IsInterface && type.IsGenericType)
            {
                var definition = type.GetGenericTypeDefinition();

                if (targetTypes.Contains(definition))
                {
                    implementingType = type;
                    return true;
                }
            }

            implementingType = type.GetInterfaces()
                .Where(x => x.IsGenericType)
                .Where(x => targetTypes.Contains(x.GetGenericTypeDefinition()))
                .FirstOrDefault();

            return implementingType != null;
        }

        public static bool TryGetImplementingGenericDictionaryType(this Type type, out Type implementingType)
        {
            return type.TryGetImplementingGenericType(out implementingType, typeof(IDictionary<,>), typeof(IReadOnlyDictionary<,>));
        }

        public static bool TryGetImplementingGenericListType(this Type type, out Type implementingType)
        {
            return type.TryGetImplementingGenericType(out implementingType, typeof(IList<>), typeof(IReadOnlyList<>));
        }

        public static bool IsDictionaryType(this Type type)
        {
            return typeof(IDictionary).IsAssignableFrom(type) || type.TryGetImplementingGenericDictionaryType(out var _);
        }

        public static bool IsListType(this Type type)
        {
            return typeof(IList).IsAssignableFrom(type) || type.TryGetImplementingGenericListType(out var _);
        }

        public static bool IsGenericDefinition(this Type type, Type genericDefinition)
        {
            if (!type.IsGenericType)
            {
                return false;
            }
            return type.GetGenericTypeDefinition() == genericDefinition;
        }
    }
}
