using System;
namespace Plugin.CloudFirestore
{
    public readonly partial struct Timestamp : IEquatable<Timestamp>, IComparable<Timestamp>
    {
        public Timestamp(long seconds, int nanoseconds)
        {
            Seconds = seconds;
            Nanoseconds = nanoseconds;
        }

        public Timestamp(DateTime dateTime)
        {
            var ms = (dateTime - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalMilliseconds;
            Seconds = (long)(ms / 1000);
            Nanoseconds = (int)(ms % 1000 * 1000000);
        }

        public Timestamp(DateTimeOffset dateTimeOffset)
        {
            var ms = (dateTimeOffset - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalMilliseconds;
            Seconds = (long)(ms / 1000);
            Nanoseconds = (int)(ms % 1000 * 1000000);
        }

        public long Seconds { get; }
        public int Nanoseconds { get; }

        public DateTime ToDateTime()
        {
            return new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero).AddMilliseconds(Seconds * 1000 + Nanoseconds / 1000000.0).UtcDateTime;
        }

        public DateTimeOffset ToDateTimeOffset()
        {
            return new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero).AddMilliseconds(Seconds * 1000 + Nanoseconds / 1000000.0);
        }

        public override bool Equals(object obj)
        {
            return obj is Timestamp timestamp && Equals(timestamp);
        }

        public bool Equals(Timestamp other)
        {
            return Seconds == other.Seconds && Nanoseconds == other.Nanoseconds;
        }

        public int CompareTo(Timestamp other)
        {
            if (Seconds == other.Seconds)
            {
                return Nanoseconds.CompareTo(other.Nanoseconds);
            }
            return Seconds.CompareTo(other.Seconds);
        }

        public override int GetHashCode()
        {
            return Seconds.GetHashCode() ^ Nanoseconds;
        }

        public static bool operator ==(Timestamp lhs, Timestamp rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(Timestamp lhs, Timestamp rhs)
        {
            return !lhs.Equals(rhs);
        }

        public static bool operator >(Timestamp lhs, Timestamp rhs)
        {
            return lhs.CompareTo(rhs) > 0;
        }

        public static bool operator <(Timestamp lhs, Timestamp rhs)
        {
            return lhs.CompareTo(rhs) < 0;
        }

        public static bool operator >=(Timestamp lhs, Timestamp rhs)
        {
            return lhs.CompareTo(rhs) >= 0;
        }

        public static bool operator <=(Timestamp lhs, Timestamp rhs)
        {
            return lhs.CompareTo(rhs) <= 0;
        }
    }
}
