using System;
namespace Plugin.CloudFirestore
{
    public partial struct GeoPoint
    {
        internal GeoPoint(Firebase.Firestore.GeoPoint geoPoint)
        {
            Latitude = geoPoint.Latitude;
            Longitude = geoPoint.Longitude;
        }

        internal Firebase.Firestore.GeoPoint ToNative()
        {
            return new Firebase.Firestore.GeoPoint(Latitude, Longitude);
        }
    }
}
