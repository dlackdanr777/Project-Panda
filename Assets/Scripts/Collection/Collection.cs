using BT;
using System;
using System.Collections.Generic;
using UnityEngine;


public class Collection : MonoBehaviour, IInteraction
{
    private bool _isCollection = false; // ä�� ���ΰ�?
    private bool _isExit = false;

    #region ���� ����, �ð�, ��
    // ���� ���� �ð�, ���� ���Ƿ� ����
    [SerializeField] 
    private int _hour; // ���߿� _hour�� ���� ������ �ð� ������ ����
    private string[] _timeIds = new string[6]; // ���� ä�� ������ ��� �ð� ID
    [SerializeField] 
    private string _season; // ���߿� _season�� ���� ������ �ð� ������ ����
    private string[] _seasonIds = new string[4]; // ���� ä�� ������ ��� ���� ID

    [SerializeField] private string _map;
    #endregion

    [SerializeField] private string toolId; // ���߿� �÷��̾� ���� ������ ����

    #region ���� ä�� ������ ������ ����Ʈ
    private List<string> normalIDList = new List<string>();
    private List<string> rareIDList = new List<string>();
    private List<string> specialIDList = new List<string>();
    #endregion

    #region ä�� ���� �ð�
    private float _waitTime; // ä�� ��ٸ� �ð�
    [SerializeField] private float _spawnTime = 30f; // ä�� ������ �ð� ���� - 10�ʸ��� ä�� ����
    private float _fadeTime = 1f; // ȭ�� ��ο� �ð�
    private float _collectionLatency = 4f; // ä�� �Ϸ� �� ��� ��� ������ ���� �ð�(���� �ð� ���� �ִϸ��̼� ���)
    private float _checkTime = -1;
    #endregion

    /// <summary> ���� ä�� ���� </summary>
    [SerializeField] private GatheringItemType _gatheringType;
    private string[] _isCollecting = { "IsCollectingBug", "IsCollectingFish", "IsCollectingFruit" };


    #region ��ġ ����
    private Vector3 _targetPos; // ī�޶� ��ġ

    [SerializeField] private Vector3 _pandaPosition = new Vector3(-0.83f, -3.72f, 0); // ä���� �Ǵ� ��ġ ����
    private Vector3 _lastPandaPosition; // �Ǵ��� ������ ��ġ

    [SerializeField] private Transform[] _collectionPosition;
    private Vector3 _currentCollectionPosition;
    #endregion

    private Sprite _pandaImage;

    [SerializeField] private Sprite _collectionSprite;
    private Animator _collectionAnim;
    //[SerializeField] private CollectionButton _collectionButton;
    [SerializeField] private GameObject _speechBubble;

    public Action<string, GatheringItemType> OnCollectionSuccess;
    public Action<string> OnSuccessFrame;
    public Action<GatheringItemType> OnCollectionFail;
    public Action OnExitCollection; // ä�� ȭ�� ����
    public Action<float, GatheringItemType> OnCollectionButtonClicked;

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

    private void Start()
    {
        _waitTime = _spawnTime -1;
        //_collectionButton.OnCollectionButtonClicked += ClickCollectionButton;

    }
    private void OnDestroy()
    {
        //_collectionButton.OnCollectionButtonClicked -= ClickCollectionButton;

    }

    private void Update()
    {
        // ä�� ���ð� üũ
        if (_waitTime < _spawnTime && !_isCollection && !_isExit)
        {
            _waitTime += Time.deltaTime;
        }
        else if(_waitTime >= _spawnTime)
        {
            _waitTime = 0;
            
            // �̹� ä�� ��ġ ����
            _currentCollectionPosition = _collectionPosition[UnityEngine.Random.Range(0, _collectionPosition.Length)].position;
            gameObject.transform.position = _currentCollectionPosition;

            GetComponent<SpriteRenderer>().enabled = true;
            GetComponent<Animator>().enabled = true;
            GetComponent<BoxCollider2D>().enabled = true;
        }

        // ä�� ���̸� ī�޶� ���
        if(_isCollection)
        {
            CameraLock();

            // ä�� �����ð��� �ƴϸ� _checkTime = -1
            if (_checkTime >= 0)
            {
                _checkTime += Time.deltaTime;
                if( _checkTime >= _collectionLatency) // ä�� �����ð��� �������� Ȯ��
                {
                    IsSuccess();
                    _checkTime = -1;
                }
            }
        }
        else if(Input.GetMouseButtonDown(0) && _isExit)
        {
            OnCollectionButtonClicked?.Invoke(_fadeTime, GatheringItemType.None);

            _isExit = false;
            Debug.Log("ä�� ���� �ִϸ��̼� false");
            _collectionAnim.SetBool("IsCollectionLatency", false);
            Invoke("ExitCollection", _fadeTime);
            OnExitCollection?.Invoke();
        }
    }

    public void StartInteraction()
    {
        OnCollectionButtonClicked?.Invoke(_fadeTime, _gatheringType); // ȭ�� FadeOut
        ClickCollectionButton();

        //// ä�� ���� ������ ����
        //_collectionButton.gameObject.SetActive(true);
        GetComponent<BoxCollider2D>().enabled = false;
    }

    public void UpdateInteraction()
    {

    }

    public void ExitInteraction()
    {

    }


    private void ClickCollectionButton()
    {
        //_fadeTime = fadeTime;
        Invoke("ReadyCollection", _fadeTime);
    }

    private void ReadyCollection()
    {
        StarterPanda starterPanda = DatabaseManager.Instance.StartPandaInfo.StarterPanda;

        _pandaImage = starterPanda.GetComponent<SpriteRenderer>().sprite;
        if(_collectionAnim == null)
        {
            _collectionAnim = starterPanda.GetComponent<Animator>();
        }

        // ĳ���Ͱ� ä�� ����Ʈ�� �̵�
        _lastPandaPosition = starterPanda.gameObject.transform.position;
        starterPanda.gameObject.transform.position = _currentCollectionPosition + _pandaPosition;

        starterPanda.GetComponent<SpriteRenderer>().sprite = _collectionSprite;

        // ī�޶� �߽��� ĳ���ͷ� �����ǵ��� �̵�
        _targetPos = new Vector3(starterPanda.transform.position.x, starterPanda.transform.position.y, Camera.main.transform.position.z);
        Camera.main.gameObject.transform.position = _targetPos;

        _isCollection = true;
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Animator>().enabled = false;

        // ȭ�� ������ �ð��� ���߾� ä�� ����
        Invoke("StartCollection", _fadeTime);
    }

    /// <summary>
    /// ī�޶� �������� ���ϰ� ���� </summary>
    public void CameraLock()
    {
        Camera.main.gameObject.transform.position = _targetPos;
    }

    /// <summary>
    /// ä�� ������ �� ���� </summary>
    private void StartCollection()
    {
        // ä�� �ִϸ��̼� �Ǵٿ� ��ǳ�� ����
        _collectionAnim.enabled = true;
        _collectionAnim.SetTrigger(_isCollecting[(int)_gatheringType]);

        _speechBubble.SetActive(true);
        _speechBubble.GetComponent<Animator>().enabled = true;
    }

    /// <summary>
    /// ä�� �Ϸ� �� ��� ������ �� ���� �ð����� ���� </summary>
    public void CollectionLatency()
    {
        Debug.Log("ä�� ���� �ִϸ��̼� true");

        _collectionAnim.SetBool("IsCollectionLatency", true); // ä�� ��� ������ �� �ִϸ��̼�
        _checkTime = 0;
        Debug.Log("_checktime :"+_checkTime);
    }

    /// <summary>
    /// �������� �������� üũ </summary>
    public void IsSuccess()
    {
        float random = UnityEngine.Random.Range(0, 1f);
        Debug.Log("���� ��: "+ random);

        CheckSuccessRate();

        if (_successRate < random)
        {
            Debug.Log("���� �ִϸ��̼�");
            _collectionAnim.SetTrigger("IsCollectionFail");
            IsSuccessCollection = false;
            return;
        }
        Debug.Log("���� �ִϸ��̼�");

        _collectionAnim.SetTrigger("IsCollectionSuccess");
        IsSuccessCollection = true;
    }

    /// <summary>
    /// �������� �� ������ �� �������� �� ���� ä�� - �켱 Bug </summary>
    private void CollectionSuccess()
    {
        // ������ �ð��� ����ؼ� �� �� �������� �����ؾ� �� + ���
        // 1. ���� üũ 2. �ð� üũ 3. ��� üũ(����) 4. ������ ����(����)  

        // ���� ������ ���� �ð��� ����Ǿ��ٸ� ä���� �ð� ����
        TimeChanged();

        string collectionID = SetItem(); // ������ ����

        OnCollectionSuccess?.Invoke(collectionID, _gatheringType); // �ִϸ��̼� ���� �� �����ϴ� ������ �����ؾ� ��

        // �κ��丮�� ������ �̵�
        GameManager.Instance.Player.GatheringItemInventory[(int)_gatheringType].AddById(InventoryItemField.GatheringItem, (int)_gatheringType, collectionID);


        // ���� ������Ʈ

    }

    private void CollectionFail()
    {
        //_collectionAnim.SetBool("IsCollectionEnd", true);
        OnCollectionFail?.Invoke(_gatheringType);
    }

    private void ExitCollection()
    {
        _collectionAnim.enabled = false;
        _collectionAnim.gameObject.GetComponent<SpriteRenderer>().sprite = _pandaImage;

        DatabaseManager.Instance.StartPandaInfo.StarterPanda.gameObject.transform.position = _lastPandaPosition;

        _targetPos = new Vector3(_lastPandaPosition.x, _lastPandaPosition.y, Camera.main.transform.position.z);
        Camera.main.gameObject.transform.position = _targetPos;
    }

    /// <summary>
    /// ���� �ð��� ����Ǿ��ٸ� �׿� ���߾� ä���� �� �ֵ��� ���� </summary>
    private void TimeChanged()
    {
        if (_timeIds[5] != "GTS" + (21 + _hour)) // ���߿� _hour�� ���� ������ �ð� ������ ����
        {
            SetTime();
            Debug.Log("setTime");
            if (_seasonIds[3] != _season)
            {
                SetSeason();
                Debug.Log("setseason");

            }

            List<GatheringItem> getItemList = new List<GatheringItem>();
            switch (_gatheringType)
            {
                case GatheringItemType.Bug:
                    getItemList = DatabaseManager.Instance.GetBugItemList();
                    break;
                case GatheringItemType.Fish:
                    getItemList = DatabaseManager.Instance.GetFishItemList();
                    break;
                case GatheringItemType.Fruit:
                    getItemList = DatabaseManager.Instance.GetFruitItemList();
                    break;
            }

            normalIDList.Clear();
            rareIDList.Clear();

            foreach (GatheringItem item in getItemList)
            {
                // ���� �ð��� ä�� �����ϴٸ�(1, 2 üũ)
                if (Array.Exists(_timeIds, time => time == item.Time) && Array.Exists(_seasonIds, season => season == item.Season) && item.Map == _map)
                {
                    if (item.Rank == "�����")
                    {
                        specialIDList.Add(item.Id);
                    }
                    else if (item.Rank == "����")
                    {
                        rareIDList.Add(item.Id);
                    }
                    else
                    {
                        normalIDList.Add(item.Id);
                    }
                }
            }
        }
    }


    private void SetTime()
    {
        // �ð� ����
        _timeIds[0] = "GTS00";
        _timeIds[1] = "GTS0" + (1 + _hour / 12);
        _timeIds[2] = "GTS0" + (3 + _hour / 6);
        if(7 + _hour / 4 > 10)
        {
            _timeIds[3] = "GTS" + (7 + _hour / 4);
        }
        else
        {
            _timeIds[3] = "GTS0" + (7 + _hour / 4);
        }
        _timeIds[4] = "GTS" + (13 + _hour / 3);
        _timeIds[5] = "GTS" + (21 + _hour);
    }

    private void SetSeason()
    {
        _seasonIds[0] = "WAS";

        switch (_season)
        {
            case "WSP":
                _seasonIds[1] = "WSS";
                _seasonIds[2] = "WWS";
                break;
            case "WSU":
                _seasonIds[1] = "WSS";
                _seasonIds[2] = "WSF";
                break;
            case "WFA":
                _seasonIds[1] = "WFW";
                _seasonIds[2] = "WSF";
                break;
            case "WWT":
                _seasonIds[1] = "WFW";
                _seasonIds[2] = "WWS";
                break;
        }

        _seasonIds[3] = _season;
    }

    /// <summary>
    /// ������ ��� üũ �� ��޿� �´� ������ ���� </summary>
    private string SetItem()
    {
        float randomRarity = UnityEngine.Random.Range(0f, 1f); // 3. ��� ����
        int random;
        string collectionID;
        if (specialIDList.Count == 0 && randomRarity >= normal + rare)
        {
            randomRarity = UnityEngine.Random.Range(0f, normal + rare);
        }
        if (rareIDList.Count == 0 && randomRarity >= normal && randomRarity < normal + rare)
        {
            randomRarity = UnityEngine.Random.Range(0f, 1f - rare);
            if (randomRarity >= normal)
            {
                randomRarity += rare;
            }
        }
        if (normalIDList.Count == 0 && randomRarity < normal)
        {
            randomRarity = UnityEngine.Random.Range(normal + 0.01f, 1f);
        }

        if (randomRarity < normal)
        {
            random = UnityEngine.Random.Range(0, normalIDList.Count); // 4. ������ ����
            collectionID = normalIDList[random];
        }
        else if (randomRarity < normal + rare)
        {
            random = UnityEngine.Random.Range(0, rareIDList.Count);
            collectionID = rareIDList[random];
        }
        else
        {
            Debug.Log("�����");
            random = UnityEngine.Random.Range(0, rareIDList.Count);
            collectionID = rareIDList[random];
        }

        return collectionID;
    }

    private void CheckSuccessRate()
    {

        if (_gatheringType == GatheringItemType.Bug)
        { 
            // �÷��̾� ������ ���� Ȯ�� �����ϱ�
            if (toolId == "ITG02")
            {
                _successRate = 0.6f;
            }
            else
            {
                _successRate = 0.8f;
            }

        }
        else if (_gatheringType == GatheringItemType.Fish)
        {
            _successRate = 0.7f;

        }
        else if (_gatheringType == GatheringItemType.Fruit)
        {
            _successRate = 1f;
        }
    }
}
