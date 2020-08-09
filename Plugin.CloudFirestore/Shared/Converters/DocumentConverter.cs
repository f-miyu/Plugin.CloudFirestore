using System;

namespace Plugin.CloudFirestore.Converters
{
    public abstract class DocumentConverter
    {
        public Type TargetType { get; }

        public DocumentConverter(Type targetType)
        {
            TargetType = targetType ?? throw new ArgumentNullException(nameof(targetType));
        }

        public abstract (bool IsConverted, object Result) ConvertTo(object value);
        public abstract (bool IsConverted, object Result) ConvertFrom(DocumentObject value);
    }

    public abstract class DocumentConverter<T1> : DocumentConverter
    {
        public DocumentConverter(Type targetType, T1 arg1) : base(targetType)
        {
        }
    }

    public abstract class DocumentConverter<T1, T2> : DocumentConverter
    {
        public DocumentConverter(Type targetType, T1 arg1, T2 arg2) : base(targetType)
        {
        }
    }

    public abstract class DocumentConverter<T1, T2, T3> : DocumentConverter
    {
        public DocumentConverter(Type targetType, T1 arg1, T2 arg2, T3 arg3) : base(targetType)
        {
        }
    }

    public abstract class DocumentConverter<T1, T2, T3, T4> : DocumentConverter
    {
        public DocumentConverter(Type targetType, T1 arg1, T2 arg2, T3 arg3, T4 arg4) : base(targetType)
        {
        }
    }

    public abstract class DocumentConverter<T1, T2, T3, T4, T5> : DocumentConverter
    {
        public DocumentConverter(Type targetType, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) : base(targetType)
        {
        }
    }

    public abstract class DocumentConverter<T1, T2, T3, T4, T5, T6> : DocumentConverter
    {
        public DocumentConverter(Type targetType, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) : base(targetType)
        {
        }
    }
}

