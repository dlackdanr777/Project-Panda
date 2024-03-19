using Muks.Tween;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class IntroScene : MonoBehaviour
{

    [Header("Components")]
    [SerializeField] private UIIntroScene _uiIntroScene;
    [SerializeField] private Button _skipButton;

    [Space]
    [Header("Scene1 Components")]
    [SerializeField] private SpriteRenderer _fadeImage;
    [SerializeField] private GameObject _flash;
    [SerializeField] private GameObject _poirot;
    [SerializeField] private GameObject _reporter;

    [Space]
    [Header("Scene2 Components")]
    [SerializeField] private GameObject _backgroundImage;
    [SerializeField] private GameObject _scene2Poya;
    [SerializeField] private GameObject _closeLetter;
    [SerializeField] private Button _closeLetterButton;
    [SerializeField] private Image _openLetter;
    [SerializeField] private Image _letterEffect;
    [SerializeField] private Sprite _poyaSprite;
    [SerializeField] private Sprite _poirotSprite;
    [SerializeField] private Sprite _poirotGreetSprite;
    [SerializeField] private Image _uiFadeImage;
    [SerializeField] private GameObject _companyLogo;

    [Space]
    [Header("Scene3 Components")]
    [SerializeField] private UIIntroTitle _uiTitle;
    [SerializeField] private Transform _startPos;
    [SerializeField] private Transform _endCameraPos;
    [SerializeField] private Transform _endPoyaPos;
    [SerializeField] private GameObject _scene3Poya;
    [SerializeField] private TextMeshProUGUI _scene3Text;

    [Space]
    [Header("Clips")]
    [SerializeField] private AudioClip _scene1Music;
    [SerializeField] private AudioClip _scene2Music;
    [SerializeField] private AudioClip _scene3Music;
    [SerializeField] private AudioClip _treeMusic;
    [SerializeField] private AudioClip _poirotHideSound;
    [SerializeField] private AudioClip _reportersRumblingSound;

    private Animator _flashAnimator;
    private Animator _poirotAnimator;
    private Animator _reporterAnimator;
    private Animator _poyaAnimator;

    private bool _isletterClicked;

    private void Awake()
    {
        _flashAnimator = _flash.GetComponent<Animator>();
        _poirotAnimator = _poirot.GetComponent<Animator>();
        _reporterAnimator = _reporter.GetComponent<Animator>();
        _poyaAnimator = _scene3Poya.GetComponent<Animator>();

        _uiIntroScene.Init();
        Scene1Init();
        Scene2Init();
        Scene3Init();

        _skipButton.onClick.AddListener(OnSkipButtonClicked);
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
        _backgroundImage.SetActive(false);
        _scene2Poya.SetActive(false);

        _openLetter.GetComponent<CanvasGroup>().alpha = 0;
        _openLetter.gameObject.SetActive(false);

        _uiFadeImage.color = new Color(1, 1, 1, 0);
        _uiFadeImage.gameObject.SetActive(false);

        _letterEffect.gameObject.SetActive(false);
    }


    private void Scene3Init() 
    {
        _scene3Text.text = string.Empty;
        _uiTitle.Init();
        _scene3Text.gameObject.SetActive(false);
    }


    private IEnumerator Scene1()
    {
        //페이드 아웃
        SoundManager.Instance.PlayBackgroundAudio(_scene1Music, 3);
        yield return YieldCache.WaitForSeconds(3f);
        Tween.SpriteRendererAlpha(_fadeImage.gameObject, 0, 3f, TweenMode.Constant, () => _fadeImage.gameObject.SetActive(false));

        yield return YieldCache.WaitForSeconds(6f);


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

        string context = "여러분... ";
        yield return StartCoroutine(_uiIntroScene.StartContext(context, 35, 0.15f));

        context = "저는 오늘을 기점으로   \n탐정 생활을 마감하고 새로운 여정을 시작하려 합니다. ";
        yield return StartCoroutine(_uiIntroScene.StartContext(context, 35));

        context = "그러니 이제 안녕히... ";
        yield return StartCoroutine(_uiIntroScene.StartContext(context, 35));

        //대사 종료
        _uiIntroScene.EndDialogue();

        SoundManager.Instance.PlayBackgroundAudio(_poirotHideSound, 1.5f, false);

        yield return YieldCache.WaitForSeconds(5.5f);

        //인사
        _poirotAnimator.SetTrigger("Greet");

        yield return YieldCache.WaitForSeconds(2f);

        //반짝 효과음
        _flashAnimator.SetTrigger("Flashing");

        yield return YieldCache.WaitForSeconds(0.5f);

        //포와로 사라짐
        _poirot.gameObject.SetActive(false);

        yield return YieldCache.WaitForSeconds(6f);


        //대사2 시작
        _uiIntroScene.StartDialogue();
        _uiIntroScene.SetDialogueNameText("기자들");
        yield return YieldCache.WaitForSeconds(1f);

        //ui흔들기
        _uiIntroScene.ShakeDialogue(1.2f);

        //관중 웅성웅성
        _reporterAnimator.SetTrigger("Turmoil");
        SoundManager.Instance.PlayEffectAudio(_reportersRumblingSound);

        context = "포와로가 사라졌다!!! ";
        yield return StartCoroutine(_uiIntroScene.StartContext(context, 60, 0.07f));

        context = "이건 대단한 뉴스야!         \n은퇴 기자회견에서 사라진 세계적인 탐정!  ";
        yield return StartCoroutine(_uiIntroScene.StartContext(context));

        //대사 종료
        _uiIntroScene.EndDialogue();

        yield return YieldCache.WaitForSeconds(1f);

        //페이드 인
        _fadeImage.gameObject.SetActive(true);
        _fadeImage.color = new Color(0, 0, 0, 0);

        Tween.SpriteRendererAlpha(_fadeImage.gameObject, 1, 2, TweenMode.Constant);

        yield return YieldCache.WaitForSeconds(2f);
        StartCoroutine(Scene2());
    }


    private IEnumerator Scene2()
    {
        //씬2 셋팅
        SoundManager.Instance.PlayBackgroundAudio(_scene2Music, 2, true);

        _backgroundImage.SetActive(true);
        _scene2Poya.SetActive(true);

        yield return YieldCache.WaitForSeconds(4f);

        //페이드 아웃
        _fadeImage.gameObject.SetActive(true);
        _fadeImage.color = new Color(_fadeImage.color.r, _fadeImage.color.g, _fadeImage.color.b, 1);
        Tween.SpriteRendererAlpha(_fadeImage.gameObject, 0, 2, TweenMode.Constant);


        yield return YieldCache.WaitForSeconds(5f);


        //포야 흐느낌
        _uiIntroScene.StartDialogue();
        _uiIntroScene.SetDialogueNameText("포야");
        _uiIntroScene.SetDialogueImage(_poyaSprite);

        yield return YieldCache.WaitForSeconds(1f);

        string context = "흑흑흑... 할아버지... ";
        yield return StartCoroutine(_uiIntroScene.StartContext(context, 35, 0.15f));

        context = "어라...?          \n이건 뭐지? ";
        yield return StartCoroutine(_uiIntroScene.StartContext(context, 35));


        //포야 대사 종료
        _uiIntroScene.EndDialogue();

        yield return YieldCache.WaitForSeconds(2f);

        //검은색 페이드아웃
        _fadeImage.color = new Color(0, 0, 0, 0);
        _fadeImage.gameObject.SetActive(true);

        Tween.SpriteRendererAlpha(_fadeImage.gameObject, 0.8f, 2);


        yield return YieldCache.WaitForSeconds(3f);

        //편지가 위로 올라오는 장면
        _closeLetterButton.interactable = false;
        Tween.RectTransfromAnchoredPosition(_closeLetter.gameObject, new Vector2(0, 0), 3, TweenMode.Smoothstep, () =>
        {
            Tween.RectTransfromAnchoredPosition(_closeLetter.gameObject, new Vector2(0, 0), 1, TweenMode.Smoothstep, () =>
            {
                _letterEffect.gameObject.SetActive(true);
                _letterEffect.color = new Color(1, 1, 1, 0);
                Tween.IamgeAlpha(_letterEffect.gameObject, 0.5f, 0.5f, TweenMode.Constant, () =>
                {
                    _closeLetterButton.interactable = true;
                    _closeLetterButton.onClick.AddListener(OnLetterClicked);
                    _letterEffect.color = new Color(1, 1, 1, 0.5f);
                    Color targetColor = new Color(1, 1, 1, 1);


                    Tween.IamgeColor(_letterEffect.gameObject, targetColor, 2f, TweenMode.EaseInOutBack).Loop(LoopType.Yoyo);
                });

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
        _letterEffect.color = new Color(1, 1, 1, 0.4f);

        Tween.IamgeAlpha(_closeLetterButton.gameObject, 0, 1, TweenMode.Constant, () => _closeLetter.gameObject.SetActive(false));
        Tween.IamgeAlpha(_letterEffect.gameObject, 0, 1, TweenMode.Constant);

        yield return YieldCache.WaitForSeconds(2f);
        Tween.CanvasGroupAlpha(_openLetter.gameObject, 1, 3f);

        yield return YieldCache.WaitForSeconds(5f);


        //포야 독백 시작
        _uiIntroScene.StartDialogue();
        _uiIntroScene.SetDialogueNameText("포아로");
        _uiIntroScene.SetDialogueImage(_poirotSprite);

        yield return YieldCache.WaitForSeconds(1f);

        context = "내 손주 포야에게 ";
        yield return StartCoroutine(_uiIntroScene.StartContext(context));

        context = "네가 할아버지의 마지막 모습에 놀랐을 거라 생각해. ";
        yield return StartCoroutine(_uiIntroScene.StartContext(context));

        context = "하지만 이건 시작에 불과해.          \n나의 진짜 모험은 이제부터야. ";
        yield return StartCoroutine(_uiIntroScene.StartContext(context));

        _uiIntroScene.SetDialogueImage(_poirotGreetSprite);

        context = "내가 남긴 책과 소원나무의 열쇠로 너의 여정을 시작하길 바란다. ";
        yield return StartCoroutine(_uiIntroScene.StartContext(context));


        //대사 종료
        _uiIntroScene.EndDialogue();

        yield return YieldCache.WaitForSeconds(2f);

        //펑 애니메이션
        _uiFadeImage.color = new Color(1, 1, 1, 0);
        _uiFadeImage.gameObject.SetActive(true);

        Tween.IamgeAlpha(_uiFadeImage.gameObject, 1, 0.3f, TweenMode.Quadratic);
        Tween.IamgeAlpha(_uiFadeImage.gameObject, 1, 1.5f);

        yield return YieldCache.WaitForSeconds(2f);
        //포야 대사
        _uiIntroScene.StartDialogue();
        _uiIntroScene.SetDialogueNameText("포야");
        _uiIntroScene.SetDialogueImage(_poyaSprite);

        yield return YieldCache.WaitForSeconds(1f);

        //ui흔들기
        _uiIntroScene.ShakeDialogue(1.2f);

        context = "으아악!!        ";
        yield return StartCoroutine(_uiIntroScene.StartContext(context, 60, 0.05f));


        //추신 대화 출력
        _uiIntroScene.SetDialogueNameText("");
        _uiIntroScene.SetDialogueImage(null);

        context = ". . . ";
        yield return StartCoroutine(_uiIntroScene.StartContext(context, 35, 0.2f));

        //추신 대화 출력
        _uiIntroScene.SetDialogueNameText("포아로");
        _uiIntroScene.SetDialogueImage(_poirotSprite);

        context = "ps. 아 맞다. 편지 읽자마자 전송되니 조심하거라      \n- 바이바이 - ";
        yield return StartCoroutine(_uiIntroScene.StartContext(context));

      
        _uiIntroScene.EndDialogue();

        //검은색 페이드아웃
        _uiFadeImage.color = new Color(1, 1, 1, 1);
        _uiFadeImage.gameObject.SetActive(true);

        Color targetColor = new Color(0, 0, 0, 1);
        Tween.IamgeColor(_uiFadeImage.gameObject, targetColor, 2);

        SoundManager.Instance.PlayBackgroundAudio(_scene3Music, 3, false);
        yield return YieldCache.WaitForSeconds(4f);

        //멀티버스 스튜디오 로고명 출력

        _companyLogo.gameObject.SetActive(true);
        _companyLogo.GetComponent<CanvasGroup>().alpha = 0;

        Tween.CanvasGroupAlpha(_companyLogo, 1, 2);

        yield return YieldCache.WaitForSeconds(3f);

        //대사 및 편지 비활성화
        _uiIntroScene.EndDialogue();
        _openLetter.gameObject.SetActive(false);
        _backgroundImage.SetActive(false);
        _scene2Poya.SetActive(false);

        yield return YieldCache.WaitForSeconds(3f);

        Tween.CanvasGroupAlpha(_companyLogo, 0, 2);
        yield return YieldCache.WaitForSeconds(3f);

        StartCoroutine(Scene3());
    }


    private IEnumerator Scene3()
    {
        _fadeImage.gameObject.SetActive(false);
        _uiFadeImage.gameObject.SetActive(true);
        _uiFadeImage.color = Color.black;

        //포야 독백
        yield return YieldCache.WaitForSeconds(3f);
        _scene3Text.gameObject.SetActive(true);
        char[] tempChars = "신비한 숲에서 꼭 할아버지를 찾아내고     \n나도 대탐정이 되겠어! ".ToCharArray();
        string tempString = string.Empty;

        for (int i = 0, count = tempChars.Length; i < count; i++)
        {
            tempString += tempChars[i];
            _scene3Text.text = tempString;

            yield return YieldCache.WaitForSeconds(0.07f);
        }

        yield return YieldCache.WaitForSeconds(4f);

        Tween.TMPAlpha(_scene3Text.gameObject, 0, 2);

        yield return YieldCache.WaitForSeconds(5);
        _scene3Text.gameObject.SetActive(false);

        Camera.main.transform.position = _startPos.position;
        Tween.IamgeAlpha(_uiFadeImage.gameObject, 0, 2);
        _poyaAnimator.SetTrigger("Turn");
        Tween.TransformMove(_scene3Poya, _endPoyaPos.position, 9f, TweenMode.Smoothstep);
        Tween.TransformMove(Camera.main.gameObject, _endCameraPos.position, 7.5f, TweenMode.Smoothstep);

        yield return YieldCache.WaitForSeconds(8f);

        //페이드 인 아웃
        _uiFadeImage.color = new Color(0, 0, 0, 0);
        _uiFadeImage.gameObject.SetActive(true);

        Tween.IamgeAlpha(_uiFadeImage.gameObject, 1, 3f, TweenMode.Constant);
        yield return YieldCache.WaitForSeconds(6);

        Tween.IamgeAlpha(_uiFadeImage.gameObject, 0, 3f, TweenMode.Constant);

        //타이틀 시작
        yield return YieldCache.WaitForSeconds(6);
        _uiFadeImage.gameObject.SetActive(false);
        _uiTitle.Show();
    }


    private void OnLetterClicked()
    {
        _isletterClicked = true;
        Tween.Stop(_letterEffect.gameObject);
        _closeLetterButton.onClick.RemoveAllListeners();
        _closeLetterButton.interactable = false;
    }


    private void OnSkipButtonClicked()
    {
        DatabaseManager.Instance.UserInfo.IsExistingUser = true;
        DatabaseManager.Instance.UserInfo.SaveUserInfoData(10);
        LoadingSceneManager.LoadScene("24_01_09_Integrated");
    }
}
