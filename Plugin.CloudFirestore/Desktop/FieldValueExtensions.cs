using System;
using System.Collections.Generic;
using System.Linq;

namespace Plugin.CloudFirestore
{
    internal static class FieldValueExtensions
    {
        public static object ToNativeFieldValue(this object? fieldValue, IDocumentFieldInfo? fieldInfo = null)
        {
            if (fieldValue is { } && fieldInfo is null)
            {
                fieldInfo = new DocumentFieldInfo(fieldValue.GetType());
            }

            if (fieldInfo?.ConvertTo(fieldValue) is (true, var result))
            {
                fieldValue = result;
                fieldInfo = result != null ? new DocumentFieldInfo(result.GetType()) : null;
            }

            switch (fieldValue)
            {
                case ulong @ulong:
                    if (@ulong > long.MaxValue)
                    {
                        throw new OverflowException();
                    }
                    return @ulong;
                case DateTime dateTime:
                    return Google.Cloud.Firestore.Timestamp.FromDateTime(dateTime);
                case DateTimeOffset dateTimeOffset:
                    return Google.Cloud.Firestore.Timestamp.FromDateTimeOffset(dateTimeOffset);
                case Timestamp timestamp:
                    return Google.Cloud.Firestore.Timestamp.FromDateTime(timestamp.ToDateTime());
                case GeoPoint geoPoint:
                    return geoPoint.ToNative();
                case IDocumentReference documentReference:
                    return documentReference.ToNative();
                case FieldValue firestoreFieldValue:
                    return firestoreFieldValue.ToNative();
                case FieldPath fieldPath:
                    return fieldPath.ToNative();
                default:
                    fieldInfo ??= new DocumentFieldInfo(fieldValue.GetType());
                    return fieldInfo.DocumentInfo.ConvertToFieldValue(fieldValue);
            }
        }

        public static Dictionary<string, object>? ToNativeFieldValues<T>(this T fieldValues)
        {
            if (fieldValues is null)
                return null;

            var dic = ObjectProvider.GetDocumentInfo<T>().ConvertToFieldObject(fieldValues);
            if (dic.Count == 0)
            {
                return new Dictionary<string, object>();
            }

            return dic.ToDictionary(x => x.Key.ToString(), x => x.Value.ToNativeFieldValue());
        }

        public static Dictionary<string, object>? ToNativeFieldValues(this object? fieldValues)
        {
            if (fieldValues is null)
                return null;

            var dic = ObjectProvider.GetDocumentInfo(fieldValues.GetType()).ConvertToFieldObject(fieldValues);
            if (dic.Count == 0)
            {
                return new Dictionary<string, object>();
            }
            return dic.ToDictionary(x => x.Key.ToString(), x => x.Value.ToNativeFieldValue());
        }

        public static object? ToFieldValue(this object? fieldValue, IDocumentFieldInfo fieldInfo)
        {
            return (fieldValue switch
            {
                null => new DocumentObject(),
                bool number => new DocumentObject(number),
                long number => new DocumentObject(number),
                int number => new DocumentObject(number),
                double number => new DocumentObject(number),
                string @string => new DocumentObject(@string),
                Timestamp timestamp => new DocumentObject(timestamp),
                DateTime date => new DocumentObject(new Timestamp(date)),
                object[] array => DocumentObject.CreateAsList((fieldInfo) => fieldInfo.DocumentInfo.Create(array)),
                byte[] array => DocumentObject.CreateAsList((fieldInfo) => fieldInfo.DocumentInfo.Create(array)),
                Dictionary<string, object> dictionary => DocumentObject.CreateAsDictionary((fieldInfo) => fieldInfo.DocumentInfo.Create(dictionary)),
                Google.Cloud.Firestore.GeoPoint geoPoint => new DocumentObject(new GeoPoint(geoPoint)),
                Google.Cloud.Firestore.DocumentReference documentReference => new DocumentObject(new DocumentReferenceWrapper(documentReference)),
                IDocumentReference doc => new DocumentObject(doc),
                _ => throw new ArgumentOutOfRangeException(nameof(fieldValue), $"{fieldValue.GetType().FullName} is not supported")
            }).GetFieldValue(fieldInfo);
        }
    }
}
