using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "Data", menuName = "Scripts/ScriptableObjects" , order = 1)]
public class MagnifyingLensCombinations : ScriptableObject
{
    public List<LensCombination> CombinationsList = new List<LensCombination>();
}
