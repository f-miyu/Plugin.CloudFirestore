using System;
using System.Collections.Generic;

namespace Plugin.CloudFirestore
{
    internal partial interface IDocumentInfo
    {
        string GetMappingName(string name);
    }
}
