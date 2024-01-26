using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using NaughtyAttributes;

[Serializable]
public class Dialog
{
    [Header("SPEAKER 1 IS THE FIRST ONE TO TALK , FROM THERE IT SWAPS BETWEEN EACH SENTENCE")]
    [Space(30)] 
    public string Speaker1;
    public string Speaker2;
    public string[] Lines;
}
