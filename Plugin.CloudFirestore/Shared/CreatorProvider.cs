using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace Plugin.CloudFirestore
{
    internal static class CreatorProvider
    {
        private static class CreatorCache<T>
        {
            public static readonly Func<object> Instance = CreateCreator(typeof(T));

            private static Func<object> CreateCreator(Type type)
            {
                return Expression.Lambda<Func<object>>(Expression.Convert(Expression.New(type), typeof(object))).Compile();
            }
        }

        private static ConcurrentDictionary<Type, Func<object>> _creators = new ConcurrentDictionary<Type, Func<object>>();

        public static Func<object> GetCreator(Type type)
        {
            return _creators.GetOrAdd(type, GetCreatorCache);
        }

        public static Func<object> GetCreator<T>()
        {
            return CreatorCache<T>.Instance;
        }

        private static Func<object> GetCreatorCache(Type type)
        {
            return (Func<object>)typeof(CreatorCache<>).MakeGenericType(type)
                .GetField("Instance", BindingFlags.Public | BindingFlags.Static)
                .GetValue(null);
        }
    }

    internal static class CreatorProvider<TArgument>
    {
        private static class CreatorCache<T>
        {
            public static readonly Func<TArgument, object> Instance = CreateCreator(typeof(T));

            private static Func<TArgument, object> CreateCreator(Type type)
            {
                var constructor = type.GetConstructor(new[] { typeof(TArgument) });
                var args = Expression.Parameter(typeof(TArgument), "args");
                return Expression.Lambda<Func<TArgument, object>>(Expression.Convert(Expression.New(constructor, args), typeof(object)), args).Compile();
            }
        }

        private static ConcurrentDictionary<Type, Func<TArgument, object>> _creators = new ConcurrentDictionary<Type, Func<TArgument, object>>();

        public static Func<TArgument, object> GetCreator(Type type)
        {
            return _creators.GetOrAdd(type, GetCreatorCache);
        }

        public static Func<TArgument, object> GetCreator<T>()
        {
            return CreatorCache<T>.Instance;
        }

        private static Func<TArgument, object> GetCreatorCache(Type type)
        {
            return (Func<TArgument, object>)typeof(CreatorCache<>).MakeGenericType(type)
                .GetField("Instance", BindingFlags.Public | BindingFlags.Static)
                .GetValue(null);
        }
    }
}
