using UnityEngine;


public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    [Space]
    [Tooltip("카메라 확대/축소 속도")]
    [SerializeField] private float _zoomSpeed;
    public float ZoomSpeed => _zoomSpeed;

    [Tooltip("카메라 확대 최대 크기")]
    [SerializeField] private float _maxZoomSize;
    public float MaxZoomSize => _maxZoomSize;

    [Tooltip("카메라 축소 최소 크기")]
    [SerializeField] private float _minZoomSize;
    public float MinZoomSize => _minZoomSize;

    [Tooltip("카메라 이동 범위")]
    [SerializeField] private Vector2 _mapSize;
    public Vector2 MapSize
    {
        get { return _mapSize; }
        set { _mapSize = value; }
    }

    [Tooltip("카메라 이동 범위 센터")]
    [SerializeField] private Vector2 _mapCenter;

    [Space]
    [Tooltip("카메라 가속 배율")]
    [SerializeField] private float _accelerationRate;

    [Tooltip("카메라 감속 배율")]
    [SerializeField] private float _decelerationRate;

    public Vector2 MapCenter
    {
        get { return _mapCenter; }
        set { _mapCenter = value; }
    }

    private Vector3 _tmpTouchPos;

    private Vector3 _tmpCameraPos;

    private Vector3 _distanceMoved;

    private Vector3 _lastPos;

    private Vector3 _velocity;

    private float _touchDownTimer;

    private float _height;

    private float _width;

    private IInteraction _currentInteraction;

    private IInteraction _tempInteaction;

    private bool _touchEnabled; 


    private void Update()
    {

#if UNITY_EDITOR

        MouseMovement();
        MouseZoomInOut();


#elif PLATFORM_ANDROID

        TouchMovement();
        TouchZoomInOut();
        
#endif

        RunningAcceleration();
        TouchInteraction();
        _currentInteraction?.UpdateInteraction();
    }


    private void Start()
    {
        _height = Camera.main.orthographicSize;
        _width = _height * Screen.width / Screen.height;
    }



    RaycastHit2D _hit;

    private void TouchInteraction()
    {
        if (GameManager.Instance.FirezeInteraction)
            return;

        if (Input.GetMouseButton(0))
        {
            Vector2 touchPos = _camera.ScreenToWorldPoint(Input.mousePosition);
            _hit = Physics2D.Raycast(touchPos, Vector2.zero, 10f);
        }

        //마우스를 눌렀을때 레이를 쏘고 레이를 쏜곳에 IInteraction을 가진 오브젝트가 있을때 
        //IInteraction을 임시 변수에 담아놓고 마우스 버튼을 땟을때 임시 변수와 같은 오브젝트 일 경우 실행하게 했습니다.
        if (Input.GetMouseButtonDown(0))
        {
            if (_hit.collider == null)
            {
                _tempInteaction = null;
                return;
            }

            if (!_hit.collider.TryGetComponent(out IInteraction interaction))
            {
                _tempInteaction = null;
                return;
            }
                
            _tempInteaction = interaction;
        }


        if (Input.GetMouseButtonUp(0))
        {

            if (_hit.collider == null)
            {
                return;
            }
                
            if (!_hit.collider.TryGetComponent(out IInteraction interaction))
                return;

            if(_tempInteaction == interaction)
            {
                interaction.StartInteraction();
               // _currentInteraction = interaction;
                _tempInteaction = null;
            }
        }
    }


    private void TouchMovement()
    {
        if (GameManager.Instance.FriezeCameraMove)
        {
            _touchEnabled = false;
            ResetAcceleration();
            return;
        }

        if (Input.touchCount != 1)
            return;

        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            //화면 터치시 카메라, 클릭 위치 값을 저장해둔다.
            //화면을 꾹 누르고 있을 때 위치를 이동시키기 위함
            _tmpTouchPos = touch.position;
            _tmpCameraPos = _camera.transform.position;
            _touchEnabled = true;

            ResetAcceleration();
        }

        else if (touch.phase == TouchPhase.Moved && _touchEnabled)
        {

            Vector3 movePos = Camera.main.ScreenToViewportPoint(_tmpTouchPos - (Vector3)touch.position);
            _camera.transform.position = LimitPos(_tmpCameraPos + movePos * _camera.orthographicSize);

            CheckAcceleration();
        }

        else if (touch.phase == TouchPhase.Ended)
        {
            _touchEnabled = false;
            //터치를 뗀 경우 가속도를 적용한다.
            SetAcceleration(_distanceMoved);
        }

    }


    private void TouchZoomInOut()
    {
        if (GameManager.Instance.FriezeCameraZoom)
            return;

        if (Input.touchCount != 2)
            return;

        Touch touchZero = Input.GetTouch(0); //첫번째 손가락 터치를 저장
        Touch touchOne = Input.GetTouch(1); //두번째 손가락 터치를 저장

        //터치에 대한 이전 위치값을 각각 저장함
        //처음 터치한 위치(touchZero.position)에서 이전 프레임에서의 터치 위치와 이번 프로임에서 터치 위치의 차이를 뺌
        Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition; //deltaPosition는 이동방향 추적할 때 사용
        Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

        // 각 프레임에서 터치 사이의 벡터 거리 구함
        float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude; //magnitude는 두 점간의 거리 비교(벡터)
        float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

        // 거리 차이 구함(거리가 이전보다 크면(마이너스가 나오면)손가락을 벌린 상태_줌인 상태)
        float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

        _camera.orthographicSize += deltaMagnitudeDiff * 0.1f * _zoomSpeed * Time.deltaTime;
        _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize, _minZoomSize, _maxZoomSize);

        _camera.transform.position = LimitPos(transform.position);
    }


    /// <summary>마우스로 카메라를 이동시키는 함수</summary>
    private void MouseMovement()
    {
        if (GameManager.Instance.FriezeCameraMove)
        {
            _touchEnabled = false;
            ResetAcceleration();
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            //좌클릭시 카메라, 클릭 위치 값을 저장해둔다.
            //좌클릭을 꾹 누르고 있을 때 위치를 이동시키기 위함
            _tmpTouchPos = Input.mousePosition;
            _tmpCameraPos = _camera.transform.position;
            _touchEnabled = true;

            ResetAcceleration();
        }

        else if (Input.GetMouseButton(0) && _touchEnabled)
        {
            Vector3 movePos = Camera.main.ScreenToViewportPoint(_tmpTouchPos - Input.mousePosition);
            _camera.transform.position = LimitPos(_tmpCameraPos + movePos * _camera.orthographicSize);

            CheckAcceleration();
        }

        else if (Input.GetMouseButtonUp(0))
        {
            _touchEnabled = false;
            //마우스 좌클릭 버튼을 뗀 경우 가속도를 적용한다.
            SetAcceleration(_distanceMoved);
        }
    }


    private void MouseZoomInOut()
    {
        if (GameManager.Instance.FriezeCameraZoom)
            return;

        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");

        if (scrollWheel != 0)
        {
            _camera.orthographicSize += -scrollWheel * 100 * Time.deltaTime * _zoomSpeed;
            _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize, _minZoomSize, _maxZoomSize);
            _camera.transform.position = LimitPos(transform.position);
        }     
    }


    /// <summary>위치 값을 받아 해당 위치 값이 설정된 범위 밖이면 제한을 걸어 반환하는 함수</summary>
    private Vector3 LimitPos(Vector3 pos)
    {
        _height = _camera.orthographicSize;
        _width = _height * Screen.width / Screen.height;

        float lx = _mapSize.x - _width;
        float clampX = Mathf.Clamp(pos.x, -lx + _mapCenter.x, lx + _mapCenter.x);

        float ly = _mapSize.y - _height;
        float clampY = Mathf.Clamp(pos.y, -ly + _mapCenter.y, ly + _mapCenter.y);

        return new Vector3(clampX, clampY, -10f);
    }


    /// <summary>현재 위치와 이전 위치를 비교하여 가속도 값을 저장하는 함수</summary>
    private void CheckAcceleration()
    {
        _distanceMoved = transform.position - _lastPos; //이전 프레임과 현재 프레임의 위치 차이를 계산해 삽입
        _lastPos = transform.position;
        _velocity = Vector3.zero;

        _touchDownTimer += Time.deltaTime; //체크 시간이 일정 시간 이하면 가속도를 적용하지 않는다.
    }


    /// <summary>가속도 관련 변수를 초기화 하는 함수</summary>
    private void ResetAcceleration()
    {
        _distanceMoved = Vector3.zero;
        _lastPos = Vector3.zero;
        _velocity = Vector3.zero;

        _touchDownTimer = 0;
    }


    /// <summary>가속도를 설정하는 함수</summary>
    private void SetAcceleration(Vector3 acceleration)
    {
        //터치한 시간이 0.05초 미만이면 리턴한다.
        if (_touchDownTimer <= 0.05f)
            return;

        _velocity = (acceleration / Time.deltaTime) * _accelerationRate;
    }


    /// <summary>_velocity값이 0이 아닐 경우 가속도를 주는 함수</summary>
    private void RunningAcceleration()
    {
        //속도가 0일 경우 리턴
        if (_velocity.sqrMagnitude == 0)
            return;

        //감속 속도를 현재 (속도 * Time.deltaTime * 감속)으로 구한다.
        Vector3 _deceleration = _velocity * (Time.deltaTime * _decelerationRate);
        _velocity -= _deceleration;

        //현재 속도가 일정 수치 이하로 내려가면 0으로 지정한다.
        if (_velocity.sqrMagnitude < 0.5f && _velocity.sqrMagnitude > -0.5f)
        {
            _velocity = Vector3.zero;
        }

        _camera.transform.position = LimitPos(transform.position + _deceleration);
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_mapCenter, _mapSize * 2);
    }
}
