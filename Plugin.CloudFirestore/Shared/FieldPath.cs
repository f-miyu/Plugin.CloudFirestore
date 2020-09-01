using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Plugin.CloudFirestore
{
    public partial class FieldPath
    {
        private string[] _fieldNames;
        private bool _isDocumentId;

        public FieldPath(params string[] fieldNames)
        {
            _fieldNames = fieldNames;
        }

        public static FieldPath DocumentId
        {
            get
            {
                var fieldPath = new FieldPath();
                fieldPath._isDocumentId = true;
                return fieldPath;
            }
        }

        public static FieldPath CreateFrom<T, TMember>(Expression<Func<T, TMember>> expression)
        {
            var names = new List<string>();
            AddNames(expression.Body, names);

            return new FieldPath(names.ToArray());
        }

        private static void AddNames(Expression expression, List<string> names)
        {
            switch (expression)
            {
                case MemberExpression member:
                    AddNames(member.Expression, names);
                    names.Add(GetMappingName(member.Expression.Type, member.Member.Name));
                    break;
                case ParameterExpression _:
                    break;
                default:
                    throw new ArgumentException($"{nameof(expression)} must be MemberExpression or ParameterExpression.", nameof(expression));
            }
        }

        public static string GetMappingName<T>(string name)
        {
            return ObjectProvider.GetDocumentInfo<T>().GetMappingName(name);
        }

        public static string GetMappingName(Type type, string name)
        {
            return ObjectProvider.GetDocumentInfo(type).GetMappingName(name);
        }
    }
}
