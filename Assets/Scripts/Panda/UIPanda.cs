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

    [SerializeField] private Button _cameraButton;
    [SerializeField] private Button _giftButton;
    private bool _isGift;

    [SerializeField]
    private Sprite[] _stateSprite = new Sprite[5]; //상태 이미지

    private Panda _panda;

    private bool _isStart;


    public void Init(Panda panda)
    {
        _panda = panda;

        // uiPanda 판다 머리 위에 뜨도록 설정
        _canvas = GetComponentInParent<Canvas>();
        _rectTransform = _canvas.transform as RectTransform;
        _uiPandaTransform = _panda.gameObject.transform.GetChild(1);
        _rtUIPanda = transform as RectTransform;

        UpdateUIPandaPosition();


        _panda.StateHandler += StarterPanda_StateHandler;
        _panda.UIAlphaHandler += StarterPanda_UIAlphaHandler;
        _panda.ImageAlphaHandler += StarterPanda_ImageAlphaHandler;
        _panda.GiftHandler += StarterPanda_GiftHandler;

        _cameraButton.onClick.AddListener(OnClickCameraButton);
        _giftButton.onClick.AddListener(OnClickGiftButton);

        _isStart = true;
    }

    private void OnDestroy()
    {
        _panda.StateHandler -= StarterPanda_StateHandler;
        _panda.UIAlphaHandler -= StarterPanda_UIAlphaHandler;
        _panda.ImageAlphaHandler -= StarterPanda_ImageAlphaHandler;
        _panda.GiftHandler -= StarterPanda_GiftHandler;
    }

    private void Update()
    {
        if (!_isStart)
            return;

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

        OnChangeAlpha(_giftButton.gameObject, 0, 1, () => _panda.TakeAGift());
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
        //Tween.IamgeAlpha(_cameraButton.gameObject, targetAlpha, duration, TweenMode.Smoothstep);
        Tween.IamgeAlpha(_cameraButton.gameObject.transform.GetChild(0).gameObject, targetAlpha, duration, TweenMode.Smoothstep, onComplate);
    }

    private void OnChangeAlpha(GameObject gameObject, float targetAlpha, float duration, Action onComplate = null)
    {
        Tween.IamgeAlpha(gameObject, targetAlpha, duration, TweenMode.Smoothstep, onComplate);
    }

    private void UpdateUIPandaPosition()
    {
        Vector3 pandaScreenPos = Camera.main.WorldToScreenPoint(_uiPandaTransform.position + Vector3.up);
        transform.position = pandaScreenPos;
        //RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTransform, pandaScreenPos, Camera.main, out _localPosition);
        //_rtUIPanda.anchoredPosition = _localPosition;
    }
}
