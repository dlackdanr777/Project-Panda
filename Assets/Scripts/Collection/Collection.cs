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
    public Action OnExitCollection; // 채집 화면 종료
    public Action<float, GatheringItemType> OnCollectionButtonClicked;

    private bool _isClickStarButton;
    private bool _isCollection = false; // 채집 중인가?
    private bool _isExit = false;


    //[SerializeField] private CollectionButton _collectionButton;
    [SerializeField] private Animator _speechBubble;
    [SerializeField] private GameObject _starButton;
    private Animator _collectionAnim;


    #region 판다
    [Tooltip("판다 채집 이미지")]
    [SerializeField] private Sprite _pandaCollectionSprite;
    private Sprite _pandaImage;
    private Animator _pandaCollectionAnim;
    private SpriteRenderer _pandaSpriteRenderer;
    #endregion

    #region 위치 지정
    private Vector3 _targetPos; // 카메라 위치

    [Tooltip("채집과 판다 사이의 세부 위치 조정")]
    [SerializeField] private Vector3 _pandaPosition = new Vector3(-0.83f, -3.72f, 0); // 채집과 판다 위치 차이
    private Vector3 _lastPandaPosition; // 판다의 마지막 위치

    [Tooltip("채집 위치 지정")]
    [SerializeField] private Transform[] _collectionPosition;
    private Vector3 _currentCollectionPosition;
    #endregion

    #region 채집 관련 시간
    private float _waitTime; // 채집 기다린 시간
    [SerializeField] private float _spawnTime = 30f; // 채집 가능한 시간 간격 - 10초마다 채집 가능
    private float _fadeTime = 1f; // 화면 어두운 시간
    private float _collectionLatency = 1.7f; // 채집 완료 후 결과 출력 전까지 지연 시간(지연 시간 동안 애니메이션 재생)
    private float _checkTime = -1;
    #endregion

    #region 현재 계절, 시간, 맵
    // 현재 게임 시간, 계절 임의로 설정
    public int Hour => TimeManager.Instance.GameHour; // 나중에 _hour은 유저 정보의 시간 정보로 수정
    private string[] _timeIds = new string[6]; // 현재 채집 가능한 모든 시간 ID
    public string Season => TimeManager.Instance.GameWeatherId; // 나중에 _season은 유저 정보의 시간 정보로 수정
    private string[] _seasonIds = new string[4]; // 현재 채집 가능한 모든 계절 ID

    [SerializeField] private string _map;
    #endregion

    #region 현재 채집 가능한 아이템 리스트
    private List<string> normalIDList = new List<string>();
    private List<string> rareIDList = new List<string>();
    private List<string> specialIDList = new List<string>();
    #endregion

    /// <summary> 현재 채집 종류 </summary>
    [SerializeField] private GatheringItemType _gatheringType;
    private string[] _isCollecting = { "IsCollectingBug", "IsCollectingFish", "IsCollectingFruit" };

    [SerializeField] private string toolId; // 나중에 플레이어 도구 정보로 수정

    // 아이템 등급 확률
    private float normal = 0.87f;
    private float rare = 0.1f;

    #region 채집 성공 체크
    private float _successRate;
    private bool _isSuccessCollection;
    public bool IsSuccessCollection 
    {
        get { return _isSuccessCollection; }
        set
        {
            _isSuccessCollection = value;

            // 채집 성공: 애니메이션 실행 + 텍스트 띄워줌
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
        _collectionAnim = _starButton.GetComponent<Animator>();
        //_collectionButton.OnCollectionButtonClicked += ClickCollectionButton;

        string starButton = "";
        switch (_gatheringType)
        {
            case GatheringItemType.Bug:
                starButton = "BugStarButton";
                break;
            case GatheringItemType.Fish:
                starButton = "FishStarButton";
                break;
            case GatheringItemType.Fruit:
                starButton = "FruitStarButton";
                break;
        }
        DataBind.SetButtonValue(starButton, ClickStarButton);

    }
    //private void OnDestroy()
    //{
    //    _collectionButton.OnCollectionButtonClicked -= ClickCollectionButton;

    //}

    private void Update()
    {
        // 채집 대기시간 체크
        if (_waitTime < _spawnTime && !_isCollection && !_isExit)
        {
            _waitTime += Time.deltaTime;
        }
        else if(_waitTime >= _spawnTime)
        {
            _waitTime = 0;
            
            // 이번 채집 위치 설정
            _currentCollectionPosition = _collectionPosition[UnityEngine.Random.Range(0, _collectionPosition.Length)].position;
            gameObject.transform.position = _currentCollectionPosition;

            _starButton.SetActive(true);
            _collectionAnim.enabled = true;
        }

        if(_isCollection)
        {
            // 채집 지연시간이 아니면 _checkTime = -1
            if (_checkTime >= 0)
            {
                _checkTime += Time.deltaTime;
                if( _checkTime >= _collectionLatency) // 채집 지연시간이 지났는지 확인
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
        OnCollectionButtonClicked?.Invoke(_fadeTime, _gatheringType); // 화면 FadeOut
        ClickCollectionButton();

        //// 채집 가능 아이콘 생성
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
        if(_pandaCollectionAnim == null)
        {
            _pandaCollectionAnim = starterPanda.GetComponent<Animator>();
        }

        // 캐릭터가 채집 포인트로 이동
        _lastPandaPosition = starterPanda.gameObject.transform.position;
        starterPanda.gameObject.transform.position = _currentCollectionPosition + _pandaPosition;

        _pandaSpriteRenderer.sprite = _pandaCollectionSprite;

        // 카메라 중심이 캐릭터로 고정되도록 이동
        _targetPos = new Vector3(starterPanda.transform.position.x, starterPanda.transform.position.y, Camera.main.transform.position.z);
        Camera.main.gameObject.transform.position = _targetPos;

        _isCollection = true;

        _collectionAnim.enabled = false;

        // 화면 켜지는 시간에 맞추어 채집 시작
        Invoke("StartCollection", _fadeTime);
    }

    /// <summary>
    /// 카메라 설정 </summary>
    public void CameraSet(bool set)
    {
        GameManager.Instance.FriezeCameraMove = set;
        GameManager.Instance.FriezeCameraZoom = set;
        GameManager.Instance.FirezeInteraction = set;
        //Camera.main.gameObject.transform.position = _targetPos;
    }

    /// <summary>
    /// 채집 시작할 때 실행 </summary>
    private void StartCollection()
    {
        // 채집 애니메이션 판다와 말풍선 실행
        _pandaCollectionAnim.enabled = true;
        _pandaCollectionAnim.SetTrigger(_isCollecting[(int)_gatheringType]);

        _speechBubble.gameObject.SetActive(true);
        _speechBubble.enabled = true;
    }

    /// <summary>
    /// 채집 완료 후 결과 나오기 전 지연 시간동안 실행 </summary>
    public void CollectionLatency()
    {
        _pandaCollectionAnim.SetBool("IsCollectionLatency", true); // 채집 결과 나오기 전 애니메이션
        _checkTime = 0;
    }

    /// <summary>
    /// 성공인지 실패인지 체크 </summary>
    public void IsSuccess()
    {
        float random = UnityEngine.Random.Range(0, 1f);

        CheckSuccessRate();

        if (_successRate < random)
        {
            Debug.Log("실패 애니메이션");
            _pandaCollectionAnim.SetTrigger("IsCollectionFail");
            IsSuccessCollection = false;
            return;
        }
        Debug.Log("성공 애니메이션");

        _pandaCollectionAnim.SetTrigger("IsCollectionSuccess");
        IsSuccessCollection = true;
    }

    /// <summary>
    /// 성공했을 시 아이템 중 랜덤으로 한 가지 채집 - 우선 Bug </summary>
    private void CollectionSuccess()
    {
        // 1. 계절 체크 2. 시간 체크 3. 등급 체크(랜덤) 4. 아이템 결정(랜덤)  

        // 유저 정보를 통해 시간이 변경되었다면 채집의 시간 수정
        TimeChanged();

        string collectionID = SetItem(); // 아이템 결정

        OnCollectionSuccess?.Invoke(collectionID, _gatheringType);

        // 인벤토리로 아이템 이동
        GameManager.Instance.Player.AddItemById(collectionID, 1);


        // 도감 업데이트

        // 도전 과제 달성
        DatabaseManager.Instance.Challenges.GatheringSuccess((int)_gatheringType);
    }

    private void CollectionFail()
    {
        //_collectionAnim.SetBool("IsCollectionEnd", true);
        OnCollectionFail?.Invoke(_gatheringType);
    }

    private void ExitCollection()
    {
        _pandaCollectionAnim.enabled = false;
        _pandaSpriteRenderer.sprite = _pandaImage;

        StarterPanda.Instance.gameObject.transform.position = _lastPandaPosition;

        DataBind.GetAction("ShowMainUIButton")?.Invoke();
        CameraSet(false);
        _targetPos = new Vector3(_lastPandaPosition.x, _lastPandaPosition.y, Camera.main.transform.position.z);
        Camera.main.gameObject.transform.position = _targetPos;
        _isClickStarButton = false;
    }

    /// <summary>
    /// 게임 시간이 변경되었다면 그에 맞추어 채집할 수 있도록 설정 </summary>
    private void TimeChanged()
    {
        if (_timeIds[5] != "GTS" + (21 + Hour)) // 나중에 _hour는 유저 정보의 시간 정보로 수정
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

            normalIDList.Clear();
            rareIDList.Clear();

            foreach (GatheringItem item in getItemList)
            {
                // 현재 시간에 채집 가능하다면(1, 2 체크)
                if (Array.Exists(_timeIds, time => time == item.Time) && Array.Exists(_seasonIds, season => season == item.Season) && item.Map == _map)
                {
                    Debug.Log("item name: " + item.Name);
                    if (item.Rank == "스페셜")
                    {
                        specialIDList.Add(item.Id);
                    }
                    else if (item.Rank == "레어")
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
        //시간 설정
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
    /// 아이템 등급 체크 후 등급에 맞는 아이템 결정 </summary>
    private string SetItem()
    {
        float randomRarity = UnityEngine.Random.Range(0f, 1f); // 3. 등급 결정
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
            random = UnityEngine.Random.Range(0, normalIDList.Count); // 4. 아이템 결정
            collectionID = normalIDList[random];
        }
        else if (randomRarity < normal + rare)
        {
            random = UnityEngine.Random.Range(0, rareIDList.Count);
            collectionID = rareIDList[random];
        }
        else
        {
            Debug.Log("스페셜");
            random = UnityEngine.Random.Range(0, rareIDList.Count);
            collectionID = rareIDList[random];
        }

        return collectionID;
    }

    private void CheckSuccessRate()
    {

        if (_gatheringType == GatheringItemType.Bug)
        {
            // 플레이어 도구에 따라 확률 변경하기
            switch (toolId)
            {
                case "ITG02":
                    _successRate = 0.6f;
                    break;
                case "ITG04":
                    _successRate = 0.7f;
                    break;
                case "ITG06":
                    _successRate = 0.8f;
                    break;
            }

        }
        else if (_gatheringType == GatheringItemType.Fish)
        {
            switch (toolId)
            {
                case "ITG01":
                    _successRate = 0.6f;
                    break;
                case "ITG03":
                    _successRate = 0.7f;
                    break;
                case "ITG05":
                    _successRate = 0.8f;
                    break;
            }
        }
        else if (_gatheringType == GatheringItemType.Fruit)
        {
            _successRate = 1f;
        }
    }
}
