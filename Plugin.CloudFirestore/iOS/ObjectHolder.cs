using System;
using Foundation;
namespace Plugin.CloudFirestore
{
    internal class ObjectHolder : NSObject
    {
        public ObjectHolder(object @object)
        {
            Object = @object;
        }

        public object Object { get; }
    }

    internal class ObjectHolder<T> : NSObject
    {
        public ObjectHolder(T @object)
        {
            Object = @object;
        }

        public T Object { get; }
    }
}
