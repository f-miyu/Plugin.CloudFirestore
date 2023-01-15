using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Plugin.CloudFirestore
{
    public static class Setup
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void Init(string AppName, string embeddedResourceFileName)
        {
            FirestoreProvider.AppName = AppName;
            if (embeddedResourceFileName == null) throw new ArgumentNullException(nameof(embeddedResourceFileName));

            Assembly assembly = Assembly.GetCallingAssembly();
            var resourceName = assembly.GetManifestResourceNames()
                .FirstOrDefault(x => x.EndsWith(embeddedResourceFileName));
            if (resourceName is null)
                throw new Exception($"Embedded resource '{embeddedResourceFileName}' not found at {assembly.FullName}");
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                FirestoreProvider.Credentials = Google.Apis.Auth.OAuth2.GoogleCredential.FromStream(stream);
            }
            FirestoreProvider.Firestore = new FirestoreWrapper(FirestoreProvider.GetFirestore());
        }
    }
}
