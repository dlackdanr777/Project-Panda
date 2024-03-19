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
        //���̵� �ƿ�
        SoundManager.Instance.PlayBackgroundAudio(_scene1Music, 3);
        yield return YieldCache.WaitForSeconds(3f);
        Tween.SpriteRendererAlpha(_fadeImage.gameObject, 0, 3f, TweenMode.Constant, () => _fadeImage.gameObject.SetActive(false));

        yield return YieldCache.WaitForSeconds(6f);


        //���ͷ� ��
        _poirotAnimator.SetTrigger("Smoke");

        yield return YieldCache.WaitForSeconds(1.5f);

        //ī�޶� �÷���
        _flashAnimator.SetTrigger("Flash");

        yield return YieldCache.WaitForSeconds(5);

        //��� ����
        _uiIntroScene.StartDialogue();
        _uiIntroScene.SetDialogueNameText("���ͷ�");
        yield return YieldCache.WaitForSeconds(1f);

        string context = "������... ";
        yield return StartCoroutine(_uiIntroScene.StartContext(context, 35, 0.15f));

        context = "���� ������ ��������   \nŽ�� ��Ȱ�� �����ϰ� ���ο� ������ �����Ϸ� �մϴ�. ";
        yield return StartCoroutine(_uiIntroScene.StartContext(context, 35));

        context = "�׷��� ���� �ȳ���... ";
        yield return StartCoroutine(_uiIntroScene.StartContext(context, 35));

        //��� ����
        _uiIntroScene.EndDialogue();

        SoundManager.Instance.PlayBackgroundAudio(_poirotHideSound, 1.5f, false);

        yield return YieldCache.WaitForSeconds(5.5f);

        //�λ�
        _poirotAnimator.SetTrigger("Greet");

        yield return YieldCache.WaitForSeconds(2f);

        //��¦ ȿ����
        _flashAnimator.SetTrigger("Flashing");

        yield return YieldCache.WaitForSeconds(0.5f);

        //���ͷ� �����
        _poirot.gameObject.SetActive(false);

        yield return YieldCache.WaitForSeconds(6f);


        //���2 ����
        _uiIntroScene.StartDialogue();
        _uiIntroScene.SetDialogueNameText("���ڵ�");
        yield return YieldCache.WaitForSeconds(1f);

        //ui����
        _uiIntroScene.ShakeDialogue(1.2f);

        //���� ��������
        _reporterAnimator.SetTrigger("Turmoil");
        SoundManager.Instance.PlayEffectAudio(_reportersRumblingSound);

        context = "���ͷΰ� �������!!! ";
        yield return StartCoroutine(_uiIntroScene.StartContext(context, 60, 0.07f));

        context = "�̰� ����� ������!         \n���� ����ȸ�߿��� ����� �������� Ž��!  ";
        yield return StartCoroutine(_uiIntroScene.StartContext(context));

        //��� ����
        _uiIntroScene.EndDialogue();

        yield return YieldCache.WaitForSeconds(1f);

        //���̵� ��
        _fadeImage.gameObject.SetActive(true);
        _fadeImage.color = new Color(0, 0, 0, 0);

        Tween.SpriteRendererAlpha(_fadeImage.gameObject, 1, 2, TweenMode.Constant);

        yield return YieldCache.WaitForSeconds(2f);
        StartCoroutine(Scene2());
    }


    private IEnumerator Scene2()
    {
        //��2 ����
        SoundManager.Instance.PlayBackgroundAudio(_scene2Music, 2, true);

        _backgroundImage.SetActive(true);
        _scene2Poya.SetActive(true);

        yield return YieldCache.WaitForSeconds(4f);

        //���̵� �ƿ�
        _fadeImage.gameObject.SetActive(true);
        _fadeImage.color = new Color(_fadeImage.color.r, _fadeImage.color.g, _fadeImage.color.b, 1);
        Tween.SpriteRendererAlpha(_fadeImage.gameObject, 0, 2, TweenMode.Constant);


        yield return YieldCache.WaitForSeconds(5f);


        //���� �����
        _uiIntroScene.StartDialogue();
        _uiIntroScene.SetDialogueNameText("����");
        _uiIntroScene.SetDialogueImage(_poyaSprite);

        yield return YieldCache.WaitForSeconds(1f);

        string context = "������... �Ҿƹ���... ";
        yield return StartCoroutine(_uiIntroScene.StartContext(context, 35, 0.15f));

        context = "���...?          \n�̰� ����? ";
        yield return StartCoroutine(_uiIntroScene.StartContext(context, 35));


        //���� ��� ����
        _uiIntroScene.EndDialogue();

        yield return YieldCache.WaitForSeconds(2f);

        //������ ���̵�ƿ�
        _fadeImage.color = new Color(0, 0, 0, 0);
        _fadeImage.gameObject.SetActive(true);

        Tween.SpriteRendererAlpha(_fadeImage.gameObject, 0.8f, 2);


        yield return YieldCache.WaitForSeconds(3f);

        //������ ���� �ö���� ���
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

        //���� Ŭ�������� ���
        while (!_isletterClicked)
        {
            yield return YieldCache.WaitForSeconds(0.02f);
        }


        //���� ���� �ִϸ��̼�
        _openLetter.GetComponent<CanvasGroup>().alpha = 0;
        _openLetter.gameObject.SetActive(true);
        _letterEffect.color = new Color(1, 1, 1, 0.4f);

        Tween.IamgeAlpha(_closeLetterButton.gameObject, 0, 1, TweenMode.Constant, () => _closeLetter.gameObject.SetActive(false));
        Tween.IamgeAlpha(_letterEffect.gameObject, 0, 1, TweenMode.Constant);

        yield return YieldCache.WaitForSeconds(2f);
        Tween.CanvasGroupAlpha(_openLetter.gameObject, 1, 3f);

        yield return YieldCache.WaitForSeconds(5f);


        //���� ���� ����
        _uiIntroScene.StartDialogue();
        _uiIntroScene.SetDialogueNameText("���Ʒ�");
        _uiIntroScene.SetDialogueImage(_poirotSprite);

        yield return YieldCache.WaitForSeconds(1f);

        context = "�� ���� ���߿��� ";
        yield return StartCoroutine(_uiIntroScene.StartContext(context));

        context = "�װ� �Ҿƹ����� ������ ����� ����� �Ŷ� ������. ";
        yield return StartCoroutine(_uiIntroScene.StartContext(context));

        context = "������ �̰� ���ۿ� �Ұ���.          \n���� ��¥ ������ �������;�. ";
        yield return StartCoroutine(_uiIntroScene.StartContext(context));

        _uiIntroScene.SetDialogueImage(_poirotGreetSprite);

        context = "���� ���� å�� �ҿ������� ����� ���� ������ �����ϱ� �ٶ���. ";
        yield return StartCoroutine(_uiIntroScene.StartContext(context));


        //��� ����
        _uiIntroScene.EndDialogue();

        yield return YieldCache.WaitForSeconds(2f);

        //�� �ִϸ��̼�
        _uiFadeImage.color = new Color(1, 1, 1, 0);
        _uiFadeImage.gameObject.SetActive(true);

        Tween.IamgeAlpha(_uiFadeImage.gameObject, 1, 0.3f, TweenMode.Quadratic);
        Tween.IamgeAlpha(_uiFadeImage.gameObject, 1, 1.5f);

        yield return YieldCache.WaitForSeconds(2f);
        //���� ���
        _uiIntroScene.StartDialogue();
        _uiIntroScene.SetDialogueNameText("����");
        _uiIntroScene.SetDialogueImage(_poyaSprite);

        yield return YieldCache.WaitForSeconds(1f);

        //ui����
        _uiIntroScene.ShakeDialogue(1.2f);

        context = "���ƾ�!!        ";
        yield return StartCoroutine(_uiIntroScene.StartContext(context, 60, 0.05f));


        //�߽� ��ȭ ���
        _uiIntroScene.SetDialogueNameText("");
        _uiIntroScene.SetDialogueImage(null);

        context = ". . . ";
        yield return StartCoroutine(_uiIntroScene.StartContext(context, 35, 0.2f));

        //�߽� ��ȭ ���
        _uiIntroScene.SetDialogueNameText("���Ʒ�");
        _uiIntroScene.SetDialogueImage(_poirotSprite);

        context = "ps. �� �´�. ���� ���ڸ��� ���۵Ǵ� �����ϰŶ�      \n- ���̹��� - ";
        yield return StartCoroutine(_uiIntroScene.StartContext(context));

      
        _uiIntroScene.EndDialogue();

        //������ ���̵�ƿ�
        _uiFadeImage.color = new Color(1, 1, 1, 1);
        _uiFadeImage.gameObject.SetActive(true);

        Color targetColor = new Color(0, 0, 0, 1);
        Tween.IamgeColor(_uiFadeImage.gameObject, targetColor, 2);

        SoundManager.Instance.PlayBackgroundAudio(_scene3Music, 3, false);
        yield return YieldCache.WaitForSeconds(4f);

        //��Ƽ���� ��Ʃ��� �ΰ�� ���

        _companyLogo.gameObject.SetActive(true);
        _companyLogo.GetComponent<CanvasGroup>().alpha = 0;

        Tween.CanvasGroupAlpha(_companyLogo, 1, 2);

        yield return YieldCache.WaitForSeconds(3f);

        //��� �� ���� ��Ȱ��ȭ
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

        //���� ����
        yield return YieldCache.WaitForSeconds(3f);
        _scene3Text.gameObject.SetActive(true);
        char[] tempChars = "�ź��� ������ �� �Ҿƹ����� ã�Ƴ���     \n���� ��Ž���� �ǰھ�! ".ToCharArray();
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

        //���̵� �� �ƿ�
        _uiFadeImage.color = new Color(0, 0, 0, 0);
        _uiFadeImage.gameObject.SetActive(true);

        Tween.IamgeAlpha(_uiFadeImage.gameObject, 1, 3f, TweenMode.Constant);
        yield return YieldCache.WaitForSeconds(6);

        Tween.IamgeAlpha(_uiFadeImage.gameObject, 0, 3f, TweenMode.Constant);

        //Ÿ��Ʋ ����
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
