using BT;
using System;
using UnityEngine;

public class Collection : MonoBehaviour, IInteraction
{
    private bool _isCollection = false; // 채집 중인가?
    private bool _isExit = false;

    private float _time;
    private float _spawnTime = 30f; // 채집 가능한 시간 간격 - 10초마다 채집 가능
    private float _fadeTime; // 화면 어두운 시간

    #region 위치 지정
    private Vector3 _targetPos;
    private Vector3 CollectionPosition = new Vector3(-3.4f, -14f, 0);
    #endregion

    private Sprite _pandaImage;
    private Animator _collectionAnim;
    [SerializeField] private CollectionButton _collectionButton;
    [SerializeField] private GameObject _speechBubble;

    public Action OnCollectionSuccess;
    public Action OnCollectionFail;
    public Action OnExitCollection; // 채집 화면 종료

    #region 채집 성공 체크
    private float _successRate = 0.7f;
    private bool _isSuccessCollection;
    public bool IsSuccessCollection
    {
        get { return _isSuccessCollection; }
        set
        {
            // 채집 성공
            if (_isSuccessCollection)
            {
                Debug.Log("채집 성공");
                OnCollectionSuccess?.Invoke();
            }
            else
            {
                Debug.Log("채집 실패");
                OnCollectionFail?.Invoke();
            }
            _isCollection = false;
            _isExit = true;
        }
    }
    #endregion

    private void Start()
    {
        _time = 29;
        _collectionButton.OnCollectionButtonClicked += ClickCollectionButton;
        OnCollectionSuccess += CollectionSuccess;
        OnCollectionFail += CollectionFail;

    }
    private void OnDestroy()
    {
        _collectionButton.OnCollectionButtonClicked -= ClickCollectionButton;
        OnCollectionSuccess -= CollectionSuccess;
        OnCollectionFail -= CollectionFail;
    }

    private void Update()
    {
        if (_time < _spawnTime)
        {
            _time += Time.deltaTime;
        }
        else
        {
            _time = 0;
            GetComponent<SpriteRenderer>().enabled = true;
            GetComponent<BoxCollider2D>().enabled = true;
        }

        // 채집 중이면 카메라 잠금
        if(_isCollection)
        {
            CameraLock();
        }
        else if(Input.GetMouseButtonDown(0) && _isExit)
        {
            _isExit = false;
            OnExitCollection?.Invoke();
            ExitCollection();
        }
    }

    public void StartInteraction()
    {
        // 채집 가능 아이콘 생성
        _collectionButton.gameObject.SetActive(true);
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
        _fadeTime = fadeTime;
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

        // 카메라가 캐릭터가 중앙으로 고정되도록 이동
        _targetPos = new Vector3(starterPanda.transform.position.x, starterPanda.transform.position.y, Camera.main.transform.position.z);
        Camera.main.gameObject.transform.position = _targetPos;

        _isCollection = true;
        GetComponent<SpriteRenderer>().enabled = false;

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
        Debug.Log("채집시작함니다 ~~");

        // 채집 애니메이션 판다와 말풍선 실행
        _collectionAnim.enabled = true;
        _collectionAnim.SetTrigger("IsCollecting");
        _speechBubble.SetActive(true);
        _speechBubble.GetComponent<Animator>().enabled = true;

        // 말풍선 ...이 끝나면 느낌표 표시
        // 느낌표 터치하면 채집 성공 실패 여부 알려줌
        // 애니메이션 실행 + 텍스트 띄워줌
        // 인벤토리와 도감으로 정보 전달

        // 채집 종료
        //IsCollection = false;
    }

    /// <summary>
    /// 성공인지 실패인지 체크 </summary>
    public void IsSuccess()
    {
        Debug.Log("성공인지 확인");
        float random = UnityEngine.Random.Range(0, 1);
        if (_successRate < random)
        {
            IsSuccessCollection = false;
        }
        IsSuccessCollection = true;
    }


    private void CollectionSuccess()
    {
        _collectionAnim.SetBool("IsCollectionEnd", true); // 채집 완료 시 실행
        // 인벤토리로 아이템 이동
        // 도감 업데이트
    }

    private void CollectionFail()
    {
        _collectionAnim.SetBool("IsCollectionEnd", true);
    }

    private void ExitCollection()
    {
        _collectionAnim.SetBool("IsCollectionEnd", false);

        _collectionAnim.enabled = false; // 끊겨서 그냥 판다 애니메이션 종료될 때 조건 체크한 다음 enable = false 하는게 나을 거 같음..
        _collectionAnim.gameObject.GetComponent<SpriteRenderer>().sprite = _pandaImage;
    }

}
