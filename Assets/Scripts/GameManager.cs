using NaughtyAttributes;
using RedBlueGames.Tools.TextTyper;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public enum Languages
    {
        HUH,
        FR,
        EN,
        SP
    }

    public static GameManager instance;

    [SerializeField] TextTyper _text;
    [SerializeField] AudioClip _typingSound ;

    [SerializeField]
    private Button printNextButton;

    private Queue<string> _dialogStrings = new Queue<string>();
    private Languages _language = Languages.HUH;

    public Languages Language { get => _language; set => _language = value; }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        this._text.PrintCompleted.AddListener(this.HandlePrintCompleted);
        //this._text.CharacterPrinted.AddListener(this.HandleCharacterPrinted); useless for now
        this.printNextButton.onClick.AddListener(this.HandlePrintNextClicked);
    }

    [Button("test dialog")]
    public void PlayDialog(string dialogName = "bruh")
    {
        GameDesignerDialogFriend gdfriend = null;
        GameDesignerDialogFriend[] list = Resources.FindObjectsOfTypeAll<GameDesignerDialogFriend>();

        foreach (var item in list)
        {
            if (item.name == dialogName)
            {
                gdfriend = item;
            }
        }


        if (gdfriend == null)
        {
            Debug.Log("dialog not found");
            return;
        }
        gdfriend.Load();

        foreach (var key in gdfriend.SentencesKEYS)
        {
            foreach (var row in gdfriend.rowList)
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

        PrintText();
    }

    private void HandlePrintNextClicked()
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
}
