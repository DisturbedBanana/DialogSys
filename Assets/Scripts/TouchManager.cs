using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class TouchManager : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] float _touchRange;
    [SerializeField] float _touchedObjectScaleMultiplier;
    [Space(10f)]
    [SerializeField] LayerMask _objectLayer;
    [SerializeField] MagnifyingLensCombinations _magnifyingLensCombinations;

    [Space(20f), Header("Materials")]
    [SerializeField] private Material _baseObjectMaterial;
    [SerializeField] private Material _selectedObjectMaterial;

    InteractableObject _selectedObject;
    private InteractableObject _lastSelectedObject;

    private bool _isUsingLens = false;
    private SpriteRenderer _lensSprite;
    private bool _isPreviewingObject = false;
    private Vector2 _basePosition;
    private Vector2 _baseScale;
    private Vector2 _previewPosition = new Vector2(0, 0);

    private List<GameObject> _currentCombination = new List<GameObject>();

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
        DOTween.Init();
    }

    private void OnDisable()
    {
        
    }

    private void OnInputStarted(Finger finger)
    {
        if (_isPreviewingObject)
        {
            _lastSelectedObject.transform.DOMove(_basePosition, 0.5f);
            _lastSelectedObject.transform.DOScale(_baseScale, 0.5f);
            _isPreviewingObject = false;
            return;
        }
        
        Vector2 fingerPosition = Camera.main.ScreenToWorldPoint(finger.currentTouch.screenPosition);
        Collider2D collider2D = GetClosestCollider(Physics2D.OverlapCircleAll(fingerPosition, _touchRange, _objectLayer), fingerPosition);
        
        if (collider2D == null) return; 
        
        _selectedObject = collider2D.GetComponent<InteractableObject>();
        SpriteRenderer _selectedObjectRenderer = _selectedObject.GetComponent<SpriteRenderer>();
        if (_isUsingLens)
        {
            if (_selectedObject.gameObject.CompareTag("Lens"))
            {
                _selectedObject.GetComponent<SpriteRenderer>().color = Color.white;
                _isUsingLens = false; _currentCombination.Clear(); 
                return;
            }

            _currentCombination.Add(_selectedObject.gameObject);
            _selectedObjectRenderer.material = _selectedObjectMaterial;
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
                foreach (var item in _currentCombination)
                {
                    item.GetComponent<SpriteRenderer>().material = _baseObjectMaterial;
                }
                _currentCombination.Clear();
            }
            
        }
        else
        {
            if (!_isPreviewingObject)
            {
                if (_selectedObject.gameObject.CompareTag("Lens"))
                {
                    _lensSprite = _selectedObject.GetComponent<SpriteRenderer>();
                    _isUsingLens = true;
                    _lensSprite.color = Color.red;
                }
                
                else
                {
                    _basePosition = _selectedObject.transform.position;
                    _selectedObject.GetComponent<Transform>().DOMove(_previewPosition, 2f);
                    _selectedObject.GetComponent<Transform>().DOScale(_touchedObjectScaleMultiplier, 2f);
                    _lastSelectedObject = _selectedObject;
                    _baseScale = _selectedObject.transform.localScale;
                    _isPreviewingObject = true;
                }
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