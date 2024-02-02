using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CompletionManager : MonoBehaviour
{
    public static CompletionManager instance;
    [SerializeField] Image _blackScreen;
    [SerializeField] Image _dollHouseLetter;
    [SerializeField] Canvas _boxCanvas;
    [SerializeField] GameObject _box;
    private int step = 0;
    private Vector2 _middlePreviewPosition = Vector2.zero;
    TouchManager _touch;
    GameManager _gameManager;

    [SerializeField] GameObject _youngRoom;
    [SerializeField] GameObject _oldRoom;

    [SerializeField] GameObject _cassette;
    [SerializeField] GameObject _horloge;
    [SerializeField] Transform _cassetteTarget;
    [SerializeField] Transform _horlogeTarget;
    [SerializeField] Transform _letterTarget;
    [SerializeField] GameObject _invitationLetter;
    [SerializeField] Image _diary;
    [SerializeField] Image _phoneLocked;
    [SerializeField] Image _phoneUnlocked;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        _boxCanvas.gameObject.SetActive(false);
        _touch = TouchManager.instance;
        _gameManager = GameManager.instance;
    }

    public void NextStep()
    {
        Debug.Log("START");
        switch (step)
        {
            case 0: //START
                
                _gameManager._titleScreen.DOColor(Color.clear, 1f).OnComplete(() => _gameManager.PlayDialog("INTRO"));
                _gameManager.startLangButtons.SetActive(false);
                _gameManager.gameLangButtons.SetActive(true);
                break;
            case 1: //Fade in scene
                _blackScreen.DOColor(Color.white, 3f).SetEase(Ease.InOutSine); //fade white
                StartCoroutine(FadeOut());
                break;
            case 2: //Play start dialog
                _gameManager.PlayDialog("APPA", false);
                break;
            case 3: //Appear tiroir letter
                _dollHouseLetter.gameObject.SetActive(true);
                Transform letterTransform = _dollHouseLetter.GetComponent<Transform>();
                letterTransform.DOScale(letterTransform.localScale * 6, 1.5f);
                _touch.previewedImagesList.Add(_dollHouseLetter);
                _touch.shouldTriggerNextStep = true;
                _touch.isPreviewingObject = true;
                break;
            case 4:
                _gameManager.PlayDialog("BURO", false);
                _box.GetComponent<Collider2D>().enabled = true;
                break;
            case 5:
                _gameManager.PlayDialog("COFFRE");
                break;
            case 6:
                _boxCanvas.gameObject.SetActive(true);
                break;
            case 7:
                _boxCanvas.gameObject.SetActive(false);
                StartCoroutine(AppearCassette());
                break;
            case 8:
                _horloge.GetComponent<Transform>().DOScale(_horloge.GetComponent<Transform>().localScale / 2f, 1.5f).SetEase(Ease.InOutSine);
                _horloge.GetComponent<Transform>().DOScale(_horloge.GetComponent<Transform>().localScale / 2f, 1.5f).SetEase(Ease.InOutSine);
                _horloge.GetComponent<Transform>().DOMove(_horlogeTarget.position, 1.5f);
                _cassette.GetComponent<Transform>().DOScale(_cassette.GetComponent<Transform>().localScale / 4.5f, 1.5f).SetEase(Ease.InOutSine);
                _cassette.GetComponent<Transform>().DOMove(_cassetteTarget.position, 1.5f);
                break;
            case 9: //switch rooms
                _blackScreen.DOColor(Color.black, 3f).SetEase(Ease.InOutSine).OnComplete(()=>SwitchRooms());
                break;
            case 10:
                _gameManager.PlayDialog("FUTUR",false);
                break;
            case 11:
                _invitationLetter.SetActive(true);
                _invitationLetter.GetComponent<Transform>().DOScale(_invitationLetter.GetComponent<Transform>().localScale * 5, 1.5f).SetEase(Ease.InOutSine);
                break;
            case 12:
                _invitationLetter.GetComponent<Transform>().DOMove(_letterTarget.position, 1.5f).SetEase(Ease.InOutSine);
                _invitationLetter.GetComponent<Transform>().DOScale(_invitationLetter.GetComponent<Transform>().localScale / 5, 1.5f).SetEase(Ease.InOutSine);
                break;
            case 13:
                _diary.gameObject.SetActive(true);
                _diary.GetComponent<Transform>().DOScale(_diary.GetComponent<Transform>().localScale * 5, 1.5f).SetEase(Ease.InOutSine);
                break;
            case 14:
                _diary.DOFade(0, 1.5f).SetEase(Ease.InOutSine);
                break;
            case 15:
                _phoneLocked.gameObject.SetActive(true);
                _phoneLocked.GetComponent<Transform>().DOScale(_phoneLocked.GetComponent<Transform>().localScale * 5, 1.5f).SetEase(Ease.InOutSine);
                _phoneUnlocked.gameObject.SetActive(true);
                _phoneUnlocked.GetComponent<Transform>().DOScale(_phoneUnlocked.GetComponent<Transform>().localScale * 5, 1.5f).SetEase(Ease.InOutSine);
                StartCoroutine(FadeOutPhone());
                break;
            case 16:
                _phoneUnlocked.DOFade(0, 1.5f).SetEase(Ease.InOutSine);
                break;
            case 17:
                _gameManager.PlayDialog("FIN1");
                break;
            case 18:
                _gameManager.PlayDialog("FIN2");
                break;
            case 19:
                _gameManager.PlayDialog("FIN3");
                break;
            case 20:
                _gameManager.PlayDialog("FIN4");
                break;
            case 21:
                _gameManager.PlayDialog("FIN5");
                break;
            case 22:
                _blackScreen.DOColor(Color.black, 5f).SetEase(Ease.InOutSine).OnComplete(() => _gameManager.LoadEnd());
                break;
            default:

                break;
        }
        step++;
    }

    private IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(3f);
        _blackScreen.DOColor(Color.clear, 2f).SetEase(Ease.InOutSine).OnComplete(() => NextStep());
    }

    private IEnumerator FadeOutPhone()
    {
        yield return new WaitForSeconds(3f);
        _phoneLocked.DOFade(0, 1.5f).SetEase(Ease.InOutSine);
    }

    private void SwitchRooms()
    {
        _youngRoom.SetActive(false);
        _oldRoom.SetActive(true);
        StartCoroutine(FadeOut());
    }

    private IEnumerator AppearCassette()
    {
        yield return new WaitForSeconds(1f);
        _gameManager.PlayDialog("COFFRE1");
        _horloge.gameObject.SetActive(true);
        _cassette.gameObject.SetActive(true);
        _cassette.GetComponent<Transform>().DOScale(_cassette.GetComponent<Transform>().localScale * 4.5f, 1.5f).SetEase(Ease.InOutSine);
        _horloge.GetComponent<Transform>().DOScale(_horloge.GetComponent<Transform>().localScale * 4.5f, 1.5f).SetEase(Ease.InOutSine);
    }
}
