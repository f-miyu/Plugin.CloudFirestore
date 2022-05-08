using System;
using System.Runtime.InteropServices;
using ObjCRuntime;

namespace Plugin.CloudFirestore
{
    internal class Messaging
    {
        [DllImport(Constants.ObjectiveCLibrary, EntryPoint = "objc_msgSend")]
        public static extern IntPtr IntPtr_objc_msgSend_IntPtr_IntPtr(IntPtr receiver, IntPtr selector, IntPtr arg1, IntPtr arg2);
    }
}
