using Muks.Tween;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MBTIChooice : StartList
{
    private StartClassController _uiStart;

    [Tooltip("CSV ���� �̸�")]
    [SerializeField] string _csvFileName;

    [Tooltip("UI �θ� ������Ʈ")]
    [SerializeField] GameObject _uiFirstChooice;

    [Tooltip("���� ������Ʈ")]
    [SerializeField] private GameObject _uiLetter;

    [Tooltip("���� ��ư")]
    [SerializeField] private Button _letterButton;

    [Tooltip("���� ����")]
    [SerializeField] private Text _contexts;

    [Tooltip("���� ��ư")]
    [SerializeField] private Button _leftButton;

    [Tooltip("���� ��ư �ؽ�Ʈ")]
    [SerializeField] private Text _leftButtonContexts;

    [Tooltip("������ ��ư")]
    [SerializeField] private Button _rightButton;

    [Tooltip("���� ��ư �ؽ�Ʈ")]
    [SerializeField] private Text _rightButtonContexts;


    private Dictionary<int, Dialogue> _dialogueDic = new Dictionary<int, Dialogue>();

    private int _dialogueIndex = 1;

    private string _totalMBTI;

    private bool _isStart;

    private bool _isEnd;

    private bool _isButtonClickEnable;

    private Dialogue _currentDialogue;




    public override void UIStart()
    {
        if (!_isStart)
        {
            _isStart = true;
            _uiFirstChooice.SetActive(true);
            _contexts.gameObject.SetActive(false);
            _rightButton.gameObject.SetActive(false);
            _leftButton.gameObject.SetActive(false);
            _letterButton.gameObject.SetActive(false);

            _leftButton.onClick.AddListener(OnLeftButtonClicked);
            _rightButton.onClick.AddListener(OnRightButtonClicked);

            StartAnime();

            Debug.Log("����");
        }
        else
        {
            Debug.Log("�̹� ������ �Դϴ�.");
        } 
    }

    public override void UIUpdate()
    {

    }


    public override void UIEnd()
    {
        _uiFirstChooice.SetActive(false);
        _uiStart.ChangeCurrentClass();
    }


    public override void Init(StartClassController uiStart)
    {
        _uiStart = uiStart;

        DialogueParser theParser = GetComponent<DialogueParser>();
        Dialogue[] dialogues = theParser.Parse(_csvFileName);
        _uiFirstChooice.SetActive(false);

        for (int i = 0; i < dialogues.Length; i++)
        {
            _dialogueDic.Add(i + 1, dialogues[i]);
        }
    }

    //���� �ִϸ��̼�
    private void StartAnime()
    {
        Tween.RectTransfromAnchoredPosition(_uiLetter, new Vector2(0, 0), 4, TweenMode.Smoothstep);
        
        Tween.TransformRotate(_uiLetter, new Vector3(0, 0, 360), 1f, TweenMode.Constant, () => _uiLetter.transform.eulerAngles = new Vector3(0,0,0));
        Tween.TransformRotate(_uiLetter, new Vector3(0, 0, 360), 1f, TweenMode.Constant, () => _uiLetter.transform.eulerAngles = new Vector3(0, 0, 0));
        Tween.TransformRotate(_uiLetter, new Vector3(0, 0, 360), 2f, TweenMode.Constant, ButtonAnime);
    }

    private void AtivateDialog()
    {
        _isButtonClickEnable = true;
        _contexts.gameObject.SetActive(true);
        _rightButton.gameObject.SetActive(true);
        _leftButton.gameObject.SetActive(true);
        ShowDialogue();
        UIChangeAlpha(1, 0.5f);
    }

    //��ư�� �ִϸ��̼�
    private void ButtonAnime()
    {
        _letterButton.gameObject.SetActive(true);
        _letterButton.onClick.AddListener(OnLetterButtonClicked);
    }

    //��ư Ŭ�� �̺�Ʈ
    private void OnLetterButtonClicked()
    {
        AtivateDialog();
        _letterButton.onClick.RemoveAllListeners();
        _letterButton.gameObject.SetActive(false);
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
            if (!_isEnd)
            {
                _isEnd = true;
                StartCoroutine(CompleteMBTI(1.5f));
            }
        }
    }

    private IEnumerator CompleteMBTI(float time)
    {
        _contexts.text = string.Empty;
        char[] tempText = { '��', '��', '��', ' ', '��', '��', '��', '.', '.', '.' };

        _leftButton.gameObject.SetActive(false);
        _rightButton.gameObject.SetActive(false);

        yield return new WaitForSeconds(time);

        for (int i = 0; i < tempText.Length; i++)
        {
            _contexts.text += tempText[i];
            yield return new WaitForSeconds(0.15f);
        }

        yield return new WaitForSeconds(time);

        _contexts.text = _totalMBTI + "\n�Դϴ�.";

        yield return new WaitForSeconds(time);

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


}
