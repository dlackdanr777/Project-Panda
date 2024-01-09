using BT;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Collection : MonoBehaviour, IInteraction
{
    private bool _isCollection = false; // 채집 중인가?
    private bool _isExit = false;

    #region 현재 계절, 시간
    // 현재 게임 시간, 계절 임의로 설정
    [SerializeField] private int _hour; // 나중에 _hour은 유저 정보의 시간 정보로 수정
    [SerializeField] private string[] _timeIDs = new string[6]; // 현재 채집 가능한 모든 시간 ID
    [SerializeField] private string _season; // 나중에 _season은 유저 정보의 시간 정보로 수정
    [SerializeField] private string[] _seasonIDs = new string[4]; // 현재 채집 가능한 모든 계절 ID
    #endregion

    #region 현재 채집 가능한 아이템 리스트
    private List<string> normalBugIDList = new List<string>();
    private List<string> rareBugIDList = new List<string>();
    #endregion

    #region 채집 관련 시간
    private float _waitTime; // 채집 기다린 시간
    private float _spawnTime = 30f; // 채집 가능한 시간 간격 - 10초마다 채집 가능
    private float _fadeTime = 1f; // 화면 어두운 시간
    private float _collectionLatency = 4f; // 채집 완료 후 결과 출력 전까지 지연 시간(지연 시간 동안 애니메이션 재생)
    private float _checkTime = -1;
    #endregion

    /// <summary> 현재 채집 종류 </summary>
    private int fieldIndex = -1;

    #region 위치 지정
    private Vector3 _targetPos;
    private Vector3 CollectionPosition = new Vector3(-3.4f, -14f, 0);
    #endregion

    private Sprite _pandaImage;
    [SerializeField] private Sprite _pandaCollectionImage;

    private Animator _collectionAnim;
    [SerializeField] private CollectionButton _collectionButton;
    [SerializeField] private GameObject _speechBubble;

    public Action<string> OnCollectionSuccess;
    public Action<string> OnSuccessFrame;
    public Action OnCollectionFail;
    public Action OnExitCollection; // 채집 화면 종료
    public Action<float> OnCollectionButtonClicked;

    // 나중에 스페셜이 추가된다면 레어&스페셜 추가하기
    private float normal = 0.87f;

    #region 채집 성공 체크
    private float _successRate = 0.8f;
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
        _waitTime = 29;
        //_collectionButton.OnCollectionButtonClicked += ClickCollectionButton;
        OnCollectionButtonClicked += ClickCollectionButton;

    }
    private void OnDestroy()
    {
        //_collectionButton.OnCollectionButtonClicked -= ClickCollectionButton;
        OnCollectionButtonClicked -= ClickCollectionButton;
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
            GetComponent<SpriteRenderer>().enabled = true;
            GetComponent<Animator>().enabled = true;
            GetComponent<BoxCollider2D>().enabled = true;
        }

        // 채집 중이면 카메라 잠금
        if(_isCollection)
        {
            CameraLock();

            // 채집 지연시간이 아니면 _time = -1
            if(_checkTime >= 0)
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
            _isExit = false;
            _collectionAnim.SetBool("IsCollectionLatency", false);
            Invoke("ExitCollection", 0.4f);
            OnExitCollection?.Invoke();
        }
    }

    public void StartInteraction()
    {
        OnCollectionButtonClicked?.Invoke(_fadeTime); // 화면 FadeOut

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


    private void ClickCollectionButton(float fadeTime)
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
        starterPanda.gameObject.transform.position = CollectionPosition;

        // 판다 채집 이미지로 변경
        DatabaseManager.Instance.StartPandaInfo.StarterPanda.gameObject.GetComponent<SpriteRenderer>().sprite = _pandaCollectionImage;
        Debug.Log("판다 이미지 변경 완 " + DatabaseManager.Instance.StartPandaInfo.StarterPanda.gameObject.GetComponent<SpriteRenderer>().sprite.name);

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
        _collectionAnim.SetTrigger("IsCollecting");

        _speechBubble.SetActive(true);
        _speechBubble.GetComponent<Animator>().enabled = true;
    }

    /// <summary>
    /// 채집 완료 후 결과 나오기 전 지연 시간동안 실행 </summary>
    public void CollectionLatency()
    {
        _collectionAnim.SetBool("IsCollectionLatency", true); // 채집 결과 나오기 전 애니메이션
        _checkTime = 0;
    }

    /// <summary>
    /// 성공인지 실패인지 체크 </summary>
    public void IsSuccess()
    {
        float random = UnityEngine.Random.Range(0, 1f);
        Debug.Log("랜덤 값: "+ random);
        if (_successRate < random)
        {
            _collectionAnim.SetTrigger("IsCollectionFail");
            IsSuccessCollection = false;
            return;
        }
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
        if (_timeIDs[5] != "GTS" + (21 + _hour)) // 나중에 _hour는 유저 정보의 시간 정보로 수정
        {
            // 시간 설정
            SetTime();

            if (_seasonIDs[3] != _season)
            {
                // 계절 설정
                SetSeason();
            }

            foreach (Item item in DatabaseManager.Instance.GetBugItemList())
            {
                if (item is GatheringItem gatheringItem)
                {
                    // 현재 시간에 채집 가능하다면(1, 2 체크)
                    if (Array.Exists(_timeIDs, time => time == gatheringItem.Time) && Array.Exists(_seasonIDs, season => season == gatheringItem.Season))
                    {
                        if(item.Rank == "레어")
                        {
                            rareBugIDList.Add(item.Id);
                        }
                        else
                        {
                            normalBugIDList.Add(item.Id);
                        }
                    }
                }
            }
        }

        float randomRarity = UnityEngine.Random.Range(0f, 1f); // 3. 등급 결정
        int random;
        string collectionID;

        if (randomRarity < normal ) 
        {
            Debug.Log("normal");
            random = UnityEngine.Random.Range(0, normalBugIDList.Count); // 4. 아이템 결정
            collectionID = normalBugIDList[random];
        }
        else
        {
            random = UnityEngine.Random.Range(0, rareBugIDList.Count); // 4. 아이템 결정
            collectionID = rareBugIDList[random];
        }

        OnCollectionSuccess?.Invoke(collectionID); // 애니메이션 종료 후 실행하는 것으로 수정해야 함

        fieldIndex = 0; // 곤충은 0번

        // 인벤토리로 아이템 이동
        GameManager.Instance.Player.GatheringItemInventory[fieldIndex].AddById(InventoryItemField.GatheringItem, fieldIndex, collectionID);


        // 도감 업데이트

    }

    private void CollectionFail()
    {
        //_collectionAnim.SetBool("IsCollectionEnd", true);
        OnCollectionFail?.Invoke();
    }

    private void ExitCollection()
    {
        _collectionAnim.enabled = false;
        _collectionAnim.gameObject.GetComponent<SpriteRenderer>().sprite = _pandaImage;
    }

    private void SetTime()
    {
        // 시간 설정
        _timeIDs[0] = "GTS00";
        _timeIDs[1] = "GTS0" + (1 + _hour / 12);
        _timeIDs[2] = "GTS0" + (3 + _hour / 6);
        if(7 + _hour / 4 > 10)
        {
            _timeIDs[3] = "GTS" + (7 + _hour / 4);
        }
        else
        {
            _timeIDs[3] = "GTS0" + (7 + _hour / 4);
        }
        _timeIDs[4] = "GTS" + (13 + _hour / 3);
        _timeIDs[5] = "GTS" + (21 + _hour);
    }

    private void SetSeason()
    {
        _seasonIDs[0] = "WAS";

        switch (_season)
        {
            case "WSP":
                _seasonIDs[1] = "WSS";
                _seasonIDs[2] = "WWS";
                break;
            case "WSU":
                _seasonIDs[1] = "WSS";
                _seasonIDs[2] = "WSF";
                break;
            case "WFA":
                _seasonIDs[1] = "WFW";
                _seasonIDs[2] = "WSF";
                break;
            case "WWT":
                _seasonIDs[1] = "WFW";
                _seasonIDs[2] = "WWS";
                break;
        }

        _seasonIDs[3] = _season;
    }
}
