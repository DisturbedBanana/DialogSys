using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field)]
public class OptionsAttribute : PropertyAttribute
{
    public string OptionsName { get; set; }

    public OptionsAttribute(string OptionsName)
    {
        this.OptionsName = OptionsName;
    }
}


