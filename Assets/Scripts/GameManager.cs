using NaughtyAttributes;
using RedBlueGames.Tools.TextTyper;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public enum SPEAKERS
    {
        MOM,
        DAD,
        GIRL,
        HIDDEN,
        NARRATOR
    }

    public enum Languages
    {
        FR,
        EN,
        SP
    }

    public static GameManager instance;

    [SerializeField] TextTyper _text;
    [SerializeField] AudioClip _typingSound ;
    [SerializeField] Image _speakerImage;

    [SerializeField] private Sprite _momSprite;
    [SerializeField] private Sprite _dadSprite;
    [SerializeField] private Sprite _girlSprite;
    private Queue<string> _dialogStrings = new Queue<string>();
    private Languages _language = Languages.FR;
    public Languages Language { get => _language; set => _language = value; }

    private bool _isTextActive = false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        this._text.PrintCompleted.AddListener(this.HandlePrintCompleted);
        //this._text.CharacterPrinted.AddListener(this.HandleCharacterPrinted); useless for now

        _text.gameObject.SetActive(false);


        //_momSprite = (Sprite)AssetDatabase.LoadAssetAtPath(" CORRECT PATH ", typeof(Sprite)); 
        //_dadSprite = (Sprite)AssetDatabase.LoadAssetAtPath(" CORRECT PATH ", typeof(Sprite));   <-- Add paths to correct speaker sprites when added to project
        //_girlSprite = (Sprite)AssetDatabase.LoadAssetAtPath(" CORRECT PATH ", typeof(Sprite));      temporary sprites added for debug purposes mom = unity, dad = git, girl = tree



    }

    [Button("test dialog")]
    public void PlayDialog(string dialogName = "swag")
    {
        DialogMaker dialogMaker = null;
        DialogMaker[] list = Resources.FindObjectsOfTypeAll<DialogMaker>();

        foreach (var item in list)
        {
            if (item.name == dialogName)
            {
                dialogMaker = item;
            }
        }


        if (dialogMaker == null)
        {
            Debug.Log("dialog not found");
            return;
        }
        dialogMaker.Load();



        switch (dialogMaker.Speaker)
        {
            case SPEAKERS.MOM:
                _speakerImage.sprite = _momSprite;
                break;
            case SPEAKERS.DAD:
                _speakerImage.sprite = _dadSprite;
                break;
            case SPEAKERS.GIRL:
                _speakerImage.sprite = _girlSprite;
                break;
            case SPEAKERS.HIDDEN:
                break;
            case SPEAKERS.NARRATOR:
                break;
            default:
                break;
        }

        foreach (var key in dialogMaker.SentencesKEYS)
        {
            foreach (var row in dialogMaker.rowList)
            {
                if (row.KEY == key)
                {
                    switch (_language)
                    {
                        case Languages.FR:
                            _dialogStrings.Enqueue(row.FR);
                            break;
                        case Languages.EN:
                            _dialogStrings.Enqueue(row.EN);
                            break;
                        case Languages.SP:
                            _dialogStrings.Enqueue(row.SP);
                            break;
                        default:
                            _dialogStrings.Enqueue("default string");
                            break;
                    }
                }
            }
        }

        TouchManager.instance.IsDialogInProgress = true;
        ShowDialog();
        PrintText();
    }

    public void HandlePrintNextClicked()
    {
        if (_text.IsSkippable() && _text.IsTyping)
        {
            _text.Skip();
        }
        else
        {
            PrintText();
        }
    }

    private void PrintText()
    {
        if (_dialogStrings.Count == 0)
        {
            if (_isTextActive)
            {
                HideDialog();
            }
            return;
        } 

        _text.TypeText(_dialogStrings.Dequeue());
    }

    private void HandleCharacterPrinted(string printedCharacter)
    {
        // Do not play a sound for whitespace
        if (printedCharacter == " " || printedCharacter == "\n")
        {
            return;
        }

        var audioSource = this.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = this.gameObject.AddComponent<AudioSource>();
        }

        audioSource.clip = this._typingSound;
        audioSource.Play();
    }

    private void HandlePrintCompleted()
    {
        Debug.Log("TypeText Complete");
    }

    private void ShowDialog()
    {
        _isTextActive = true;
        _text.gameObject.SetActive(true);
        _text.GetComponent<Transform>().DOMove(new Vector2(_text.GetComponent<Transform>().position.x, _text.GetComponent<Transform>().position.y + 630), 0.5f);
    }

    private void HideDialog()
    {
        _isTextActive = false;
        _text.GetComponent<Transform>().DOMove(new Vector2(_text.GetComponent<Transform>().position.x, _text.GetComponent<Transform>().position.y - 630), 0.5f);
        StartCoroutine(DissapearText());
    }

    private IEnumerator DissapearText()
    {
        yield return new WaitForSeconds(0.5f);
        _text.gameObject.SetActive(false);
        TouchManager.instance.IsDialogInProgress = false;
    }

}
