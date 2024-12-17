using System.Collections.Generic;

namespace Dt.Extension
{
    public static class TypeExtension
    {
        public static bool IsList(this System.Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>);
        }
    }
}