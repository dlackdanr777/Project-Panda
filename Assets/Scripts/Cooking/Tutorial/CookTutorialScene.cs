using System.Collections;
using UnityEngine;

namespace CookingTutorial
{
    public class CookTutorialScene : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private UICookTutorialScene _uiCookTutorial;

        [Space]
        [Header("Sprites")]
        [SerializeField] private Sprite _cookerSprite;

        [Space]
        [Header("Audio")]
        [SerializeField] private AudioClip _backgroundAudio;


        private void Start()
        {
            SoundManager.Instance.PlayBackgroundAudio(_backgroundAudio, 1);
            StartCoroutine(StartTutorial());
        }


        private IEnumerator StartTutorial()
        {
            _uiCookTutorial.ChangeStep(CookTutorialStep.Step1);

            yield return YieldCache.WaitForSeconds(3);

            //ù��° �̹���
            _uiCookTutorial.Dialogue.StartDialogue();

            yield return YieldCache.WaitForSeconds(1.5f);

            _uiCookTutorial.Dialogue.SetDialogueNameText("�丮��");
            _uiCookTutorial.Dialogue.SetDialogueImage(_cookerSprite);
            string context = "�� �ֹ� �丮 ������ ���� ���ؾ� �ϱ�����!     \n�پ��� ������ �� Ȱ���� ������ ";
            yield return StartCoroutine(_uiCookTutorial.Dialogue.StartContext(context));

            //�ι�° �̹���
            _uiCookTutorial.ChangeStep(CookTutorialStep.Step2);

            context = "������ ���ߴٸ� �ϳ� �Ǵ� �� ������ ��Ḧ Ȱ���ؼ� ������ ���ô�! ";
            yield return StartCoroutine(_uiCookTutorial.Dialogue.StartContext(context));

            context = "���� ������ ��� �� ��ŭ ��Ḧ ���� �� ������ �� ������! ";
            yield return StartCoroutine(_uiCookTutorial.Dialogue.StartContext(context));

            //����° �̹���
            _uiCookTutorial.ChangeStep(CookTutorialStep.Step3);
            context = "�� �������� �Ѿ�ô�. ";
            yield return StartCoroutine(_uiCookTutorial.Dialogue.StartContext(context));

            //�׹�° �̹���
            _uiCookTutorial.ChangeStep(CookTutorialStep.Step4);
            context = "���õ� ���Կ� ��Ḧ �����! ";
            yield return StartCoroutine(_uiCookTutorial.Dialogue.StartContext(context));

            //�ټ���° �̹���
            _uiCookTutorial.ChangeStep(CookTutorialStep.Step5);
            context = "��Ḧ �� �� �������� �Ѿ�� ���� ��Ḧ �ٲ� �� ������ �����ؾ�����! ";
            yield return StartCoroutine(_uiCookTutorial.Dialogue.StartContext(context));

            //������° �̹���
            _uiCookTutorial.ChangeStep(CookTutorialStep.Step6);
            context = "��! ���� ���������� �丮�� �����ϴ°ſ���     \n���� ��ܿ� ù��° �������� ��Ȯ�� ����Ʈ���� ���纸�°ſ��� ";
            yield return StartCoroutine(_uiCookTutorial.Dialogue.StartContext(context));

            //�ϰ���° �̹���
            _uiCookTutorial.ChangeStep(CookTutorialStep.Step7);
            context = "�ϴܿ� ��ư���� ������ Ȱ���ؼ� ��Ȯ�� ����Ʈ�� ���ߴ°ſ��� ";
            yield return StartCoroutine(_uiCookTutorial.Dialogue.StartContext(context));

            context = "��ư�� ����� �� ���� ü���� �Ҹ��ϴϱ� �丮�� �ϼ��ϱ� ���� ü���� ��� ������� �ʵ��� �����ؾ�����! ";
            yield return StartCoroutine(_uiCookTutorial.Dialogue.StartContext(context, 40));

            //������° �̹���
            _uiCookTutorial.ChangeStep(CookTutorialStep.Step8);
            context = "�����ʿ� �˹��ϰ� �帣�� �ð� ������! �ð� �ȿ� �ϼ����ϸ� �״�� ���̿���! ";
            yield return StartCoroutine(_uiCookTutorial.Dialogue.StartContext(context));

            //��ȩ��° �̹���
            _uiCookTutorial.ChangeStep(CookTutorialStep.Step9);
            context = "��Ȯ�� �������� ���߰� ������ �ٷ� �丮 ���Ḧ ������! ";
            yield return StartCoroutine(_uiCookTutorial.Dialogue.StartContext(context));

            //����° �̹���
            _uiCookTutorial.ChangeStep(CookTutorialStep.Step10);
            context = "�׷��ٸ� �丮 �ϼ�! ������ �ѹ� ���̴� �� �����? �丮 �����غ��ڱ���! ";
            yield return StartCoroutine(_uiCookTutorial.Dialogue.StartContext(context));

            DatabaseManager.Instance.UserInfo.IsCookTutorialClear = true;
            DatabaseManager.Instance.UserInfo.AsyncSaveUserInfoData(3);

            _uiCookTutorial.Dialogue.EndDialogue();
            yield return YieldCache.WaitForSeconds(1);
            LoadingSceneManager.LoadScene("CookingScene");
        }

    }
}

