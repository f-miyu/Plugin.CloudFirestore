namespace Plugin.CloudFirestore
{
    public partial struct GeoPoint
    {
        internal GeoPoint(Google.Cloud.Firestore.GeoPoint geoPoint)
        {
            Latitude = geoPoint.Latitude;
            Longitude = geoPoint.Longitude;
        }

        internal Google.Cloud.Firestore.GeoPoint ToNative()
        {
            return new Google.Cloud.Firestore.GeoPoint(Latitude, Longitude);
        }
    }
}
