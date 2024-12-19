﻿using System;
using UnityEngine;

namespace Dt.Attribute
{
    [AttributeUsage(AttributeTargets.Field)]
    public class TitleAttribute : PropertyAttribute
    {
        public readonly string Title;

        public TitleAttribute(string title)
        {
            this.Title = title;
        }
    }
}