using Muks.DataBind;
using Muks.Tween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroScene : MonoBehaviour
{
    [SerializeField] private GameObject _flashImage;

    private Animator _flashAnimator;

    [SerializeField] private UIIntroScene _uiIntroScene;


    private void Start()
    {
        _flashAnimator = _flashImage.GetComponent<Animator>();
        _uiIntroScene.Init();
        StartCoroutine(Scene1());

        FadeInOutManager.Instance.FadeIn(0.01f);
    }

    private IEnumerator Scene1()
    {
        //���̵� �ƿ�
        yield return YieldCache.WaitForSeconds(1f);

        FadeInOutManager.Instance.FadeOut(2, 0);

        yield return YieldCache.WaitForSeconds(3f);

        _flashAnimator.SetTrigger("Flash");

        yield return YieldCache.WaitForSeconds(3);

        _uiIntroScene.StartDialogue(() => StartCoroutine(StartContexts1()));
    }


    private IEnumerator StartContexts1()
    {
        yield return YieldCache.WaitForSeconds(1);

        char[] tempChars = "������...".ToCharArray();
        string tempString = string.Empty;

        for (int i = 0, count = tempChars.Length; i < count; i++)
        {
            tempString += tempChars[i];
            _uiIntroScene.SetDialogueText(tempString);

            yield return new WaitForSeconds(0.2f);
        }
        yield return YieldCache.WaitForSeconds(2f);

        tempChars = "���� ������ �������� Ž�� ��Ȱ�� �����ϰ� ���ο� ������ �����Ϸ� �մϴ�.".ToCharArray();
        tempString = string.Empty;

        for (int i = 0, count = tempChars.Length; i < count; i++)
        {
            tempString += tempChars[i];
            _uiIntroScene.SetDialogueText(tempString);

            yield return new WaitForSeconds(0.1f);
        }


        yield return YieldCache.WaitForSeconds(2f);

        tempChars = " �׷��� ���� �ȳ��� ��ʽÿ�.".ToCharArray();
        tempString = string.Empty;

        for (int i = 0, count = tempChars.Length; i < count; i++)
        {
            tempString += tempChars[i];
            _uiIntroScene.SetDialogueText(tempString);

            yield return new WaitForSeconds(0.1f);
        }
    }
}
