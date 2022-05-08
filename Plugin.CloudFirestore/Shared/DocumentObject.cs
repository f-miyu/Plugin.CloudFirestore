using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Plugin.CloudFirestore
{
    public sealed partial class DocumentObject
    {
        private readonly Func<IDocumentFieldInfo, object?>? _creator;

        public DocumentObject()
        {
            Type = DocumentObjectType.Null;
        }

        public DocumentObject(bool value)
        {
            _boolean = value;
            Type = DocumentObjectType.Boolean;
        }

        public DocumentObject(long value)
        {
            _int64 = value;
            Type = DocumentObjectType.Int64;
        }

        public DocumentObject(double value)
        {
            _double = value;
            Type = DocumentObjectType.Double;
        }

        public DocumentObject(string value)
        {
            _string = value;
            Type = DocumentObjectType.String;
        }

        public DocumentObject(IList<DocumentObject> value)
        {
            _list = new Lazy<IList<DocumentObject>>(() => value, LazyThreadSafetyMode.PublicationOnly);
            Type = DocumentObjectType.List;
        }

        public DocumentObject(IDictionary<string, DocumentObject> value)
        {
            _dictionary = new Lazy<IDictionary<string, DocumentObject>>(() => value, LazyThreadSafetyMode.PublicationOnly);
            Type = DocumentObjectType.Dictionary;
        }

        public DocumentObject(Timestamp value)
        {
            _timestamp = value;
            Type = DocumentObjectType.Timestamp;
        }

        public DocumentObject(byte[] value)
        {
            _bytes = value;
            Type = DocumentObjectType.Bytes;
        }

        public DocumentObject(GeoPoint value)
        {
            _geoPoint = value;
            Type = DocumentObjectType.GeoPoint;
        }

        public DocumentObject(IDocumentReference value)
        {
            _documentReference = value;
            Type = DocumentObjectType.DocumentReference;
        }

        public DocumentObjectType Type { get; }

        private readonly bool _boolean;
        public bool Boolean => Type == DocumentObjectType.Boolean
            ? _boolean : throw new InvalidOperationException("Type is not Boolean.");

        private readonly long _int64;
        public long Int64 => Type == DocumentObjectType.Int64
            ? _int64 : throw new InvalidOperationException("Type is not Int64.");

        private readonly double _double;
        public double Double => Type == DocumentObjectType.Double
            ? _double : throw new InvalidOperationException("Type is not Double.");

        private readonly string? _string;
        public string String => Type == DocumentObjectType.String
            ? _string! : throw new InvalidOperationException("Type is not String.");

        private Lazy<IList<DocumentObject>>? _list;
        public IList<DocumentObject> List => Type == DocumentObjectType.List
            ? _list!.Value : throw new InvalidOperationException("Type is not List.");

        private Lazy<IDictionary<string, DocumentObject>>? _dictionary;
        public IDictionary<string, DocumentObject> Dictionary => Type == DocumentObjectType.Dictionary
            ? _dictionary!.Value : throw new InvalidOperationException("Type is not Dictionary.");

        private readonly Timestamp _timestamp;
        public Timestamp Timestamp => Type == DocumentObjectType.Timestamp
            ? _timestamp : throw new InvalidOperationException("Type is not Timestapm.");

        private readonly byte[]? _bytes;
        public byte[] Bytes => Type == DocumentObjectType.Bytes
            ? _bytes! : throw new InvalidOperationException("Type is not Bytes.");

        private readonly GeoPoint _geoPoint;
        public GeoPoint GeoPoint => Type == DocumentObjectType.GeoPoint
            ? _geoPoint : throw new InvalidOperationException("Type is not GeoPoint.");

        private readonly IDocumentReference? _documentReference;
        public IDocumentReference DocumentReference => Type == DocumentObjectType.DocumentReference
            ? _documentReference! : throw new InvalidOperationException("Type is not DocumentReference.");

        public object? Value => Type switch
        {
            DocumentObjectType.Null => null,
            DocumentObjectType.Boolean => Boolean,
            DocumentObjectType.Int64 => Int64,
            DocumentObjectType.Double => Double,
            DocumentObjectType.String => String,
            DocumentObjectType.List => List,
            DocumentObjectType.Dictionary => Dictionary,
            DocumentObjectType.Timestamp => Timestamp,
            DocumentObjectType.Bytes => Bytes,
            DocumentObjectType.GeoPoint => GeoPoint,
            DocumentObjectType.DocumentReference => DocumentReference,
            _ => throw new InvalidOperationException()
        };

        internal static DocumentObject CreateAsList(Func<IDocumentFieldInfo, object?> creator)
        {
            return new DocumentObject(creator, () =>
                (IList<DocumentObject>)creator(new DocumentFieldInfo<List<DocumentObject>>())!);
        }

        internal static DocumentObject CreateAsDictionary(Func<IDocumentFieldInfo, object?> creator)
        {
            return new DocumentObject(creator, () =>
                (IDictionary<string, DocumentObject>)creator(new DocumentFieldInfo<Dictionary<string, DocumentObject>>())!);
        }

        private DocumentObject(Func<IDocumentFieldInfo, object?> creator, Func<IList<DocumentObject>> factory)
        {
            _list = new Lazy<IList<DocumentObject>>(factory, LazyThreadSafetyMode.PublicationOnly);
            _creator = creator;
            Type = DocumentObjectType.List;
        }

        private DocumentObject(Func<IDocumentFieldInfo, object?> creator, Func<IDictionary<string, DocumentObject>> factory)
        {
            _dictionary = new Lazy<IDictionary<string, DocumentObject>>(factory, LazyThreadSafetyMode.PublicationOnly);
            _creator = creator;
            Type = DocumentObjectType.Dictionary;
        }

        internal object? GetFieldValue(IDocumentFieldInfo fieldInfo)
        {
            if (fieldInfo.ConvertFrom(this) is (true, var result))
            {
                return result;
            }

            var fieldType = fieldInfo.NullableUnderlyingType;

            return Type switch
            {
                DocumentObjectType.Null => null,
                DocumentObjectType.Boolean => Convert.ChangeType(Boolean, fieldType),
                DocumentObjectType.Int64 => Convert.ChangeType(Int64, fieldType),
                DocumentObjectType.Double => Convert.ChangeType(Double, fieldType),
                DocumentObjectType.String when fieldType == typeof(char) => string.IsNullOrEmpty(String) ? default : Convert.ToChar(String),
                DocumentObjectType.String => String,
                DocumentObjectType.List when _creator == null => List,
                DocumentObjectType.List => _creator(fieldInfo),
                DocumentObjectType.Dictionary when _creator == null => Dictionary,
                DocumentObjectType.Dictionary => _creator(fieldInfo),
                DocumentObjectType.Timestamp when fieldType == typeof(DateTime) => Timestamp.ToDateTime(),
                DocumentObjectType.Timestamp when fieldType == typeof(DateTimeOffset) => Timestamp.ToDateTimeOffset(),
                DocumentObjectType.Timestamp => Timestamp,
                DocumentObjectType.Bytes when fieldType == typeof(byte[]) => Bytes,
                DocumentObjectType.Bytes => new MemoryStream(Bytes),
                DocumentObjectType.GeoPoint => GeoPoint,
                DocumentObjectType.DocumentReference => DocumentReference,
                _ => throw new InvalidOperationException()
            };
        }
    }
}
