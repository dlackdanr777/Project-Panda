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
        //���̵� �ƿ�
        yield return YieldCache.WaitForSeconds(1f);

        Tween.SpriteRendererAlpha(_fadeImage.gameObject, 0, 2f, TweenMode.Constant, () => _fadeImage.gameObject.SetActive(false));

        yield return YieldCache.WaitForSeconds(5f);


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

        char[] tempChars = "������... ".ToCharArray();
        string tempString = string.Empty;

        for (int i = 0, count = tempChars.Length; i < count; i++)
        {
            _uiIntroScene.SetDialogueContext(tempString);
            tempString += tempChars[i];

            yield return YieldCache.WaitForSeconds(0.1f);
        }

        yield return YieldCache.WaitForSeconds(3f);

        tempChars = "���� ������ ��������   \nŽ�� ��Ȱ�� �����ϰ� ���ο� ������ �����Ϸ� �մϴ�. ".ToCharArray();
        tempString = string.Empty;

        for (int i = 0, count = tempChars.Length; i < count; i++)
        {
            _uiIntroScene.SetDialogueContext(tempString);
            tempString += tempChars[i];

            yield return YieldCache.WaitForSeconds(0.07f);
        }


        yield return YieldCache.WaitForSeconds(3f);

        tempChars = "�׷��� ���� �ȳ��� ��ʽÿ�.".ToCharArray();
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

        //�λ�
        _poirotAnimator.SetTrigger("Greet");

        yield return YieldCache.WaitForSeconds(2f);

        //��¦ ȿ����
        _flashAnimator.SetTrigger("Flashing");

        yield return YieldCache.WaitForSeconds(0.5f);

        //���ͷ� �����
        _poirot.gameObject.SetActive(false);

        yield return YieldCache.WaitForSeconds(5.5f);




        //���2 ����
        _uiIntroScene.StartDialogue();
        _uiIntroScene.SetDialogueNameText("���ڵ�");
        yield return YieldCache.WaitForSeconds(1f);

        //���� ��������
        _reporterAnimator.SetTrigger("Turmoil");

        tempChars = "���ͷΰ� �������!!!".ToCharArray();
        tempString = string.Empty;

        for (int i = 0, count = tempChars.Length; i < count; i++)
        {
            _uiIntroScene.SetDialogueContext(tempString, 60);
            tempString += tempChars[i];

            yield return YieldCache.WaitForSeconds(0.05f);
        }

        yield return YieldCache.WaitForSeconds(4f);

        tempChars = "�̰� ����� ������!         \n���� ����ȸ�߿��� ����� �������� Ž��!  ".ToCharArray();
        tempString = string.Empty;

        for (int i = 0, count = tempChars.Length; i < count; i++)
        {
            _uiIntroScene.SetDialogueContext(tempString);
            tempString += tempChars[i];

            yield return YieldCache.WaitForSeconds(0.07f);
        }


        yield return YieldCache.WaitForSeconds(4f);
        //��� ����
        _uiIntroScene.EndDialogue();

        yield return YieldCache.WaitForSeconds(1f);

        //���̵� ��
        _fadeImage.gameObject.SetActive(true);
        _fadeImage.color = new Color(0, 0, 0, 0);

        Tween.SpriteRendererAlpha(_fadeImage.gameObject, 1, 2, TweenMode.Constant);

        StartCoroutine(Scene2());
    }


    private IEnumerator Scene2()
    {
        yield return YieldCache.WaitForSeconds(4f);

        //������ ���� �ö���� ���
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

        //���� Ŭ�������� ���
        while (!_isletterClicked)
        {
            yield return YieldCache.WaitForSeconds(0.02f);
        }

        //���� ���� �ִϸ��̼�
        _openLetter.GetComponent<CanvasGroup>().alpha = 0;
        _openLetter.gameObject.SetActive(true);

        Tween.IamgeAlpha(_closeLetter.gameObject, 0, 1, TweenMode.Constant, () => _closeLetter.gameObject.SetActive(false));
        yield return YieldCache.WaitForSeconds(2f);
        Tween.CanvasGroupAlpha(_openLetter.gameObject, 1, 3f);

        yield return YieldCache.WaitForSeconds(5f);

        //���� ���� ����
        _uiIntroScene.StartDialogue();
        _uiIntroScene.SetDialogueNameText("���Ʒ�");
        _uiIntroScene.SetDialogueImage(_poirotSprite);

        yield return YieldCache.WaitForSeconds(1f);

        char[] tempChars = "�� ���� ���߿��� ".ToCharArray();
        string tempString = string.Empty;

        for (int i = 0, count = tempChars.Length; i < count; i++)
        {
            _uiIntroScene.SetDialogueContext(tempString);
            tempString += tempChars[i];

            yield return YieldCache.WaitForSeconds(0.07f);
        }

        yield return YieldCache.WaitForSeconds(2.5f);


        tempChars = "�װ� �Ҿƹ����� ������ ����� ����� �Ŷ� ������. ".ToCharArray();
        tempString = string.Empty;

        for (int i = 0, count = tempChars.Length; i < count; i++)
        {
            _uiIntroScene.SetDialogueContext(tempString);
            tempString += tempChars[i];

            yield return YieldCache.WaitForSeconds(0.07f);
        }

        yield return YieldCache.WaitForSeconds(3f);

        tempChars = "������ �̰� ���ۿ� �Ұ���.          \n���� ��¥ ������ �������;�. ".ToCharArray();
        tempString = string.Empty;

        for (int i = 0, count = tempChars.Length; i < count; i++)
        {
            _uiIntroScene.SetDialogueContext(tempString);
            tempString += tempChars[i];

            yield return YieldCache.WaitForSeconds(0.07f);
        }

        yield return YieldCache.WaitForSeconds(3f);

        _uiIntroScene.SetDialogueImage(_poirotGreetSprite);
        tempChars = "���� ���� å�� �ҿ������� ����� ���� ������ �����ϱ� �ٶ���. ".ToCharArray();
        tempString = string.Empty;

        for (int i = 0, count = tempChars.Length; i < count; i++)
        {
            _uiIntroScene.SetDialogueContext(tempString);
            tempString += tempChars[i];

            yield return YieldCache.WaitForSeconds(0.07f);
        }

        yield return YieldCache.WaitForSeconds(3f);
        //��� ����
        _uiIntroScene.EndDialogue();

        yield return YieldCache.WaitForSeconds(2f);

        //�� �ִϸ��̼�
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
        _uiIntroScene.SetDialogueNameText("����");
        _uiIntroScene.SetDialogueImage(_poyaSprite);

        yield return YieldCache.WaitForSeconds(1f);

        tempChars = "���ƾ�!!        ".ToCharArray();
        tempString = string.Empty;

        for (int i = 0, count = tempChars.Length; i < count; i++)
        {
            _uiIntroScene.SetDialogueContext(tempString);
            tempString += tempChars[i];

            yield return YieldCache.WaitForSeconds(0.1f);
        }

        yield return YieldCache.WaitForSeconds(3f);

        //�߽� ��ȭ ���

        _uiIntroScene.SetDialogueNameText("���Ʒ�");
        _uiIntroScene.SetDialogueImage(_poirotSprite);

        yield return YieldCache.WaitForSeconds(1f);

        tempChars = "ps. �� �´�. ���� ���ڸ��� ���۵Ǵ� �����ϰŶ�      \n- ���̹��� - ".ToCharArray();
        tempString = string.Empty;

        for (int i = 0, count = tempChars.Length; i < count; i++)
        {
            _uiIntroScene.SetDialogueContext(tempString);
            tempString += tempChars[i];

            yield return YieldCache.WaitForSeconds(0.07f);
        }

        yield return YieldCache.WaitForSeconds(4f);

        //������ ���̵�ƿ�
        _uiFadeImage.color = new Color(0, 0, 0, 0);
        _uiFadeImage.gameObject.SetActive(true);

        Tween.IamgeAlpha(_uiFadeImage.gameObject, 1, 2);

        yield return YieldCache.WaitForSeconds(4f);

        //��Ƽ���� ��Ʃ��� �ΰ�� ���

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
