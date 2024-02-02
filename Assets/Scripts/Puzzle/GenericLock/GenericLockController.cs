using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
//using static UnityEditor.Progress;

public class GenericLockController : MonoBehaviour
{
    public List<Sprite> spriteCodes;
    public Sprite correctSprite;

    public List<Sprite> icons = new();

    public List<GenericLockSlot> slots = new();

    private int _code;
    private void Awake()
    {
        OnInit();

        foreach (var s in spriteCodes)
        {
            _code += s.GetHashCode();
        }
    }

    private void OnInit()
    {
        foreach (var slot in slots)
        {
            slot.Init(icons);
            slot.checkCode += OnCheckCode;
        }
    }

    private void OnCheckCode()
    {
        int value = 0;
        slots.ForEach(x => value += icons[x.Value].GetHashCode());


        if (_code == value)
        {
            foreach (var item in slots)
            {
                
                item.isCorrect = true;
            }
            StartCoroutine(CorrectCoroutine());
        }
    }

    private IEnumerator CorrectCoroutine()
    {
        yield return new WaitForSeconds(2.5f);
        foreach (var item in slots)
        {
            item._spriteRenderer.sprite = correctSprite;
        }
        yield return new WaitForSeconds(2.5f);
        CompletionManager.instance.NextStep();
    }
}
