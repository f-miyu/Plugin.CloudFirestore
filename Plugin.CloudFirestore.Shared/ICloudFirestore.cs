using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Plugin.CloudFirestore
{
    public interface ICloudFirestore
    {
        IFirestore Instance { get; }
        IFieldValue FieldValue { get; }
        IFirestore GetInstance(string appName);
    }
}
