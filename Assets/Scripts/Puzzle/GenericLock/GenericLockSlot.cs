using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenericLockSlot : MonoBehaviour
{
    public int Value { get; set; }

    public event Action checkCode;
    
    public Image _spriteRenderer;
    private List<Sprite> _icons = new();
    public bool isCorrect = false;
    
    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<Image>();
    }
    
    public void Init(List<Sprite> icons)
    {
        _icons = icons;
        Value = UnityEngine.Random.Range(0, _icons.Count);

        RefreshIcon();
    }

    public void OnClickUp() => OnClick(true);
    public void OnClickDown() => OnClick(false);
    
    private void OnClick(bool value)
    {
        if (!isCorrect)
        {
            Value = value ? Value + 1 : Value - 1;

            Value = Value >= _icons.Count ? 0 : Value < -1 ? _icons.Count - 1 : Value;

            RefreshIcon();

            checkCode.Invoke();
        }
    }
    
    private void RefreshIcon()
    {
        _spriteRenderer.sprite = _icons[Value];
    }
}
