﻿using System;
using UnityEngine;

namespace Dt.Attribute
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ReadOnlyAttribute : PropertyAttribute
    {
    }
}