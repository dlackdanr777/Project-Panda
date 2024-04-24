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

            //첫번째 이미지
            _uiCookTutorial.Dialogue.StartDialogue();

            yield return YieldCache.WaitForSeconds(1.5f);

            _uiCookTutorial.Dialogue.SetDialogueNameText("요리사");
            _uiCookTutorial.Dialogue.SetDialogueImage(_cookerSprite);
            string context = "자 주방 요리 도구를 먼저 정해야 하구먼유!     \n다양한 도구를 잘 활용해 보와유 ";
            yield return StartCoroutine(_uiCookTutorial.Dialogue.StartContext(context));

            //두번째 이미지
            _uiCookTutorial.ChangeStep(CookTutorialStep.Step2);

            context = "도구를 정했다면 하나 또는 두 가지의 재료를 활용해서 진행해 봅시다! ";
            yield return StartCoroutine(_uiCookTutorial.Dialogue.StartContext(context));

            context = "내가 선택한 재료 수 만큼 재료를 넣을 수 있으니 잘 골라봐유! ";
            yield return StartCoroutine(_uiCookTutorial.Dialogue.StartContext(context));

            //세번째 이미지
            _uiCookTutorial.ChangeStep(CookTutorialStep.Step3);
            context = "자 다음으로 넘어갑시다. ";
            yield return StartCoroutine(_uiCookTutorial.Dialogue.StartContext(context));

            //네번째 이미지
            _uiCookTutorial.ChangeStep(CookTutorialStep.Step4);
            context = "선택된 슬롯에 재료를 골라유! ";
            yield return StartCoroutine(_uiCookTutorial.Dialogue.StartContext(context));

            //다섯번째 이미지
            _uiCookTutorial.ChangeStep(CookTutorialStep.Step5);
            context = "재료를 고른 뒤 다음으로 넘어가면 이제 재료를 바꿀 수 없으니 신중해야해유! ";
            yield return StartCoroutine(_uiCookTutorial.Dialogue.StartContext(context));

            //여섯번째 이미지
            _uiCookTutorial.ChangeStep(CookTutorialStep.Step6);
            context = "자! 이제 본격적으로 요리를 시작하는거에유     \n왼쪽 상단에 첫번째 게이지를 정확한 포인트까지 맞춰보는거에유 ";
            yield return StartCoroutine(_uiCookTutorial.Dialogue.StartContext(context));

            //일곱번째 이미지
            _uiCookTutorial.ChangeStep(CookTutorialStep.Step7);
            context = "하단에 버튼들을 적절히 활용해서 정확한 포인트를 맞추는거에유 ";
            yield return StartCoroutine(_uiCookTutorial.Dialogue.StartContext(context));

            context = "버튼을 사용할 때 마다 체력을 소모하니깐 요리를 완성하기 전에 체력을 모두 사용하지 않도록 주의해야해유! ";
            yield return StartCoroutine(_uiCookTutorial.Dialogue.StartContext(context, 40));

            //여덟번째 이미지
            _uiCookTutorial.ChangeStep(CookTutorialStep.Step8);
            context = "오른쪽에 촉박하게 흐르는 시간 보이쥬! 시간 안에 완성안하면 그대로 끝이에유! ";
            yield return StartCoroutine(_uiCookTutorial.Dialogue.StartContext(context));

            //아홉번째 이미지
            _uiCookTutorial.ChangeStep(CookTutorialStep.Step9);
            context = "정확한 게이지를 맞추고 나서는 바로 요리 종료를 눌러유! ";
            yield return StartCoroutine(_uiCookTutorial.Dialogue.StartContext(context));

            //열번째 이미지
            _uiCookTutorial.ChangeStep(CookTutorialStep.Step10);
            context = "그렇다면 요리 완성! 설명은 한번 뿐이니 잘 들었쥬? 요리 시작해보자구유! ";
            yield return StartCoroutine(_uiCookTutorial.Dialogue.StartContext(context));

            DatabaseManager.Instance.UserInfo.IsCookTutorialClear = true;
            DatabaseManager.Instance.UserInfo.AsyncSaveUserInfoData(3);

            _uiCookTutorial.Dialogue.EndDialogue();
            yield return YieldCache.WaitForSeconds(1);
            LoadingSceneManager.LoadScene("CookingScene");
        }

    }
}

