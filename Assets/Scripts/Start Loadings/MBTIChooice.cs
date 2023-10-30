using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MBTIChooice : StartList
{
    private StartClassController _uiStart;

    [Tooltip("CSV 파일 이름")]
    [SerializeField] string _csvFileName;

    [Tooltip("UI 부모 오브젝트")]
    [SerializeField] GameObject _uiFirstChooice;

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

    private Dialogue _currentDialogue;



    public override void UIStart()
    {
        if (!_isStart)
        {
            _isStart = true;
            _uiFirstChooice.SetActive(true);
            _leftButton.onClick.AddListener(OnLeftButtonClicked);
            _rightButton.onClick.AddListener(OnRightButtonClicked);
            ShowDialogue();

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


    private void ShowDialogue()
    {
        if(_dialogueIndex <= _dialogueDic.Count)
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
        _totalMBTI += _currentDialogue.LeftButtonOutput;
        ShowDialogue();
    }

    private void OnRightButtonClicked()
    {
        _totalMBTI += _currentDialogue.RightButtonOutput;
        ShowDialogue();
    }

   
}
