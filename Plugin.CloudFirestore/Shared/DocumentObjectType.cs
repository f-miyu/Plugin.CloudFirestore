using System;
namespace Plugin.CloudFirestore
{
    public enum DocumentObjectType
    {
        Null,
        Boolean,
        Int64,
        Double,
        String,
        List,
        Dictionary,
        [Obsolete("Use Timestamp instead")]
        Timestapm,
        Bytes,
        GeoPoint,
        DocumentReference,
        Timestamp = Timestapm
    }
}
