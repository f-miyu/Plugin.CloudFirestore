using System;
using Foundation;

namespace Plugin.CloudFirestore
{
    public partial struct Timestamp
    {
        internal Timestamp(Firebase.CloudFirestore.Timestamp timestamp)
        {
            Seconds = timestamp.Seconds;
            Nanoseconds = timestamp.Nanoseconds;
        }

        internal Timestamp(NSDate date) : this(Firebase.CloudFirestore.Timestamp.Create(date))
        {
        }

        internal Firebase.CloudFirestore.Timestamp ToNative()
        {
            return new Firebase.CloudFirestore.Timestamp(Seconds, Nanoseconds);
        }
    }
}
