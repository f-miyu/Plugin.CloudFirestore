using System;
using System.Collections;

namespace Plugin.CloudFirestore
{
    public partial struct GeoPoint : IEquatable<GeoPoint>, IComparable<GeoPoint>
    {
        public double Latitude { get; }
        public double Longitude { get; }

        public GeoPoint(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public override bool Equals(object obj)
        {
            return obj is GeoPoint geoPoint ? Equals(geoPoint) : false;
        }

        public bool Equals(GeoPoint other)
        {
            return Latitude == other.Latitude && Longitude == other.Longitude;
        }

        public int CompareTo(GeoPoint other)
        {
            var comparison = Latitude.CompareTo(other.Latitude);
            if (comparison == 0)
            {
                return Longitude.CompareTo(other.Longitude);
            }
            return comparison;
        }

        public override int GetHashCode()
        {
            return Latitude.GetHashCode() ^ Longitude.GetHashCode();
        }

        public static bool operator ==(GeoPoint lhs, GeoPoint rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(GeoPoint lhs, GeoPoint rhs)
        {
            return !lhs.Equals(rhs);
        }

        public static bool operator >(GeoPoint lhs, GeoPoint rhs)
        {
            return lhs.CompareTo(rhs) > 0;
        }

        public static bool operator <(GeoPoint lhs, GeoPoint rhs)
        {
            return lhs.CompareTo(rhs) < 0;
        }

        public static bool operator >=(GeoPoint lhs, GeoPoint rhs)
        {
            return lhs.CompareTo(rhs) >= 0;
        }

        public static bool operator <=(GeoPoint lhs, GeoPoint rhs)
        {
            return lhs.CompareTo(rhs) <= 0;
        }
    }
}
