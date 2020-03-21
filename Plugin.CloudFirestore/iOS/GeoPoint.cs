using System;
namespace Plugin.CloudFirestore
{
    public partial struct GeoPoint
    {
        internal GeoPoint(Firebase.CloudFirestore.GeoPoint geoPoint)
        {
            Latitude = geoPoint.Latitude;
            Longitude = geoPoint.Longitude;
        }

        internal Firebase.CloudFirestore.GeoPoint ToNative()
        {
            return new Firebase.CloudFirestore.GeoPoint(Latitude, Longitude);
        }
    }
}
