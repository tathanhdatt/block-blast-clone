using System;
using UnityEngine;

namespace Dt.Attribute
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ShowIfAttribute : PropertyAttribute
    {
        public string Condition { get; }

        public object EnumValue { get; }

        public ShowIfAttribute(string condition)
        {
            this.Condition = condition;
        }

        public ShowIfAttribute(string condition, object enumValue)
        {
            this.Condition = condition;
            this.EnumValue = enumValue;
        }
    }
}