using System;
namespace Plugin.CloudFirestore.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class MapToAttribute : Attribute
    {
        public string Mapping { get; }

        public MapToAttribute(string mapping)
        {
            Mapping = mapping;
        }
    }
}
