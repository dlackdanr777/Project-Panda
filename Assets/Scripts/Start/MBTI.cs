using Muks.Tween;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MBTI : StartClass
{
    private StartClassController _uiStart;

    [Tooltip("CSV 파일 이름")]
    [SerializeField] string _csvFileName;

    [Tooltip("UI 부모 오브젝트")]
    [SerializeField] GameObject _uiMBTI;

    [Tooltip("편지 오브젝트")]
    [SerializeField] private GameObject _uiLetter;

    [Tooltip("판다 오브젝트")]
    [SerializeField] private GameObject _pandaPaper;

    [Tooltip("편지 버튼")]
    [SerializeField] private Button _letterButton;

    [Tooltip("질문 상자")]
    [SerializeField] private Text _contexts;

    [Tooltip("왼쪽 버튼")]
    [SerializeField] private Button _leftButton;

    [Tooltip("왼쪽 버튼 텍스트")]
    [SerializeField] private Text _leftButtonContexts;

    [Tooltip("오른쪽 버튼")]
    [SerializeField] private Button _rightButton;

    [Tooltip("왼쪽 버튼 텍스트")]
    [SerializeField] private Text _rightButtonContexts;


    private Dictionary<int, Dialogue> _dialogueDic = new Dictionary<int, Dialogue>();

    private int _dialogueIndex = 1;

    private string _totalMBTI;

    private bool _isStart;

    private bool _isEnd;

    private bool _isButtonClickEnable;

    private Dialogue _currentDialogue;


    private void OnEnable()
    {
        _uiLetter.SetActive(false);
        _pandaPaper.SetActive(false);
    }

    public override void UIStart()
    {
        if (!_isStart)
        {
            _isStart = true;
            _uiMBTI.SetActive(true);
            _contexts.gameObject.SetActive(false);
            _rightButton.gameObject.SetActive(false);
            _leftButton.gameObject.SetActive(false);
            _letterButton.gameObject.SetActive(false);
            

            _leftButton.onClick.AddListener(OnLeftButtonClicked);
            _rightButton.onClick.AddListener(OnRightButtonClicked);

            StartAnime();

            Debug.Log("시작");
        }
        else
        {
            Debug.Log("이미 실행중 입니다.");
        } 
    }

    public override void UIUpdate()
    {

    }


    public override void UIEnd()
    {
        _uiMBTI.SetActive(false);
        Tween.TransformMove(_pandaPaper, new Vector2(0, -13), 11.5f, TweenMode.Smootherstep, () =>
        {
            Tween.SpriteRendererAlpha(_pandaPaper, 0, 1, TweenMode.Smootherstep);
        });
        _uiStart.ChangeCurrentClass();
    }


    public override void Init(StartClassController uiStart)
    {
        _uiStart = uiStart;

        DialogueParser theParser = GetComponent<DialogueParser>();
        Dialogue[] dialogues = theParser.Parse(_csvFileName);
        _uiMBTI.SetActive(false);

        for (int i = 0; i < dialogues.Length; i++)
        {
            _dialogueDic.Add(i + 1, dialogues[i]);
        }
    }

    //시작 애니메이션
    private void StartAnime()
    {
        _uiLetter.SetActive(true);
        Tween.TransformMove(_uiLetter, new Vector2(0, 5), 5, TweenMode.Smoothstep);
        Tween.TransformRotate(_uiLetter, new Vector3(0, 0, 720), 5, TweenMode.Smoothstep, ButtonAnime);
    }

    private void AtivateDialog()
    {
        _isButtonClickEnable = true;
        _contexts.gameObject.SetActive(true);
        _rightButton.gameObject.SetActive(true);
        _leftButton.gameObject.SetActive(true);
        ShowDialogue();
        UIChangeAlpha(1, 1f);
    }

    //버튼의 애니메이션
    private void ButtonAnime()
    {
        _letterButton.gameObject.SetActive(true);
        _letterButton.onClick.AddListener(OnLetterButtonClicked);
    }


    private void UIChangeAlpha(float alpha, float duration, Action onComplate = null)
    {
        Tween.TextAlpha(_contexts.gameObject, alpha, duration, TweenMode.Smoothstep);
        Tween.IamgeAlpha(_leftButton.gameObject, alpha, duration, TweenMode.Smootherstep);
        Tween.TextAlpha(_leftButtonContexts.gameObject, alpha, duration, TweenMode.Smoothstep);

        Tween.IamgeAlpha(_rightButton.gameObject, alpha, duration, TweenMode.Smootherstep);
        Tween.TextAlpha(_rightButtonContexts.gameObject, alpha, duration, TweenMode.Smoothstep, onComplate);
    }


    private void ShowDialogue()
    {
        _isButtonClickEnable = true;

        if (_dialogueIndex <= _dialogueDic.Count)
        {
            _currentDialogue = GetDialogue(_dialogueIndex);
            _contexts.text = _currentDialogue.Contexts;
            _leftButtonContexts.text = _currentDialogue.LeftButtonContexts;
            _rightButtonContexts.text = _currentDialogue.RightButtonContexts;
            _dialogueIndex++;
        }
        else
        {
            if (_isEnd)
                return;

            _isEnd = true;
            StartCoroutine(CompleteMBTI(1.5f));
        }
    }

    private IEnumerator CompleteMBTI(float time)
    {
        _contexts.text = string.Empty;
        char[] tempText = { '당', '신', '의', ' ', '성', '향', '은', '.', '.', '.' };

        _leftButton.gameObject.SetActive(false);
        _rightButton.gameObject.SetActive(false);

        yield return new WaitForSeconds(time);

        for (int i = 0; i < tempText.Length; i++)
        {
            _contexts.text += tempText[i];
            yield return new WaitForSeconds(0.15f);
        }

        yield return new WaitForSeconds(time);

        _contexts.text = _totalMBTI + "\n입니다.";

        yield return new WaitForSeconds(3);

        UIEnd();
    }


    private Dialogue GetDialogue(int id)
    {
        if (_dialogueDic.ContainsKey(id))
        {
            Dialogue dialogue = _dialogueDic[id];
            return dialogue;
        }
        return default;
    }


    private void OnLeftButtonClicked()
    {
        if (!_isButtonClickEnable)
            return;

        _isButtonClickEnable = false;
        _totalMBTI += _currentDialogue.LeftButtonOutput;

        UIChangeAlpha(0, 0.5f, () => {
            ShowDialogue();
            UIChangeAlpha(1, 0.5f);
            });
    }

    private void OnRightButtonClicked()
    {
        if (!_isButtonClickEnable)
            return;

        _isButtonClickEnable = false;
        _totalMBTI += _currentDialogue.RightButtonOutput;

        UIChangeAlpha(0, 0.5f, () => {
            ShowDialogue();
            UIChangeAlpha(1, 0.5f);
        });
    }

    //버튼 클릭 이벤트
    private void OnLetterButtonClicked()
    {
        _pandaPaper.SetActive(true);
        _letterButton.onClick.RemoveAllListeners();
        _letterButton.gameObject.SetActive(false);

        if(_pandaPaper.TryGetComponent(out SpriteRenderer renderer))
        {
            Color color = renderer.color;
            color.a = 0;
            renderer.color = color;
        }

        Tween.SpriteRendererAlpha(_pandaPaper, 1, 5, TweenMode.Smootherstep, () =>
        {
            Tween.SpriteRendererAlpha(_uiLetter, 0, 5, TweenMode.Smoothstep);
            Tween.TransformMove(_pandaPaper, new Vector2(0, 7), 5, TweenMode.Smootherstep, () =>AtivateDialog());
        });    
    }


}
