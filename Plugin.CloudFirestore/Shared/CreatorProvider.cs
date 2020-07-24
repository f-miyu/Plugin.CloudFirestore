using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace Plugin.CloudFirestore
{
    public static class CreatorProvider
    {
        private static ConcurrentDictionary<Type, Func<object>> _creators = new ConcurrentDictionary<Type, Func<object>>();

        public static Func<object> GetCreator(Type type)
        {
            return _creators.GetOrAdd(type, CreateCreator);
        }

        private static Func<object> CreateCreator(Type type)
        {
            try
            {
                return Expression.Lambda<Func<object>>(Expression.New(type)).Compile();
            }
            catch
            {
                return () => Activator.CreateInstance(type);
            }
        }
    }
}
