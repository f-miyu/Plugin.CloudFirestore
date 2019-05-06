using System;
namespace Plugin.CloudFirestore
{
    internal class ObjectHolder : Java.Lang.Object
    {
        public object Object { get; }

        public ObjectHolder(object @object)
        {
            Object = @object;
        }
    }

    internal class ObjectHolder<T> : Java.Lang.Object
    {
        public T Object { get; }

        public ObjectHolder(T @object)
        {
            Object = @object;
        }
    }
}
