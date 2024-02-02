using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;
using ETouch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class TouchManager : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] float _touchRange;
    [SerializeField] float _touchedObjectScaleMultiplier;
    [SerializeField] float _touchedObjectScaleSpeed;
    [Space(10f)]
    [SerializeField] LayerMask _objectLayer;
    [SerializeField] MagnifyingLensCombinations _magnifyingLensCombinations;

    [Space(20f), Header("Materials")]
    [SerializeField] private Material _baseObjectMaterial;
    [SerializeField] private Material _selectedObjectMaterial;

    InteractableObject _selectedObject;
    private InteractableObject _lastSelectedObject;

    public bool isPreviewingObject = false;
    public bool shouldTriggerNextStep = false;
    private bool _isUsingLens = false;
    private bool _isDialogInProgress = false;
    private SpriteRenderer _lensSprite;    

    private List<GameObject> _currentCombination = new List<GameObject>();
    public List<Image> previewedImagesList = new List<Image>();

    public static TouchManager instance { get; private set; }
    public bool IsDialogInProgress { set => _isDialogInProgress = value; }

    private void OnEnable()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
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
        if (_isDialogInProgress)
        {
            GameManager.instance.HandlePrintNextClicked();
            return;
        }

        if (isPreviewingObject)
        {
            foreach (var item in previewedImagesList)
            {
                item.GetComponent<Image>().DOFade(0, 1.5f).OnComplete(() => CallNextStep());
            }
            previewedImagesList.Clear();
            isPreviewingObject = false;

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
                    CompletionManager.instance.NextStep();
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
                return;
            }
            
        }
        else
        {
            if (!isPreviewingObject)
            {
                switch (_selectedObject.gameObject.tag)
                {
                    case "Lens":
                        _lensSprite = _selectedObject.GetComponent<SpriteRenderer>();
                        _isUsingLens = true;
                        _lensSprite.color = Color.red;
                        break;
                    case "Help":
                        Debug.Log("This is the help menu");
                        GameManager.instance.PlayDialog("AIDE4",false);
                        break;
                    case "TIROIR":
                        GameManager.instance.PlayDialog("TIROIR");
                        _selectedObject.enabled = false;
                        break;
                    case "DECOR":
                        _selectedObject.CallDialog();
                        break;
                    case "COFFRE":
                        CompletionManager.instance.NextStep();
                        break;
                    case "ARMOIRE":
                        GameManager.instance.PlayDialog("ARMOIRE");
                        CompletionManager.instance.NextStep();
                        break;
                    case "BUREAU":
                        GameManager.instance.PlayDialog("BUREAU");
                        _selectedObject.enabled = false;
                        CompletionManager.instance.NextStep();
                        break;
                    case "TELEPHONE":
                        GameManager.instance.PlayDialog("TELEPHONE");
                        CompletionManager.instance.NextStep();
                        break;
                    default:
                        break;
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

    private void CallNextStep()
    {
        if (shouldTriggerNextStep)
        {
            CompletionManager.instance.NextStep();
            shouldTriggerNextStep = false;
        }
    }
}