using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class TouchManager : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] float _checkSize;
    [SerializeField] LayerMask _objectLayer;
    [SerializeField] MagnifyingLensCombinations _magnifyingLensCombinations;

    InteractableObject _selectedObject;

    private bool _isUsingLens = false;
    private SpriteRenderer _lensSprite;
    private bool _isPreviewingObject = false;

    private List<string> _currentCombination = new List<string>();

    public static TouchManager Instance { get; private set; }

    private void OnEnable()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        EnhancedTouchSupport.Enable();
        ETouch.onFingerDown += OnInputStarted;
        ETouch.onFingerUp += OnInputStopped;
    }

    private void OnDisable()
    {
        
    }

    private void OnInputStarted(Finger finger)
    {
        Vector2 fingerPosition = Camera.main.ScreenToWorldPoint(finger.currentTouch.screenPosition);
        Collider2D collider2D = GetClosestCollider(Physics2D.OverlapCircleAll(fingerPosition, _checkSize, _objectLayer), fingerPosition);
        if (collider2D == null) return;
        _selectedObject = collider2D.GetComponent<InteractableObject>();
        if (_isUsingLens)
        {
            if (_selectedObject.gameObject.CompareTag("Lens"))
            {
                _selectedObject.GetComponent<SpriteRenderer>().color = Color.white;
                _isUsingLens = false; _currentCombination.Clear(); 
                return;
            }

            _currentCombination.Add(_selectedObject.gameObject.tag);
            if (_currentCombination.Count == 2)
            {
                if (_magnifyingLensCombinations.CheckCombination(_currentCombination))
                {
                    Debug.Log("Combination found");
                }
                else
                {
                    Debug.Log("Combination not found");
                }
                _isUsingLens = false;
                _lensSprite.color = Color.white;
                _currentCombination.Clear();
            }
            
        }
        else
        {
            if (_selectedObject.gameObject.CompareTag("Lens"))
            {
                _lensSprite = _selectedObject.GetComponent<SpriteRenderer>();
                _isUsingLens = true;
                _lensSprite.color = Color.red;
            }
            else
            {
                _isPreviewingObject = true;
            }
        }


    }

    private void OnInputStopped(Finger finger)
    {
        
    }

    private Collider2D GetClosestCollider(Collider2D[] colliders, Vector2 position)
    {
        if (colliders is null || colliders.Length == 0) return null;
        int closestIndex = 0;
        for (int i = 1; i < colliders.Length; i++)
        {
            if (Vector2.Distance(colliders[i].transform.position, position) 
                < Vector2.Distance(colliders[closestIndex].transform.position, position))
            {
                closestIndex = i;
            }
        }
        return colliders[closestIndex];
    }
}