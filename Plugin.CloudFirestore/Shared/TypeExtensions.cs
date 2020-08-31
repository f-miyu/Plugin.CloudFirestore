using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Plugin.CloudFirestore
{
    internal static class TypeExtensions
    {
        public static bool TryGetImplementingGenericType(this Type type, [MaybeNullWhen(false)] out Type implementingType, Type targetType)
        {
            if (type.IsInterface && type.IsGenericType)
            {
                var definition = type.GetGenericTypeDefinition();

                if (definition == targetType)
                {
                    implementingType = type;
                    return true;
                }
            }

            implementingType = type.GetInterfaces()
                .Where(x => x.IsGenericType)
                .Where(x => x.GetGenericTypeDefinition() == targetType)
                .FirstOrDefault();

            return implementingType != null;
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
