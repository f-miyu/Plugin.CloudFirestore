using System;
namespace Plugin.CloudFirestore.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ServerTimestampAttribute : Attribute
    {
        public bool CanReplace { get; set; } = true;
    }
}
