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
    public Action OnExitCollection; // 채집 화면 종료
    public Action<float, GatheringItemType, string> OnCollectionButtonClicked;

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
    [SerializeField] private Transform _pandaTransform; // 채집과 판다 위치 차이
    private Vector3 _lastPandaPosition; // 판다의 마지막 위치

    [Tooltip("채집 위치 지정")]
    [SerializeField] private Transform _collectionPosition;
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


    public string CollectionID { get; set; } // 채집해야 할 아이템 ID;
    public string MainStoryID { get; set; } // 이 메인스토리가 끝났을 경우 채집 실행
    public string NextMainStoryID { get; set; } // 이 메인스토리가 끝났을 경우 채집 종료

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
        // 채집 아이템이 가방에 있다면 채집 실행 안되도록 하기
        if (!string.IsNullOrEmpty(CollectionID) && GameManager.Instance.Player.FindItemById(CollectionID))
        {
            _waitTime = 0;
        }

        // 다음 메인스토리가 종료되었을 경우 스크립트 종료하기
        if (!string.IsNullOrEmpty(NextMainStoryID) && !string.IsNullOrEmpty(DatabaseManager.Instance.MainDialogueDatabase.StoryCompletedList.Find(x => x == NextMainStoryID)))
        {
            gameObject.SetActive(false);
        }

        // 채집 대기시간 체크
        if (_waitTime < _spawnTime && !_isCollection && !_isExit)
        {
            _waitTime += Time.deltaTime;
        }
        else if (_waitTime >= _spawnTime && !string.IsNullOrEmpty(CollectionID) && (_gatheringType == GatheringItemType.Fruit || GameManager.Instance.Player.FindItemById(toolId)))
        {
            _waitTime = 0;

            // 채집 위치 설정
            _currentCollectionPosition = _collectionPosition.position;
            gameObject.transform.position = _currentCollectionPosition;

            _starButton.SetActive(true);
            _collectionAnim.enabled = true;
        }

        if (_isCollection)
        {
            // 채집 지연시간이 아니면 _checkTime = -1
            if (_checkTime >= 0)
            {
                _checkTime += Time.deltaTime;
                if (_checkTime >= _collectionLatency) // 채집 지연시간이 지났는지 확인
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
        OnCollectionButtonClicked?.Invoke(_fadeTime, _gatheringType, MainStoryID); // 화면 FadeOut
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

        if (_pandaCollectionAnim == null)
        {
            _pandaCollectionAnim = starterPanda.GetComponent<Animator>();
            Debug.Log("_pandaCollectionAnim" + _pandaCollectionAnim);
        }

        // 캐릭터가 채집 포인트로 이동
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

        // 카메라 중심이 캐릭터로 고정되도록 이동
        _targetPos = new Vector3(starterPanda.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z);
        Camera.main.gameObject.transform.position = _targetPos;

        _isCollection = true;

        _collectionAnim.enabled = false;

        // 화면 켜지는 시간에 맞추어 채집 시작
        Invoke("StartCollection", _fadeTime);

        // 진행 중이던 애니메이션 종료
        _pandaCollectionAnim.SetInteger("Num", -1);
        _pandaCollectionAnim.Play("Idle");
        _pandaCollectionAnim.enabled = false;
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
        StarterPanda starterPanda = StarterPanda.Instance;
        // 말풍선 위치 고정
        gameObject.transform.position = new Vector3(starterPanda.transform.position.x, starterPanda.transform.position.y + 3, gameObject.transform.position.z);

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
        Debug.Log("_map" + _map);
        _pandaCollectionAnim.SetBool("IsCollectionLatency", true); // 채집 결과 나오기 전 애니메이션
        _checkTime = 0;
    }

    /// <summary>
    /// 성공인지 실패인지 체크</summary>
    public void IsSuccess()
    {
        // 퀘스트 아이템은 무조건 성공
        Debug.Log("성공 애니메이션");

        _pandaCollectionAnim.SetTrigger("IsCollectionSuccess");
        IsSuccessCollection = true;
    }

    /// <summary>
    /// 성공했을 시 퀘스트 아이템 채집
    private void CollectionSuccess()
    {
        string collectionID = CollectionID; // 정해진 아이템 채집

        if (collectionID != null)
        {
            OnCollectionSuccess?.Invoke(collectionID);

            // 인벤토리로 아이템 이동
            GameManager.Instance.Player.AddItemById(collectionID, 1);


            // 도감 업데이트

            // 도전 과제 달성
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
