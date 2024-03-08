using Muks.Tween;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class IntroScene : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private UIIntroScene _uiIntroScene;

    [Space]
    [Header("Scene1 Compnents")]
    [SerializeField] private SpriteRenderer _fadeImage;
    [SerializeField] private GameObject _flash;
    [SerializeField] private GameObject _poirot;
    [SerializeField] private GameObject _reporter;

    [Space]
    [Header("Scene2 Compnents")]
    [SerializeField] private Button _closeLetter;
    [SerializeField] private Image _openLetter;
    [SerializeField] private Sprite _poyaSprite;
    [SerializeField] private Sprite _poirotSprite;
    [SerializeField] private Sprite _poirotGreetSprite;
    [SerializeField] private Image _uiFadeImage;
    [SerializeField] private GameObject _companyLogo;

    private Animator _flashAnimator;
    private Animator _poirotAnimator;
    private Animator _reporterAnimator;

    private bool _isletterClicked;

    private void Awake()
    {
        _flashAnimator = _flash.GetComponent<Animator>();
        _poirotAnimator = _poirot.GetComponent<Animator>();
        _reporterAnimator = _reporter.GetComponent<Animator>();

        _uiIntroScene.Init();
        Scene1Init();
        Scene2Init();
    }


    private void Start()
    {
        StartCoroutine(Scene1());
    }


    private void Scene1Init()
    {
        _poirot.gameObject.SetActive(true);
        _fadeImage.gameObject.SetActive(true);
        _fadeImage.color = new Color(0, 0, 0, 1);
        _companyLogo.SetActive(false);
        _uiFadeImage.gameObject.SetActive(false);
    }


    private void Scene2Init()
    {
        _openLetter.GetComponent<CanvasGroup>().alpha = 0;
        _openLetter.gameObject.SetActive(false);

        _uiFadeImage.color = new Color(1, 1, 1, 0);
        _uiFadeImage.gameObject.SetActive(false);
    }


    private IEnumerator Scene1()
    {
        //페이드 아웃
        yield return YieldCache.WaitForSeconds(1f);

        Tween.SpriteRendererAlpha(_fadeImage.gameObject, 0, 2f, TweenMode.Constant, () => _fadeImage.gameObject.SetActive(false));

        yield return YieldCache.WaitForSeconds(5f);


        //포와로 흡연
        _poirotAnimator.SetTrigger("Smoke");

        yield return YieldCache.WaitForSeconds(1.5f);

        //카메라 플래시
        _flashAnimator.SetTrigger("Flash");

        yield return YieldCache.WaitForSeconds(5);

        //대사 시작
        _uiIntroScene.StartDialogue();
        _uiIntroScene.SetDialogueNameText("포와로");
        yield return YieldCache.WaitForSeconds(1f);

        char[] tempChars = "여러분... ".ToCharArray();
        string tempString = string.Empty;

        for (int i = 0, count = tempChars.Length; i < count; i++)
        {
            _uiIntroScene.SetDialogueContext(tempString);
            tempString += tempChars[i];

            yield return YieldCache.WaitForSeconds(0.1f);
        }

        yield return YieldCache.WaitForSeconds(3f);

        tempChars = "저는 오늘을 기점으로   \n탐정 생활을 마감하고 새로운 여정을 시작하려 합니다. ".ToCharArray();
        tempString = string.Empty;

        for (int i = 0, count = tempChars.Length; i < count; i++)
        {
            _uiIntroScene.SetDialogueContext(tempString);
            tempString += tempChars[i];

            yield return YieldCache.WaitForSeconds(0.07f);
        }


        yield return YieldCache.WaitForSeconds(3f);

        tempChars = "그러니 이제 안녕히 계십시오.".ToCharArray();
        tempString = string.Empty;

        for (int i = 0, count = tempChars.Length; i < count; i++)
        {
            _uiIntroScene.SetDialogueContext(tempString);
            tempString += tempChars[i];

            yield return YieldCache.WaitForSeconds(0.07f);
        }

        yield return YieldCache.WaitForSeconds(3f);

        _uiIntroScene.EndDialogue();

        yield return YieldCache.WaitForSeconds(2.5f);

        //인사
        _poirotAnimator.SetTrigger("Greet");

        yield return YieldCache.WaitForSeconds(2f);

        //반짝 효과음
        _flashAnimator.SetTrigger("Flashing");

        yield return YieldCache.WaitForSeconds(0.5f);

        //포와로 사라짐
        _poirot.gameObject.SetActive(false);

        yield return YieldCache.WaitForSeconds(5.5f);




        //대사2 시작
        _uiIntroScene.StartDialogue();
        _uiIntroScene.SetDialogueNameText("기자들");
        yield return YieldCache.WaitForSeconds(1f);

        //관중 웅성웅성
        _reporterAnimator.SetTrigger("Turmoil");

        tempChars = "포와로가 사라졌다!!!".ToCharArray();
        tempString = string.Empty;

        for (int i = 0, count = tempChars.Length; i < count; i++)
        {
            _uiIntroScene.SetDialogueContext(tempString, 60);
            tempString += tempChars[i];

            yield return YieldCache.WaitForSeconds(0.05f);
        }

        yield return YieldCache.WaitForSeconds(4f);

        tempChars = "이건 대단한 뉴스야!         \n은퇴 기자회견에서 사라진 세계적인 탐정!  ".ToCharArray();
        tempString = string.Empty;

        for (int i = 0, count = tempChars.Length; i < count; i++)
        {
            _uiIntroScene.SetDialogueContext(tempString);
            tempString += tempChars[i];

            yield return YieldCache.WaitForSeconds(0.07f);
        }


        yield return YieldCache.WaitForSeconds(4f);
        //대사 종료
        _uiIntroScene.EndDialogue();

        yield return YieldCache.WaitForSeconds(1f);

        //페이드 인
        _fadeImage.gameObject.SetActive(true);
        _fadeImage.color = new Color(0, 0, 0, 0);

        Tween.SpriteRendererAlpha(_fadeImage.gameObject, 1, 2, TweenMode.Constant);

        StartCoroutine(Scene2());
    }


    private IEnumerator Scene2()
    {
        yield return YieldCache.WaitForSeconds(4f);

        //편지가 위로 올라오는 장면
        _closeLetter.interactable = false;
        Tween.RectTransfromAnchoredPosition(_closeLetter.gameObject, new Vector2(0, 0), 3, TweenMode.Smoothstep, () =>
        {
            Tween.RectTransfromAnchoredPosition(_closeLetter.gameObject, new Vector2(0, 0), 1, TweenMode.Smoothstep, () =>
            {
                _closeLetter.interactable = true;
                _closeLetter.onClick.AddListener(OnLetterClicked);

                Color targetColor = new Color(1, 0.9f, 0.9f, 1);
                Tween.IamgeColor(_closeLetter.gameObject, targetColor, 1f, TweenMode.EaseInOutBack).Loop(LoopType.Yoyo);
            });
        });

        yield return YieldCache.WaitForSeconds(3f);

        //편지 클릭전까지 대기
        while (!_isletterClicked)
        {
            yield return YieldCache.WaitForSeconds(0.02f);
        }

        //편지 오픈 애니메이션
        _openLetter.GetComponent<CanvasGroup>().alpha = 0;
        _openLetter.gameObject.SetActive(true);

        Tween.IamgeAlpha(_closeLetter.gameObject, 0, 1, TweenMode.Constant, () => _closeLetter.gameObject.SetActive(false));
        yield return YieldCache.WaitForSeconds(2f);
        Tween.CanvasGroupAlpha(_openLetter.gameObject, 1, 3f);

        yield return YieldCache.WaitForSeconds(5f);

        //포야 독백 시작
        _uiIntroScene.StartDialogue();
        _uiIntroScene.SetDialogueNameText("포아로");
        _uiIntroScene.SetDialogueImage(_poirotSprite);

        yield return YieldCache.WaitForSeconds(1f);

        char[] tempChars = "내 손주 포야에게 ".ToCharArray();
        string tempString = string.Empty;

        for (int i = 0, count = tempChars.Length; i < count; i++)
        {
            _uiIntroScene.SetDialogueContext(tempString);
            tempString += tempChars[i];

            yield return YieldCache.WaitForSeconds(0.07f);
        }

        yield return YieldCache.WaitForSeconds(2.5f);


        tempChars = "네가 할아버지의 마지막 모습에 놀랐을 거라 생각해. ".ToCharArray();
        tempString = string.Empty;

        for (int i = 0, count = tempChars.Length; i < count; i++)
        {
            _uiIntroScene.SetDialogueContext(tempString);
            tempString += tempChars[i];

            yield return YieldCache.WaitForSeconds(0.07f);
        }

        yield return YieldCache.WaitForSeconds(3f);

        tempChars = "하지만 이건 시작에 불과해.          \n나의 진짜 모험은 이제부터야. ".ToCharArray();
        tempString = string.Empty;

        for (int i = 0, count = tempChars.Length; i < count; i++)
        {
            _uiIntroScene.SetDialogueContext(tempString);
            tempString += tempChars[i];

            yield return YieldCache.WaitForSeconds(0.07f);
        }

        yield return YieldCache.WaitForSeconds(3f);

        _uiIntroScene.SetDialogueImage(_poirotGreetSprite);
        tempChars = "내가 남긴 책과 소원나무의 열쇠로 너의 여정을 시작하길 바란다. ".ToCharArray();
        tempString = string.Empty;

        for (int i = 0, count = tempChars.Length; i < count; i++)
        {
            _uiIntroScene.SetDialogueContext(tempString);
            tempString += tempChars[i];

            yield return YieldCache.WaitForSeconds(0.07f);
        }

        yield return YieldCache.WaitForSeconds(3f);
        //대사 종료
        _uiIntroScene.EndDialogue();

        yield return YieldCache.WaitForSeconds(2f);

        //펑 애니메이션
        _uiFadeImage.color = new Color(1, 1, 1, 0);
        _uiFadeImage.gameObject.SetActive(true);

        Tween.IamgeAlpha(_uiFadeImage.gameObject, 1, 0.3f, TweenMode.Quadratic);
        Tween.IamgeAlpha(_uiFadeImage.gameObject, 1, 1.5f);
        Tween.IamgeAlpha(_uiFadeImage.gameObject, 0, 1f, TweenMode.Smoothstep, () =>
         {
             _uiFadeImage.color = new Color(0, 0, 0, 0);
             _uiFadeImage.gameObject.SetActive(false);
         });


        yield return YieldCache.WaitForSeconds(5f);



        _uiIntroScene.StartDialogue();
        _uiIntroScene.SetDialogueNameText("포야");
        _uiIntroScene.SetDialogueImage(_poyaSprite);

        yield return YieldCache.WaitForSeconds(1f);

        tempChars = "으아악!!        ".ToCharArray();
        tempString = string.Empty;

        for (int i = 0, count = tempChars.Length; i < count; i++)
        {
            _uiIntroScene.SetDialogueContext(tempString);
            tempString += tempChars[i];

            yield return YieldCache.WaitForSeconds(0.1f);
        }

        yield return YieldCache.WaitForSeconds(3f);

        //추신 대화 출력

        _uiIntroScene.SetDialogueNameText("포아로");
        _uiIntroScene.SetDialogueImage(_poirotSprite);

        yield return YieldCache.WaitForSeconds(1f);

        tempChars = "ps. 아 맞다. 편지 읽자마자 전송되니 조심하거라      \n- 바이바이 - ".ToCharArray();
        tempString = string.Empty;

        for (int i = 0, count = tempChars.Length; i < count; i++)
        {
            _uiIntroScene.SetDialogueContext(tempString);
            tempString += tempChars[i];

            yield return YieldCache.WaitForSeconds(0.07f);
        }

        yield return YieldCache.WaitForSeconds(4f);

        //검은색 페이드아웃
        _uiFadeImage.color = new Color(0, 0, 0, 0);
        _uiFadeImage.gameObject.SetActive(true);

        Tween.IamgeAlpha(_uiFadeImage.gameObject, 1, 2);

        yield return YieldCache.WaitForSeconds(4f);

        //멀티버스 스튜디오 로고명 출력

        _companyLogo.gameObject.SetActive(true);
        _companyLogo.GetComponent<CanvasGroup>().alpha = 0;

        Tween.CanvasGroupAlpha(_companyLogo, 1, 2);
    }


    private void OnLetterClicked()
    {
        _isletterClicked = true;
        _closeLetter.onClick.RemoveAllListeners();
        _closeLetter.interactable = false;
    }
}
