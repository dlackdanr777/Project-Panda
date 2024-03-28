using BT;
using Muks.DataBind;
using System;
using System.Collections.Generic;
using UnityEngine;


public class Collection : MonoBehaviour
{
    public Action<string, GatheringItemType> OnCollectionSuccess;
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
    private NPCAnimeControllCenter _poyaAnimeCenter;
    private bool _isPoyaActive; // ���� ���߰� ���� �ִ��� Ȯ��
    private bool _isPoyaSpriteActive; // ���� ���߰� ���� �ִ��� Ȯ��
    #endregion

    #region ��ġ ����
    private Vector3 _targetPos; // ī�޶� ��ġ

    [Tooltip("ä���� �Ǵ� ������ ���� ��ġ ����")]
    [SerializeField] private Transform _pandaTransform; // ä���� �Ǵ� ��ġ ����
    private Vector3 _lastPandaPosition; // �Ǵ��� ������ ��ġ

    [Tooltip("ä�� ��ġ ����")]
    [SerializeField] private Transform[] _collectionPosition;
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

    #region ���� ä�� ������ ������ ����Ʈ
    private List<string> normalIDList = new List<string>();
    private List<string> rareIDList = new List<string>();
    private List<string> specialIDList = new List<string>();
    #endregion

    /// <summary> ���� ä�� ���� </summary>
    [SerializeField] private GatheringItemType _gatheringType;
    private string[] _isCollecting = { "IsCollectingBug", "IsCollectingFish", "IsCollectingFruit" };

    private string toolId; // ä���� �ʿ��� ����ID

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


    [Space]
    [Header("Audio Clips")]
    [SerializeField] private AudioClip _fishAnimeStartSound;
    [SerializeField] private AudioClip _fishAnimeMiddleSound;
    [SerializeField] private AudioClip _fishAnimeEndSound;

    private void Start()
    {
        _waitTime = _spawnTime -1;
        _collectionAnim = _starButton.GetComponent<Animator>();
        //_collectionButton.OnCollectionButtonClicked += ClickCollectionButton;

        string starButton = "";
        switch (_gatheringType)
        {
            case GatheringItemType.Bug:
                starButton = "BugStarButton";
                toolId = "ITG02";
                break;
            case GatheringItemType.Fish:
                starButton = "FishStarButton";
                toolId = "ITG01";
                break;
            case GatheringItemType.Fruit:
                starButton = "FruitStarButton";
                break;
        }
        DataBind.SetButtonValue(_map + starButton, ClickStarButton);

        _poyaAnimeCenter = GameObject.Find("Poya Anime ControllCenter").transform.GetComponent<NPCAnimeControllCenter>();
    }
    //private void OnDestroy()
    //{
    //    _collectionButton.OnCollectionButtonClicked -= ClickCollectionButton;

    //}

    private void Update()
    {
        // ä�� ���ð� üũ
        if (_waitTime < _spawnTime && !_isCollection && !_isExit)
        {
            _waitTime += Time.deltaTime;
        }
        else if(_waitTime >= _spawnTime && (_gatheringType == GatheringItemType.Fruit || GameManager.Instance.Player.FindItemById(toolId)))
        {
            _waitTime = 0;
            
            // �̹� ä�� ��ġ ����
            _currentCollectionPosition = _collectionPosition[UnityEngine.Random.Range(0, _collectionPosition.Length)].position;
            gameObject.transform.position = _currentCollectionPosition;

            _starButton.SetActive(true);
            _collectionAnim.enabled = true;
        }

        if(_isCollection)
        {
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
            OnCollectionButtonClicked?.Invoke(_fadeTime, GatheringItemType.None, _map);

            _isExit = false;

            //_pandaCollectionAnim.SetBool("IsCollectionLatency", false);
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

        SoundManager.Instance.PlayEffectAudio(SoundEffectType.ButtonClick);

        _isClickStarButton = true;
        OnCollectionButtonClicked?.Invoke(_fadeTime, _gatheringType, _map); // ȭ�� FadeOut
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
        if(_gatheringType == GatheringItemType.Fish)
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

        foreach(Transform child in _poyaAnimeCenter.transform)
        {
            if(child.name == _map && child.gameObject.activeSelf == true)
            {
                child.gameObject.SetActive(false);
                break;
            }
            else if(child.name == _map)
            {
                _isPoyaActive = true;
                break;
            }
            
        }
        if (_pandaSpriteRenderer.gameObject.activeSelf == true)
        {
            _isPoyaSpriteActive = true;
        }

        // �Ǵ� ä�� �غ�
        _pandaCollectionAnim.SetInteger("Num", -1);
        _pandaCollectionAnim.Play("Idle");
        //_pandaCollectionAnim.enabled = false;
        _pandaSpriteRenderer.enabled = true;
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
        switch (_gatheringType)
        {
            case GatheringItemType.Bug:
                break;

            case GatheringItemType.Fish:
                SoundManager.Instance.PlayEffectAudio(_fishAnimeStartSound);
                break;

            case GatheringItemType.Fruit:
                break;
        }


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
        switch (_gatheringType)
        {
            case GatheringItemType.Bug:
                break;

            case GatheringItemType.Fish:
                SoundManager.Instance.PlayEffectAudio(_fishAnimeMiddleSound);
                SoundManager.Instance.PlayEffectAudio(_fishAnimeEndSound, 1.3f);
                break;

            case GatheringItemType.Fruit:
                break;
        }


        Debug.Log("_map" + _map);
        _pandaCollectionAnim.SetBool("IsCollectionLatency", true); // ä�� ��� ������ �� �ִϸ��̼�
        _checkTime = 0;
    }

    /// <summary>
    /// �������� �������� üũ </summary>
    public void IsSuccess()
    {
        float random = UnityEngine.Random.Range(0, 1f);

        CheckSuccessRate();



        if (_successRate < random)
        {
            Debug.Log("���� �ִϸ��̼�");
            _pandaCollectionAnim.SetTrigger("IsCollectionFail");
            IsSuccessCollection = false;
            if (_gatheringType == GatheringItemType.Fish)
            {
                StarterPanda.Instance.gameObject.transform.position += new Vector3(0, -0.5f, 0);
            }
            return;
        }
        Debug.Log("���� �ִϸ��̼�");

        _pandaCollectionAnim.SetTrigger("IsCollectionSuccess");
        if (_gatheringType == GatheringItemType.Fish)
        {
            StarterPanda.Instance.gameObject.transform.position += new Vector3(0, -0.5f, 0);
        }
        IsSuccessCollection = true;
    }

    /// <summary>
    /// �������� �� ������ �� �������� �� ���� ä�� - �켱 Bug </summary>
    private void CollectionSuccess()
    {
        // 1. ���� üũ 2. �ð� üũ 3. ��� üũ(����) 4. ������ ����(����)  

        // ���� ������ ���� �ð��� ����Ǿ��ٸ� ä���� �ð� ����
        TimeChanged();

        string collectionID = SetItem(); // ������ ����

        if(collectionID != null)
        {
            OnCollectionSuccess?.Invoke(collectionID, _gatheringType);

            // �κ��丮�� ������ �̵�
            GameManager.Instance.Player.AddItemById(collectionID, 1);


            // ���� ������Ʈ

            // ���� ���� �޼�
            DatabaseManager.Instance.Challenges.GatheringSuccess((int)_gatheringType);
        }
        else
        {
            CollectionFail();
        }
    }

    private void CollectionFail()
    {
        //_collectionAnim.SetBool("IsCollectionEnd", true);
        OnCollectionFail?.Invoke(_gatheringType);
    }

    private void ExitCollection()
    {
        _pandaCollectionAnim.SetBool("IsCollectionLatency", false);
        // ���� ���� ���·� ����
        //_pandaCollectionAnim.SetInteger("Num", StarterPanda.Instance.Num);
        _pandaCollectionAnim.enabled = false;
        _pandaSpriteRenderer.sprite = _pandaImage;
        if(!_isPoyaActive)
        {
            foreach (Transform child in _poyaAnimeCenter.transform)
            {
                if (child.name == _map)
                {
                    child.gameObject.SetActive(true);
                    break;
                }
            }
        }
        if (!_isPoyaSpriteActive)
        {
            _pandaSpriteRenderer.enabled = false;

        }

        StarterPanda.Instance.gameObject.transform.position = _lastPandaPosition;

        DataBind.GetAction("ShowMainUIButton")?.Invoke();
        CameraSet(false);

        // ������ �Ǵ� ������ ī�޶� �̵�
        //_targetPos = new Vector3(_lastPandaPosition.x, Camera.main.transform.position.y, Camera.main.transform.position.z);
        //Camera.main.gameObject.transform.position = _targetPos;
        _isClickStarButton = false;
    }

    /// <summary>
    /// ���� �ð��� ����Ǿ��ٸ� �׿� ���߾� ä���� �� �ֵ��� ���� </summary>
    private void TimeChanged()
    {
        if (_timeIds[5] != "GTS" + (21 + Hour)) // ���߿� _hour�� ���� ������ �ð� ������ ����
        {
            SetTime();

            if (_seasonIds[3] != Season)
            {
                SetSeason();
            }

            List<GatheringItem> getItemList = new List<GatheringItem>();
            switch (_gatheringType)
            {
                case GatheringItemType.Bug:
                    getItemList = DatabaseManager.Instance.ItemDatabase.ItemBugList;
                    break;
                case GatheringItemType.Fish:
                    getItemList = DatabaseManager.Instance.ItemDatabase.ItemFishList;
                    break;
                case GatheringItemType.Fruit:
                    getItemList = DatabaseManager.Instance.ItemDatabase.ItemFruitList;
                    break;
            }

            specialIDList.Clear();
            normalIDList.Clear();
            rareIDList.Clear();

            foreach (GatheringItem item in getItemList)
            {
                // ���� �ð��� ä�� �����ϴٸ�(1, 2 üũ)
                if (Array.Exists(_timeIds, time => time == item.Time) && Array.Exists(_seasonIds, season => season == item.Season) && item.Map == _map)
                {
                    Debug.Log("item name: " + item.Name);
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
        //�ð� ����
        _timeIds[0] = "GTS00";
        _timeIds[1] = "GTS0" + (1 + Hour / 12);
        _timeIds[2] = "GTS0" + (3 + Hour / 6);
        if (7 + Hour / 4 > 10)
        {
            _timeIds[3] = "GTS" + (7 + Hour / 4);
        }
        else
        {
            _timeIds[3] = "GTS0" + (7 + Hour / 4);
        }
        _timeIds[4] = "GTS" + (13 + Hour / 3);
        _timeIds[5] = "GTS" + (21 + Hour);
    }

    private void SetSeason()
    {
        _seasonIds[0] = "WAS";

        switch (Season)
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

        _seasonIds[3] = Season;
    }

    /// <summary>
    /// ������ ��� üũ �� ��޿� �´� ������ ���� </summary>
    private string SetItem()
    {
        float randomRarity = UnityEngine.Random.Range(0f, 1f); // 3. ��� ����
        int random;
        string collectionID;

        if(specialIDList.Count == 0 && rareIDList.Count == 0 && normalIDList.Count == 0)
        {
            Debug.LogError("ä�������� �������� �����ϴ�.");
            return null;
        }
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
            _successRate = 0.7f;
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
