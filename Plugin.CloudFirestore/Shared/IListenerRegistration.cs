using System;
namespace Plugin.CloudFirestore
{
    public interface IListenerRegistration : IDisposable
    {
        void Remove();
    }
}
