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

    InteractableObject _selectedObject;

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
        Debug.Log("Selected object: " + _selectedObject.name);
        _selectedObject.GetComponent<SpriteRenderer>().color = Color.red;
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
