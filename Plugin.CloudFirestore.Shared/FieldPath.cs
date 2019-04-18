using System;
namespace Plugin.CloudFirestore
{
    public partial class FieldPath
    {
        private string[] _fieldNames;
        private bool _isDocumentId;

        public static FieldPath DocumentId
        {
            get
            {
                var fieldPath = new FieldPath();
                fieldPath._isDocumentId = true;
                return fieldPath;
            }
        }

        public FieldPath(params string[] fieldNames)
        {
            _fieldNames = fieldNames;
        }
    }
}
