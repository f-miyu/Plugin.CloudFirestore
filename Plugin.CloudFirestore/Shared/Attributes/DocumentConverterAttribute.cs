using System;
namespace Plugin.CloudFirestore.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class DocumentConverterAttribute : Attribute
    {
        public DocumentConverterAttribute(Type converterType)
        {
            ConverterType = converterType ?? throw new ArgumentNullException(nameof(converterType));
        }

        public DocumentConverterAttribute(Type converterType, params object?[] converterParameters) : this(converterType)
        {
            ConverterParameters = converterParameters;
        }

        public Type ConverterType { get; }
        public object?[]? ConverterParameters { get; }
    }
}
