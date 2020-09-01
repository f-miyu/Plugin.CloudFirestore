using System;
namespace Plugin.CloudFirestore.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class MapToAttribute : Attribute
    {
        public MapToAttribute(string mapping)
        {
            Mapping = mapping;
        }

        public string Mapping { get; }
    }
}
