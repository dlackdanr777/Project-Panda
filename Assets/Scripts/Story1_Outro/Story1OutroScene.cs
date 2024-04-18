using Muks.Tween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class Story1OutroScene : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private UIOutroMiniDialogue _uiOutroScene;
    [SerializeField] private Light2D[] _ceilingLights;
    [SerializeField] private Light2D[] _floorLights;
    [SerializeField] private Light2D _gemLight;
    [SerializeField] private Light2D _gemOutlineLight;
    [SerializeField] private Animator _gemAnimator;

    [Space]
    [Header("PandaImages")]
    [SerializeField] private Sprite _poyaImage;
    [SerializeField] private Sprite _jijiImage;

    [Space]
    [Header("Audio Clips")]
    [SerializeField] private AudioClip _backgroundSound;



    private void Start()
    {
        Init();
        SoundManager.Instance.PlayBackgroundAudio(_backgroundSound, 1);
        StartCoroutine(StartOutro());
    }


    private void Init()
    {
        _gemAnimator.enabled = false;
        _uiOutroScene.Init();
    }


    private IEnumerator StartOutro()
    {
        yield return YieldCache.WaitForSeconds(3);

        Tween.Light2DIntensity(_gemLight.gameObject, 0.4f, 3);
        Tween.Light2DIntensity(_gemOutlineLight.gameObject, 0.2f, 3);

        for (int i = 0, count =  _ceilingLights.Length; i < count; i++)
        {
            Tween.Light2DIntensity(_ceilingLights[i].gameObject, 0.7f, 5);
        }

        for (int i = 0, count = _floorLights.Length; i < count; i++)
        {
            Tween.Light2DIntensity(_floorLights[i].gameObject, 0.7f, 5, TweenMode.EaseOutBounce);
        }

        yield return YieldCache.WaitForSeconds(6);
        _gemAnimator.enabled = true;

        //대사 시작
        _uiOutroScene.StartDialogue();

        yield return YieldCache.WaitForSeconds(1.5f);

        _uiOutroScene.SetDialogueNameText("포야");
        _uiOutroScene.SetDialogueImage(_poyaImage);
        string context = "이곳이 바로 할아버지의 탐정 사무소!! ";
        yield return StartCoroutine(_uiOutroScene.StartContext(context));


        _uiOutroScene.SetDialogueNameText("지지");
        _uiOutroScene.SetDialogueImage(_jijiImage);
        context = "엄청나다...      \n가운데 신기하게 생긴 돌이 떠있어~~ ";
        yield return StartCoroutine(_uiOutroScene.StartContext(context));


        _uiOutroScene.SetDialogueNameText("포야");
        _uiOutroScene.SetDialogueImage(_poyaImage);
        context = "할아버지는 이곳에서 파트너 '왓슨'과 함께 어떤 사건이든 해결했다고 했어! ";
        yield return StartCoroutine(_uiOutroScene.StartContext(context));

        context = "우리도 이제 이곳에서 시작하는거야! ";
        yield return StartCoroutine(_uiOutroScene.StartContext(context));


        _uiOutroScene.SetDialogueNameText("지지");
        _uiOutroScene.SetDialogueImage(_jijiImage);
        context = "좋아! 근데 나 할말이 있어 ";
        yield return StartCoroutine(_uiOutroScene.StartContext(context));


        _uiOutroScene.SetDialogueNameText("포야");
        _uiOutroScene.SetDialogueImage(_poyaImage);
        context = "뭔데? ";
        yield return StartCoroutine(_uiOutroScene.StartContext(context));


        _uiOutroScene.SetDialogueNameText("지지");
        _uiOutroScene.SetDialogueImage(_jijiImage);
        context = "그건 바로... ";
        yield return StartCoroutine(_uiOutroScene.StartContext(context, 35, 0.12f));

        context = "방은 먼저 고른사람이 임자다!!! ";
        _uiOutroScene.ShakeDialogue(1.2f);
        yield return StartCoroutine(_uiOutroScene.StartContext(context));


        _uiOutroScene.SetDialogueNameText("포야");
        _uiOutroScene.SetDialogueImage(_poyaImage);
        context = "아니 그런게 어딨어!!!!! ";
        yield return StartCoroutine(_uiOutroScene.StartContext(context));

        //대화 종료
        yield return YieldCache.WaitForSeconds(1);
        _uiOutroScene.EndDialogue();

        yield return YieldCache.WaitForSeconds(2);

        _uiOutroScene.StartFadeIn(3);
        yield return YieldCache.WaitForSeconds(5);

        //스토리1 아웃트로 완료
        DatabaseManager.Instance.UserInfo.SetStoryOutro(UserInfo.StoryOutroType.Story1);
        DatabaseManager.Instance.UserInfo.SaveUserInfoData(3);

        _uiOutroScene.StartEndText(2);

        yield return YieldCache.WaitForSeconds(8);

        _uiOutroScene.EndEndText(2);

        yield return YieldCache.WaitForSeconds(3);

        LoadingSceneManager.LoadScene("24_01_09_Integrated");
    }
}
