using System;
namespace Plugin.CloudFirestore
{
    public interface IFieldValue
    {
        object Delete { get; }
        object ServerTimestamp { get; }
    }
}
