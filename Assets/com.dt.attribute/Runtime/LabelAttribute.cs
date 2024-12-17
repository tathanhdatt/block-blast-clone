using System;
using UnityEngine;

namespace Dt.Attribute
{
    [AttributeUsage(AttributeTargets.Field)]
    public class LabelAttribute : PropertyAttribute
    {
        public readonly string Label;

        public LabelAttribute(string label)
        {
            this.Label = label;
        }
    }
}