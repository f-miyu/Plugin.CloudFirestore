using System;
using Foundation;
namespace Plugin.CloudFirestore
{
    public class ObjectHolder : NSObject
    {
        public object Object { get; }

        public ObjectHolder(object @object)
        {
            Object = @object;
        }
    }

    public class ObjectHolder<T> : NSObject
    {
        public T Object { get; }

        public ObjectHolder(T @object)
        {
            Object = @object;
        }
    }
}
