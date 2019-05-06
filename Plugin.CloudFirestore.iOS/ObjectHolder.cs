using System;
using Foundation;
namespace Plugin.CloudFirestore
{
    internal class ObjectHolder : NSObject
    {
        public object Object { get; }

        public ObjectHolder(object @object)
        {
            Object = @object;
        }
    }

    internal class ObjectHolder<T> : NSObject
    {
        public T Object { get; }

        public ObjectHolder(T @object)
        {
            Object = @object;
        }
    }
}
