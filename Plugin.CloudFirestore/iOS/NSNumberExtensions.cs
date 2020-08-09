using System;
using Foundation;

namespace Plugin.CloudFirestore
{
    internal static class NSNumberExtensions
    {
        public static bool IsBoolean(this NSNumber number)
        {
            return number.IsEqualTo(new NSNumber(number.BoolValue));
        }

        public static bool IsInteger(this NSNumber number)
        {
            return number.IsEqualTo(new NSNumber(number.SByteValue)) ||
                number.IsEqualTo(new NSNumber(number.ByteValue)) ||
                number.IsEqualTo(new NSNumber(number.Int16Value)) ||
                number.IsEqualTo(new NSNumber(number.UInt16Value)) ||
                number.IsEqualTo(new NSNumber(number.Int32Value)) ||
                number.IsEqualTo(new NSNumber(number.UInt32Value)) ||
                number.IsEqualTo(new NSNumber(number.Int64Value)) ||
                number.IsEqualTo(new NSNumber(number.UInt64Value));
        }

        public static bool IsFloat(this NSNumber number)
        {
            return number.IsEqualTo(new NSNumber(number.FloatValue)) ||
                number.IsEqualTo(new NSNumber(number.DoubleValue));
        }
    }
}
