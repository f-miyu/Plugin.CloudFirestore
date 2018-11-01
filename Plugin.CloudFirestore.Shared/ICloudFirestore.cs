using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Plugin.CloudFirestore
{
    public interface ICloudFirestore
    {
        IInstance Instance { get; }
        IInstance GetInstance(string appName);
    }
}
