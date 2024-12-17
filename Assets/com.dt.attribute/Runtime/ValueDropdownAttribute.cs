using UnityEngine;

namespace Dt.Attribute
{
    public class ValueDropdownAttribute : PropertyAttribute
    {
        public string Values { get; }

        public ValueDropdownAttribute(string values)
        {
            this.Values = values;
        }
    }
}