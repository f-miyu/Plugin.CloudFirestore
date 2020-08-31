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
                ICollection<T> collection => new ListAdapter<T>(collection),
                IEnumerable<T> enumerable => new ListAdapter<T>(enumerable),
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
                IEnumerable enumerable => new ListAdapter(enumerable),
                _ => throw new ArgumentOutOfRangeException(nameof(target))
            };
        }
    }
}
