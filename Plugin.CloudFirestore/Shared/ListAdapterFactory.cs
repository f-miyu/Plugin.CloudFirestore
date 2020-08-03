using System;
using System.Collections;
using System.Collections.Generic;

namespace Plugin.CloudFirestore
{
    internal interface IListAdapterFactory
    {
        IListAdapter Create(object target);
    }

    internal class ListAdapterFactory<T> : IListAdapterFactory
    {
        public IListAdapter Create(object target)
        {
            return target switch
            {
                IList<T> list => new ListAdapter<T>(list),
                IReadOnlyList<T> list => new ListAdapter<T>(list),
                _ => throw new ArgumentOutOfRangeException(nameof(target))
            };
        }
    }

    internal class ListAdapterFactory : IListAdapterFactory
    {
        public IListAdapter Create(object target)
        {
            return target switch
            {
                IList list => new ListAdapter(list),
                _ => throw new ArgumentOutOfRangeException(nameof(target))
            };
        }
    }
}
