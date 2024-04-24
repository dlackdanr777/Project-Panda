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

        //��� ����
        _uiOutroScene.StartDialogue();

        yield return YieldCache.WaitForSeconds(1.5f);

        _uiOutroScene.SetDialogueNameText("����");
        _uiOutroScene.SetDialogueImage(_poyaImage);
        string context = "�̰��� �ٷ� �Ҿƹ����� Ž�� �繫��!! ";
        yield return StartCoroutine(_uiOutroScene.StartContext(context));


        _uiOutroScene.SetDialogueNameText("����");
        _uiOutroScene.SetDialogueImage(_jijiImage);
        context = "��û����...      \n��� �ű��ϰ� ���� ���� ���־�~~ ";
        yield return StartCoroutine(_uiOutroScene.StartContext(context));


        _uiOutroScene.SetDialogueNameText("����");
        _uiOutroScene.SetDialogueImage(_poyaImage);
        context = "�Ҿƹ����� �̰����� ��Ʈ�� '�ӽ�'�� �Բ� � ����̵� �ذ��ߴٰ� �߾�! ";
        yield return StartCoroutine(_uiOutroScene.StartContext(context));

        context = "�츮�� ���� �̰����� �����ϴ°ž�! ";
        yield return StartCoroutine(_uiOutroScene.StartContext(context));


        _uiOutroScene.SetDialogueNameText("����");
        _uiOutroScene.SetDialogueImage(_jijiImage);
        context = "����! �ٵ� �� �Ҹ��� �־� ";
        yield return StartCoroutine(_uiOutroScene.StartContext(context));


        _uiOutroScene.SetDialogueNameText("����");
        _uiOutroScene.SetDialogueImage(_poyaImage);
        context = "����? ";
        yield return StartCoroutine(_uiOutroScene.StartContext(context));


        _uiOutroScene.SetDialogueNameText("����");
        _uiOutroScene.SetDialogueImage(_jijiImage);
        context = "�װ� �ٷ�... ";
        yield return StartCoroutine(_uiOutroScene.StartContext(context, 35, 0.12f));

        context = "���� ���� ������� ���ڴ�!!! ";
        _uiOutroScene.ShakeDialogue(1.2f);
        yield return StartCoroutine(_uiOutroScene.StartContext(context));


        _uiOutroScene.SetDialogueNameText("����");
        _uiOutroScene.SetDialogueImage(_poyaImage);
        context = "�ƴ� �׷��� �����!!!!! ";
        yield return StartCoroutine(_uiOutroScene.StartContext(context));

        //��ȭ ����
        yield return YieldCache.WaitForSeconds(1);
        _uiOutroScene.EndDialogue();

        yield return YieldCache.WaitForSeconds(2);

        _uiOutroScene.StartFadeIn(3);
        yield return YieldCache.WaitForSeconds(5);

        //���丮1 �ƿ�Ʈ�� �Ϸ�
        DatabaseManager.Instance.UserInfo.SetStoryOutro(UserInfo.StoryOutroType.Story1);
        DatabaseManager.Instance.UserInfo.SaveUserInfoData(3);

        _uiOutroScene.StartEndText(2);

        yield return YieldCache.WaitForSeconds(8);

        _uiOutroScene.EndEndText(2);

        yield return YieldCache.WaitForSeconds(3);

        LoadingSceneManager.LoadScene("24_01_09_Integrated");
    }
}
