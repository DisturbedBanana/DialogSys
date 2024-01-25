using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "Data", menuName = "Scripts/ScriptableObjects" , order = 1)]
public class MagnifyingLensCombinations : ScriptableObject
{
    public List<LensCombination> CombinationsList = new List<LensCombination>();

    public bool CheckCombination(List<string> currentCombination)
    {
        if (currentCombination[0] == currentCombination[1])
        {
            Debug.Log("Can't combine the same item");
            return false;
        }

        foreach (LensCombination combination in CombinationsList)
        {
            if (combination.ItemTags.Count != currentCombination.Count) continue;
            bool isCombination = true;
            for (int i = 0; i < combination.ItemTags.Count; i++)
            {
                if (!combination.ItemTags.Contains(currentCombination[i]))
                {
                    isCombination = false;
                    break;
                }
            }
            if (isCombination) return true;
        }
        return false;
    }
}
