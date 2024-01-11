using BT;
using System;
using System.Collections.Generic;
using UnityEngine;


public class Collection : MonoBehaviour, IInteraction
{
    private bool _isCollection = false; // 채집 중인가?
    private bool _isExit = false;

    #region 현재 계절, 시간, 맵
    // 현재 게임 시간, 계절 임의로 설정
    [SerializeField] 
    private int _hour; // 나중에 _hour은 유저 정보의 시간 정보로 수정
    private string[] _timeIds = new string[6]; // 현재 채집 가능한 모든 시간 ID
    [SerializeField] 
    private string _season; // 나중에 _season은 유저 정보의 시간 정보로 수정
    private string[] _seasonIds = new string[4]; // 현재 채집 가능한 모든 계절 ID

    [SerializeField] private string _map;
    #endregion

    [SerializeField] private string toolId; // 나중에 플레이어 도구 정보로 수정

    #region 현재 채집 가능한 아이템 리스트
    private List<string> normalIDList = new List<string>();
    private List<string> rareIDList = new List<string>();
    private List<string> specialIDList = new List<string>();
    #endregion

    #region 채집 관련 시간
    private float _waitTime; // 채집 기다린 시간
    [SerializeField] private float _spawnTime = 30f; // 채집 가능한 시간 간격 - 10초마다 채집 가능
    private float _fadeTime = 1f; // 화면 어두운 시간
    private float _collectionLatency = 4f; // 채집 완료 후 결과 출력 전까지 지연 시간(지연 시간 동안 애니메이션 재생)
    private float _checkTime = -1;
    #endregion

    /// <summary> 현재 채집 종류 </summary>
    [SerializeField] private GatheringItemType _gatheringType;
    private string[] _isCollecting = { "IsCollectingBug", "IsCollectingFish", "IsCollectingFruit" };


    #region 위치 지정
    private Vector3 _targetPos; // 카메라 위치

    [SerializeField] private Vector3 _pandaPosition = new Vector3(-0.83f, -3.72f, 0); // 채집과 판다 위치 차이
    private Vector3 _lastPandaPosition; // 판다의 마지막 위치

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
    public Action OnExitCollection; // 채집 화면 종료
    public Action<float, GatheringItemType> OnCollectionButtonClicked;

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
        //_collectionButton.OnCollectionButtonClicked += ClickCollectionButton;

    }
    private void OnDestroy()
    {
        //_collectionButton.OnCollectionButtonClicked -= ClickCollectionButton;

    }

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

            GetComponent<SpriteRenderer>().enabled = true;
            GetComponent<Animator>().enabled = true;
            GetComponent<BoxCollider2D>().enabled = true;
        }

        // 채집 중이면 카메라 잠금
        if(_isCollection)
        {
            CameraLock();

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
            Debug.Log("채집 지연 애니메이션 false");
            _collectionAnim.SetBool("IsCollectionLatency", false);
            Invoke("ExitCollection", _fadeTime);
            OnExitCollection?.Invoke();
        }
    }

    public void StartInteraction()
    {
        OnCollectionButtonClicked?.Invoke(_fadeTime, _gatheringType); // 화면 FadeOut
        ClickCollectionButton();

        //// 채집 가능 아이콘 생성
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

        // 캐릭터가 채집 포인트로 이동
        _lastPandaPosition = starterPanda.gameObject.transform.position;
        starterPanda.gameObject.transform.position = _currentCollectionPosition + _pandaPosition;

        starterPanda.GetComponent<SpriteRenderer>().sprite = _collectionSprite;

        // 카메라 중심이 캐릭터로 고정되도록 이동
        _targetPos = new Vector3(starterPanda.transform.position.x, starterPanda.transform.position.y, Camera.main.transform.position.z);
        Camera.main.gameObject.transform.position = _targetPos;

        _isCollection = true;
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Animator>().enabled = false;

        // 화면 켜지는 시간에 맞추어 채집 시작
        Invoke("StartCollection", _fadeTime);
    }

    /// <summary>
    /// 카메라 움직이지 못하게 설정 </summary>
    public void CameraLock()
    {
        Camera.main.gameObject.transform.position = _targetPos;
    }

    /// <summary>
    /// 채집 시작할 때 실행 </summary>
    private void StartCollection()
    {
        // 채집 애니메이션 판다와 말풍선 실행
        _collectionAnim.enabled = true;
        _collectionAnim.SetTrigger(_isCollecting[(int)_gatheringType]);

        _speechBubble.SetActive(true);
        _speechBubble.GetComponent<Animator>().enabled = true;
    }

    /// <summary>
    /// 채집 완료 후 결과 나오기 전 지연 시간동안 실행 </summary>
    public void CollectionLatency()
    {
        Debug.Log("채집 지연 애니메이션 true");

        _collectionAnim.SetBool("IsCollectionLatency", true); // 채집 결과 나오기 전 애니메이션
        _checkTime = 0;
        Debug.Log("_checktime :"+_checkTime);
    }

    /// <summary>
    /// 성공인지 실패인지 체크 </summary>
    public void IsSuccess()
    {
        float random = UnityEngine.Random.Range(0, 1f);
        Debug.Log("랜덤 값: "+ random);

        CheckSuccessRate();

        if (_successRate < random)
        {
            Debug.Log("실패 애니메이션");
            _collectionAnim.SetTrigger("IsCollectionFail");
            IsSuccessCollection = false;
            return;
        }
        Debug.Log("성공 애니메이션");

        _collectionAnim.SetTrigger("IsCollectionSuccess");
        IsSuccessCollection = true;
    }

    /// <summary>
    /// 성공했을 시 아이템 중 랜덤으로 한 가지 채집 - 우선 Bug </summary>
    private void CollectionSuccess()
    {
        // 계절과 시간을 고려해서 그 중 랜덤으로 결정해야 함 + 등급
        // 1. 계절 체크 2. 시간 체크 3. 등급 체크(랜덤) 4. 아이템 결정(랜덤)  

        // 유저 정보를 통해 시간이 변경되었다면 채집의 시간 수정
        TimeChanged();

        string collectionID = SetItem(); // 아이템 결정

        OnCollectionSuccess?.Invoke(collectionID, _gatheringType); // 애니메이션 종료 후 실행하는 것으로 수정해야 함

        // 인벤토리로 아이템 이동
        GameManager.Instance.Player.GatheringItemInventory[(int)_gatheringType].AddById(InventoryItemField.GatheringItem, (int)_gatheringType, collectionID);


        // 도감 업데이트

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
    /// 게임 시간이 변경되었다면 그에 맞추어 채집할 수 있도록 설정 </summary>
    private void TimeChanged()
    {
        if (_timeIds[5] != "GTS" + (21 + _hour)) // 나중에 _hour는 유저 정보의 시간 정보로 수정
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
                // 현재 시간에 채집 가능하다면(1, 2 체크)
                if (Array.Exists(_timeIds, time => time == item.Time) && Array.Exists(_seasonIds, season => season == item.Season) && item.Map == _map)
                {
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
        // 시간 설정
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
