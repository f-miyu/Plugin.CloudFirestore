﻿namespace Plugin.CloudFirestore
{
    public partial struct Timestamp
    {
        internal Timestamp(Google.Cloud.Firestore.Timestamp timestamp)
        {
            var offset = timestamp.ToProto();
            Seconds = offset.Seconds;
            Nanoseconds = offset.Nanos;
        }

        //internal Timestamp(DateTime date) : this(Google.Cloud.Firestore.Timestamp.FromDateTime(date))
        //{
        //}

        //internal Google.Cloud.Firestore.Timestamp ToNative()
        //{
        //    var date = new DateTime(0, 0, 0, Seconds, Nanoseconds);
        //    return Google.Cloud.Firestore.Timestamp.FromDateTime(date);
        //}
    }
}
