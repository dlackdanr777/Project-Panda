using BT;
using Muks.DataBind;
using UnityEngine;
using UnityEngine.UI;
using Muks.Tween;
using System;

public class UIPanda : MonoBehaviour
{
    private Button _stateButton;
    private Button _cameraButton;

    [SerializeField]
    public Sprite[] _stateSprite = new Sprite[5]; //상태 이미지

    [SerializeField]
    private StarterPanda _starterPanda;

    private void Awake()
    {
        _stateButton = transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<Button>();
        _cameraButton = transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.GetComponent<Button>();
    }

    private void OnEnable()
    {
        _starterPanda.StateHandler += StarterPanda_StateHandler;
        _starterPanda.UIAlphaHandler += StarterPanda_UIAlphaHandler;
        _starterPanda.AlphaImageHandler += StarterPanda_AlphaImageHandler;

        _stateButton.onClick.AddListener(OnClickStateButton);
        _cameraButton.onClick.AddListener(OnClickCameraButton);
        //ChangePandaUIAlpha(1, 1);

    }
    private void OnDisable()
    {
        _starterPanda.StateHandler -= StarterPanda_StateHandler;
        _starterPanda.UIAlphaHandler -= StarterPanda_UIAlphaHandler;
        _starterPanda.AlphaImageHandler -= StarterPanda_AlphaImageHandler;
    }

    // 상태 이미지 변경
    private void StarterPanda_StateHandler(int currentPandaState)
    {
        OnChangeStateImage(currentPandaState);
    }
    private void StarterPanda_UIAlphaHandler(float targetAlpha, float duration, Action onComplate = null)
    {
        OnChangePandaUIAlpha(targetAlpha, duration, onComplate);
    }
    private void StarterPanda_AlphaImageHandler(GameObject gameObject, float targetAlpha, float duration, Action onComplate = null)
    {
        OnChangeAlpha(gameObject, targetAlpha, duration, onComplate);
    }

    private void OnClickStateButton()
    {
        // 상태창 표시 추가
        Debug.Log("상태창 표시");
    }
    private void OnClickCameraButton()
    {
        // 카메라와 연동
        Debug.Log("카메라 실행");
    }

    /// <summary>
    /// 현재 판다 상태로 이모티콘 변경
    /// </summary>
    private void OnChangeStateImage(int currentPandaState)
    {
        //(수정) DataID 바꾸기
        DataBind.SetSpriteValue("941", _stateSprite[currentPandaState]);
    }

    /// <summary>
    /// 판다 UI Alpha 값 변경
    /// </summary>
    private void OnChangePandaUIAlpha(float targetAlpha, float duration, Action onComplate = null)
    {
        Tween.IamgeAlpha(_stateButton.gameObject, targetAlpha, duration, TweenMode.Smoothstep);
        Tween.IamgeAlpha(_stateButton.gameObject.transform.GetChild(0).gameObject, targetAlpha, duration, TweenMode.Smoothstep);
        Tween.IamgeAlpha(_cameraButton.gameObject, targetAlpha, duration, TweenMode.Smoothstep);
        Tween.IamgeAlpha(_cameraButton.gameObject.transform.GetChild(0).gameObject, targetAlpha, duration, TweenMode.Smoothstep, onComplate);
    }

    private void OnChangeAlpha(GameObject gameObject, float targetAlpha, float duration, Action onComplate = null)
    {
        Tween.IamgeAlpha(this.gameObject.transform.GetChild(1).gameObject, targetAlpha, duration, TweenMode.Smoothstep, onComplate);
    }
}
