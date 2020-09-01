using System;
namespace Plugin.CloudFirestore
{
    internal class ObjectHolder : Java.Lang.Object
    {
        public ObjectHolder(object @object)
        {
            Object = @object;
        }

        public object Object { get; }
    }

    internal class ObjectHolder<T> : Java.Lang.Object
    {
        public ObjectHolder(T @object)
        {
            Object = @object;
        }

        public T Object { get; }
    }
}
