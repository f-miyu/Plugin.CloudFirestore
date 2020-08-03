using System;
using Foundation;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.IO;

namespace Plugin.CloudFirestore
{
    internal static class FieldValueExtensions
    {
        public static object ToNativeFieldValue(this object fieldValue, IDocumentFieldInfo fieldInfo = null)
        {
            if (fieldValue == null)
                return new NSNull();

            switch (fieldValue)
            {
                case bool @bool:
                    return new NSNumber(@bool);
                case byte @byte:
                    return new NSNumber(@byte);
                case double @double:
                    return new NSNumber(@double);
                case float @float:
                    return new NSNumber(@float);
                case int @int:
                    return new NSNumber(@int);
                case long @long:
                    return new NSNumber(@long);
                case sbyte @sbyte:
                    return new NSNumber(@sbyte);
                case short @short:
                    return new NSNumber(@short);
                case uint @uint:
                    return new NSNumber(@uint);
                case ulong @ulong:
                    return new NSNumber(@ulong);
                case ushort @ushort:
                    return new NSNumber(@ushort);
                case decimal @decimal:
                    return new NSNumber((double)@decimal);
                case string @string:
                    return new NSString(@string);
                case DateTime dateTime:
                    return new Timestamp(dateTime).ToNative();
                case DateTimeOffset dateTimeOffset:
                    return new Timestamp(dateTimeOffset).ToNative();
                case Timestamp timestamp:
                    return timestamp.ToNative();
                case GeoPoint geoPoint:
                    return geoPoint.ToNative();
                case DocumentReferenceWrapper documentReference:
                    return (Firebase.CloudFirestore.DocumentReference)documentReference;
                case byte[] @byte:
                    return NSData.FromArray(@byte);
                case Stream stream:
                    using (var ms = new MemoryStream())
                    {
                        stream.CopyTo(ms);
                        return NSData.FromArray(ms.ToArray());
                    }
                case FieldValue firestoreFieldValue:
                    return firestoreFieldValue.ToNative();
                case FieldPath fieldPath:
                    return fieldPath.ToNative();
                default:
                    {
                        var type = fieldValue.GetType();

                        if (type.IsPrimitive)
                            throw new NotSupportedException($"{type.FullName} is not supported");

                        fieldInfo ??= new DocumentFieldInfo(type);

                        return fieldInfo.DocumentInfo.ConvertToFieldValue(fieldValue);
                    }
            }
        }

        public static Dictionary<object, object> ToNativeFieldValues<T>(this T fieldValues)
        {
            if (fieldValues == null)
                return null;

            return ObjectProvider.GetDocumentInfo<T>().ConvertToFieldObject(fieldValues) as Dictionary<object, object>;
        }

        public static Dictionary<object, object> ToNativeFieldValues(this object fieldValues)
        {
            if (fieldValues == null)
                return null;

            return ObjectProvider.GetDocumentInfo(fieldValues.GetType()).ConvertToFieldObject(fieldValues) as Dictionary<object, object>;
        }

        public static object ToFieldValue(this NSObject fieldValue, IDocumentFieldInfo fieldInfo = null)
        {
            if (fieldValue == null)
                return null;

            var type = fieldInfo?.FieldType ?? typeof(object);

            switch (fieldValue)
            {
                case NSNumber number:
                    if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        type = type.GenericTypeArguments[0];
                    }

                    if (type == typeof(bool))
                    {
                        return number.BoolValue;
                    }
                    if (type == typeof(byte))
                    {
                        return number.ByteValue;
                    }
                    if (type == typeof(double))
                    {
                        return number.DoubleValue;
                    }
                    if (type == typeof(float))
                    {
                        return number.FloatValue;
                    }
                    if (type == typeof(int))
                    {
                        return number.Int32Value;
                    }
                    if (type == typeof(long))
                    {
                        return number.Int64Value;
                    }
                    if (type == typeof(sbyte))
                    {
                        return number.SByteValue;
                    }
                    if (type == typeof(short))
                    {
                        return number.Int16Value;
                    }
                    if (type == typeof(uint))
                    {
                        return number.UInt32Value;
                    }
                    if (type == typeof(ulong))
                    {
                        return number.UInt64Value;
                    }
                    if (type == typeof(ushort))
                    {
                        return number.UInt16Value;
                    }
                    return Convert.ChangeType(number.DoubleValue, type);
                case NSString @string:
                    return @string.ToString();
                case Firebase.CloudFirestore.Timestamp timestamp:
                    {
                        var time = new Timestamp(timestamp);
                        if (type == typeof(DateTime) || type == typeof(DateTime?))
                        {
                            return time.ToDateTime();
                        }
                        if (type == typeof(DateTimeOffset) || type == typeof(DateTimeOffset?))
                        {
                            return time.ToDateTimeOffset();
                        }
                        return time;
                    }
                case NSDate date:
                    {
                        var time = new Timestamp(date);
                        if (type == typeof(DateTime) || type == typeof(DateTime?))
                        {
                            return time.ToDateTime();
                        }
                        if (type == typeof(DateTimeOffset) || type == typeof(DateTimeOffset?))
                        {
                            return time.ToDateTimeOffset();
                        }
                        return time;
                    }
                case NSArray array:
                    {
                        fieldInfo ??= new DocumentFieldInfo<List<object>>();
                        return fieldInfo.DocumentInfo.Create(array);
                    }
                case NSDictionary dictionary:
                    {
                        fieldInfo ??= new DocumentFieldInfo<Dictionary<string, object>>();
                        return fieldInfo.DocumentInfo.Create(dictionary);
                    }
                case NSData data:
                    if (type == typeof(byte[]))
                    {
                        return data.ToArray();
                    }
                    return new MemoryStream(data.ToArray());
                case Firebase.CloudFirestore.GeoPoint geoPoint:
                    return new GeoPoint(geoPoint);
                case Firebase.CloudFirestore.DocumentReference documentReference:
                    return new DocumentReferenceWrapper(documentReference);
                case NSNull @null:
                    return null;
                default:
                    throw new ArgumentOutOfRangeException($"{fieldValue.GetType().FullName} is not supported");
            }
        }

    }
}
