using BT;
using Muks.DataBind;
using UnityEngine;
using UnityEngine.UI;
using Muks.Tween;
using System;

public class UIPanda : MonoBehaviour
{
    #region UIPanda 위치 지정 관련 변수
    private Canvas _canvas;
    private RectTransform _rectTransform;
    private Vector2 _localPosition; // 변환된 canvas 내 좌표
    private Transform _uiPandaTransform;
    private RectTransform _rtUIPanda;
    #endregion

    private Button _cameraButton;
    private Button _giftButton;
    private bool _isGift;

    [SerializeField]
    private Sprite[] _stateSprite = new Sprite[5]; //상태 이미지

    [SerializeField]
    private StarterPanda _starterPanda;

    private void Awake()
    {
        _cameraButton = transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.GetComponent<Button>();
        _giftButton = transform.GetChild(2).gameObject.GetComponent<Button>();

        // uiPanda 판다 머리 위에 뜨도록 설정
        _canvas = GetComponentInParent<Canvas>();
        _rectTransform = _canvas.transform as RectTransform;
        _uiPandaTransform = _starterPanda.gameObject.transform.GetChild(1);
        _rtUIPanda = transform as RectTransform;

        UpdateUIPandaPosition();
    }

    private void OnEnable()
    {
        _starterPanda.StateHandler += StarterPanda_StateHandler;
        _starterPanda.UIAlphaHandler += StarterPanda_UIAlphaHandler;
        _starterPanda.ImageAlphaHandler += StarterPanda_ImageAlphaHandler;
        _starterPanda.GiftHandler += StarterPanda_GiftHandler;

        _cameraButton.onClick.AddListener(OnClickCameraButton);
        _giftButton.onClick.AddListener(OnClickGiftButton);

    }

    private void OnDisable()
    {
        _starterPanda.StateHandler -= StarterPanda_StateHandler;
        _starterPanda.UIAlphaHandler -= StarterPanda_UIAlphaHandler;
        _starterPanda.ImageAlphaHandler -= StarterPanda_ImageAlphaHandler;
        _starterPanda.GiftHandler -= StarterPanda_GiftHandler;
    }

    private void Update()
    {
        UpdateUIPandaPosition();
    }

    // 상태 이미지 변경
    private void StarterPanda_StateHandler(string dataID,int currentPandaState)
    {
        OnChangeStateImage(dataID, currentPandaState);
    }
    private void StarterPanda_UIAlphaHandler(float targetAlpha, float duration, Action onComplate = null)
    {
        OnChangePandaUIAlpha(targetAlpha, duration, onComplate);
    }
    private void StarterPanda_ImageAlphaHandler(GameObject gameObject, float targetAlpha, float duration, Action onComplate = null)
    {
        OnChangeAlpha(gameObject, targetAlpha, duration, onComplate);
    }
    private void StarterPanda_GiftHandler()
    {
        _giftButton.gameObject.SetActive(true);
        OnChangeAlpha(_giftButton.gameObject, 1, 1);
    }

    private void OnClickCameraButton()
    {
        // 카메라와 연동
    }
    private void OnClickGiftButton()
    {
        // 플레이어에게 선물 들어오는 기능 연결

        OnChangeAlpha(_giftButton.gameObject, 0, 1, () => _starterPanda.TakeAGift());
    }

    /// <summary>
    /// 현재 판다 상태로 이모티콘 변경
    /// </summary>
    private void OnChangeStateImage(string dataID, int currentPandaState)
    {
        //(수정) DataID 바꾸기
        DataBind.SetSpriteValue(dataID, _stateSprite[currentPandaState]);
    }

    /// <summary>
    /// 판다 UI Alpha 값 변경
    /// </summary>
    private void OnChangePandaUIAlpha(float targetAlpha, float duration, Action onComplate = null)
    {
        Tween.IamgeAlpha(gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject, targetAlpha, duration, TweenMode.Smoothstep);
        Tween.IamgeAlpha(_cameraButton.gameObject, targetAlpha, duration, TweenMode.Smoothstep);
        Tween.IamgeAlpha(_cameraButton.gameObject.transform.GetChild(0).gameObject, targetAlpha, duration, TweenMode.Smoothstep, onComplate);
    }

    private void OnChangeAlpha(GameObject gameObject, float targetAlpha, float duration, Action onComplate = null)
    {
        Tween.IamgeAlpha(gameObject, targetAlpha, duration, TweenMode.Smoothstep, onComplate);
    }

    private void UpdateUIPandaPosition()
    {
        Vector3 pandaScreenPos = Camera.main.WorldToScreenPoint(_uiPandaTransform.position);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTransform, pandaScreenPos, Camera.main, out _localPosition);
        _rtUIPanda.anchoredPosition = _localPosition;
    }
}
