using BT;
using Muks.DataBind;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainStoryCollection : MonoBehaviour
{
    public Action<string> OnCollectionSuccess;
    public Action<string> OnSuccessFrame;
    public Action<GatheringItemType> OnCollectionFail;
    public Action OnExitCollection; // ä�� ȭ�� ����
    public Action<float, GatheringItemType, string> OnCollectionButtonClicked;

    private bool _isClickStarButton;
    private bool _isCollection = false; // ä�� ���ΰ�?
    private bool _isExit = false;


    //[SerializeField] private CollectionButton _collectionButton;
    [SerializeField] private Animator _speechBubble;
    [SerializeField] private GameObject _starButton;
    private Animator _collectionAnim;


    #region �Ǵ�
    [Tooltip("�Ǵ� ä�� �̹���")]
    [SerializeField] private Sprite _pandaCollectionSprite;
    private Sprite _pandaImage;
    private Animator _pandaCollectionAnim;
    private SpriteRenderer _pandaSpriteRenderer;
    #endregion

    #region ��ġ ����
    private Vector3 _targetPos; // ī�޶� ��ġ

    [Tooltip("ä���� �Ǵ� ������ ���� ��ġ ����")]
    [SerializeField] private Transform _pandaTransform; // ä���� �Ǵ� ��ġ ����
    private Vector3 _lastPandaPosition; // �Ǵ��� ������ ��ġ

    [Tooltip("ä�� ��ġ ����")]
    [SerializeField] private Transform _collectionPosition;
    private Vector3 _currentCollectionPosition;
    #endregion

    #region ä�� ���� �ð�
    private float _waitTime; // ä�� ��ٸ� �ð�
    [SerializeField] private float _spawnTime = 30f; // ä�� ������ �ð� ���� - 10�ʸ��� ä�� ����
    private float _fadeTime = 1f; // ȭ�� ��ο� �ð�
    private float _collectionLatency = 1.7f; // ä�� �Ϸ� �� ��� ��� ������ ���� �ð�(���� �ð� ���� �ִϸ��̼� ���)
    private float _checkTime = -1;
    #endregion

    #region ���� ����, �ð�, ��
    // ���� ���� �ð�, ���� ���Ƿ� ����
    public int Hour => TimeManager.Instance.GameHour; // ���߿� _hour�� ���� ������ �ð� ������ ����
    private string[] _timeIds = new string[6]; // ���� ä�� ������ ��� �ð� ID
    public string Season => TimeManager.Instance.GameWeatherId; // ���߿� _season�� ���� ������ �ð� ������ ����
    private string[] _seasonIds = new string[4]; // ���� ä�� ������ ��� ���� ID

    [SerializeField] private string _map;
    #endregion

    /// <summary> ���� ä�� ���� </summary>
    [SerializeField] private GatheringItemType _gatheringType;
    private string[] _isCollecting = { "IsCollectingBug", "IsCollectingFish", "IsCollectingFruit" };

    [SerializeField] private string toolId; // ���߿� �÷��̾� ���� ������ ����

    // ������ ��� Ȯ��
    private float normal = 0.87f;
    private float rare = 0.1f;

    #region ä�� ���� üũ
    private float _successRate;
    private bool _isSuccessCollection;
    public bool IsSuccessCollection
    {
        get { return _isSuccessCollection; }
        set
        {
            _isSuccessCollection = value;

            // ä�� ����: �ִϸ��̼� ���� + �ؽ�Ʈ �����
            if (_isSuccessCollection)
            {
                CollectionSuccess();
            }
            else
            {
                CollectionFail();
            }
            _isCollection = false;
            _isExit = true;
        }
    }
    #endregion


    public string CollectionID { get; set; } // ä���ؾ� �� ������ ID;
    public string MainStoryID { get; set; } // �� ���ν��丮�� ������ ��� ä�� ����
    public string NextMainStoryID { get; set; } // �� ���ν��丮�� ������ ��� ä�� ����

    private void Start()
    {
        _waitTime = _spawnTime - 1;
        _collectionAnim = _starButton.GetComponent<Animator>();
        //_collectionButton.OnCollectionButtonClicked += ClickCollectionButton;
        switch (_gatheringType)
        {
            case GatheringItemType.Bug:
                toolId = "ITG02";
                break;
            case GatheringItemType.Fish:
                toolId = "ITG01";
                break;
            case GatheringItemType.Fruit:
                break;
        }
        DataBind.SetButtonValue(_starButton.name, ClickStarButton);

        MainStoryID = gameObject.name.Substring(0, 5);
        NextMainStoryID = DatabaseManager.Instance.MainDialogueDatabase.MSDic[MainStoryID].NextStoryID;

    }

    private void Update()
    {
        // ä�� �������� ���濡 �ִٸ� ä�� ���� �ȵǵ��� �ϱ�
        if (!string.IsNullOrEmpty(CollectionID) && GameManager.Instance.Player.FindItemById(CollectionID))
        {
            _waitTime = 0;
        }

        // ���� ���ν��丮�� ����Ǿ��� ��� ��ũ��Ʈ �����ϱ�
        if (!string.IsNullOrEmpty(NextMainStoryID) && !string.IsNullOrEmpty(DatabaseManager.Instance.MainDialogueDatabase.StoryCompletedList.Find(x => x == NextMainStoryID)))
        {
            gameObject.SetActive(false);
        }

        // ä�� ���ð� üũ
        if (_waitTime < _spawnTime && !_isCollection && !_isExit)
        {
            _waitTime += Time.deltaTime;
        }
        else if (_waitTime >= _spawnTime && !string.IsNullOrEmpty(CollectionID) && (_gatheringType == GatheringItemType.Fruit || GameManager.Instance.Player.FindItemById(toolId)))
        {
            _waitTime = 0;

            // ä�� ��ġ ����
            _currentCollectionPosition = _collectionPosition.position;
            gameObject.transform.position = _currentCollectionPosition;

            _starButton.SetActive(true);
            _collectionAnim.enabled = true;
        }

        if (_isCollection)
        {
            // ä�� �����ð��� �ƴϸ� _checkTime = -1
            if (_checkTime >= 0)
            {
                _checkTime += Time.deltaTime;
                if (_checkTime >= _collectionLatency) // ä�� �����ð��� �������� Ȯ��
                {
                    IsSuccess();
                    _checkTime = -1;
                }
            }
        }
        else if (Input.GetMouseButtonDown(0) && _isExit)
        {
            OnCollectionButtonClicked?.Invoke(_fadeTime, GatheringItemType.None, MainStoryID);

            _isExit = false;

            _pandaCollectionAnim.SetBool("IsCollectionLatency", false);
            Invoke("ExitCollection", _fadeTime);
            OnExitCollection?.Invoke();
        }
    }

    public void ClickStarButton()
    {
        if (_isClickStarButton)
        {
            return;
        }
        _isClickStarButton = true;
        OnCollectionButtonClicked?.Invoke(_fadeTime, _gatheringType, MainStoryID); // ȭ�� FadeOut
        ClickCollectionButton();

        //// ä�� ���� ������ ����
        //_collectionButton.gameObject.SetActive(true);
        _starButton.SetActive(false);

        DataBind.GetAction("HideMainUIButton")?.Invoke();
        CameraSet(true);
    }

    private void ClickCollectionButton()
    {
        //_fadeTime = fadeTime;
        Invoke("ReadyCollection", _fadeTime);
    }

    private void ReadyCollection()
    {
        TimeManager.Instance.CheckTime();

        StarterPanda starterPanda = StarterPanda.Instance;
        _pandaSpriteRenderer = starterPanda.GetComponent<SpriteRenderer>();

        _pandaImage = _pandaSpriteRenderer.sprite;

        if (_pandaCollectionAnim == null)
        {
            _pandaCollectionAnim = starterPanda.GetComponent<Animator>();
            Debug.Log("_pandaCollectionAnim" + _pandaCollectionAnim);
        }

        // ĳ���Ͱ� ä�� ����Ʈ�� �̵�
        _lastPandaPosition = starterPanda.gameObject.transform.position;
        //starterPanda.gameObject.transform.position = _currentCollectionPosition + _pandaTransform.position;
        if (_gatheringType == GatheringItemType.Fish)
        {
            starterPanda.gameObject.transform.position = new Vector3(_pandaTransform.position.x, _pandaTransform.position.y, starterPanda.gameObject.transform.position.z);
        }
        else
        {
            starterPanda.gameObject.transform.position = new Vector3(_currentCollectionPosition.x, _pandaTransform.position.y, starterPanda.gameObject.transform.position.z);
        }

        _pandaSpriteRenderer.sprite = _pandaCollectionSprite;

        // ī�޶� �߽��� ĳ���ͷ� �����ǵ��� �̵�
        _targetPos = new Vector3(starterPanda.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z);
        Camera.main.gameObject.transform.position = _targetPos;

        _isCollection = true;

        _collectionAnim.enabled = false;

        // ȭ�� ������ �ð��� ���߾� ä�� ����
        Invoke("StartCollection", _fadeTime);

        // ���� ���̴� �ִϸ��̼� ����
        _pandaCollectionAnim.SetInteger("Num", -1);
        _pandaCollectionAnim.Play("Idle");
        _pandaCollectionAnim.enabled = false;
    }

    /// <summary>
    /// ī�޶� ���� </summary>
    public void CameraSet(bool set)
    {
        GameManager.Instance.FriezeCameraMove = set;
        GameManager.Instance.FriezeCameraZoom = set;
        GameManager.Instance.FirezeInteraction = set;
        //Camera.main.gameObject.transform.position = _targetPos;
    }

    /// <summary>
    /// ä�� ������ �� ���� </summary>
    private void StartCollection()
    {
        StarterPanda starterPanda = StarterPanda.Instance;
        // ��ǳ�� ��ġ ����
        gameObject.transform.position = new Vector3(starterPanda.transform.position.x, starterPanda.transform.position.y + 3, gameObject.transform.position.z);

        // ä�� �ִϸ��̼� �Ǵٿ� ��ǳ�� ����
        _pandaCollectionAnim.enabled = true;
        _pandaCollectionAnim.SetTrigger(_isCollecting[(int)_gatheringType]);

        _speechBubble.gameObject.SetActive(true);
        _speechBubble.enabled = true;
    }

    /// <summary>
    /// ä�� �Ϸ� �� ��� ������ �� ���� �ð����� ���� </summary>
    public void CollectionLatency()
    {
        Debug.Log("_map" + _map);
        _pandaCollectionAnim.SetBool("IsCollectionLatency", true); // ä�� ��� ������ �� �ִϸ��̼�
        _checkTime = 0;
    }

    /// <summary>
    /// �������� �������� üũ</summary>
    public void IsSuccess()
    {
        // ����Ʈ �������� ������ ����
        Debug.Log("���� �ִϸ��̼�");

        _pandaCollectionAnim.SetTrigger("IsCollectionSuccess");
        IsSuccessCollection = true;
    }

    /// <summary>
    /// �������� �� ����Ʈ ������ ä��
    private void CollectionSuccess()
    {
        string collectionID = CollectionID; // ������ ������ ä��

        if (collectionID != null)
        {
            OnCollectionSuccess?.Invoke(collectionID);

            // �κ��丮�� ������ �̵�
            GameManager.Instance.Player.AddItemById(collectionID, 1);


            // ���� ������Ʈ

            // ���� ���� �޼�
            //DatabaseManager.Instance.Challenges.GatheringSuccess((int)_gatheringType);
        }
        else
        {
            CollectionFail();
        }
    }

    private void CollectionFail()
    {
        OnCollectionFail?.Invoke(_gatheringType);
    }

    private void ExitCollection()
    {
        _pandaCollectionAnim.SetInteger("Num", StarterPanda.Instance.Num);

        _pandaSpriteRenderer.sprite = _pandaImage;

        StarterPanda.Instance.gameObject.transform.position = _lastPandaPosition;

        DataBind.GetAction("ShowMainUIButton")?.Invoke();
        CameraSet(false);
        _targetPos = new Vector3(_lastPandaPosition.x, Camera.main.transform.position.y, Camera.main.transform.position.z);
        Camera.main.gameObject.transform.position = _targetPos;
        _isClickStarButton = false;
    }
}
