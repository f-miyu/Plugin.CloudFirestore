using System;

namespace Plugin.CloudFirestore
{
    public partial struct Timestamp
    {
        internal Timestamp(Firebase.Timestamp timestamp)
        {
            Seconds = timestamp.Seconds;
            Nanoseconds = timestamp.Nanoseconds;
        }

        internal Timestamp(Java.Util.Date date) : this(new Firebase.Timestamp(date))
        {
        }

        internal Firebase.Timestamp ToNative()
        {
            return new Firebase.Timestamp(Seconds, Nanoseconds);
        }
    }
}
