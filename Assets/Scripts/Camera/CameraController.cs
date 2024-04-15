using UnityEngine;


public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    [Space]
    [Tooltip("ī�޶� Ȯ��/��� �ӵ�")]
    [SerializeField] private float _zoomSpeed;
    public float ZoomSpeed => _zoomSpeed;

    [Tooltip("ī�޶� Ȯ�� �ִ� ũ��")]
    [SerializeField] private float _maxZoomSize;
    public float MaxZoomSize => _maxZoomSize;

    [Tooltip("ī�޶� ��� �ּ� ũ��")]
    [SerializeField] private float _minZoomSize;
    public float MinZoomSize => _minZoomSize;

    [Tooltip("ī�޶� �̵� ����")]
    [SerializeField] private Vector2 _mapSize;
    public Vector2 MapSize
    {
        get { return _mapSize; }
        set { _mapSize = value; }
    }

    [Tooltip("ī�޶� �̵� ���� ����")]
    [SerializeField] private Vector2 _mapCenter;

    [Space]
    [Tooltip("ī�޶� ���� ����")]
    [SerializeField] private float _accelerationRate;

    [Tooltip("ī�޶� ���� ����")]
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

        //���콺�� �������� ���̸� ��� ���̸� ����� IInteraction�� ���� ������Ʈ�� ������ 
        //IInteraction�� �ӽ� ������ ��Ƴ��� ���콺 ��ư�� ������ �ӽ� ������ ���� ������Ʈ �� ��� �����ϰ� �߽��ϴ�.
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
            //ȭ�� ��ġ�� ī�޶�, Ŭ�� ��ġ ���� �����صд�.
            //ȭ���� �� ������ ���� �� ��ġ�� �̵���Ű�� ����
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
            //��ġ�� �� ��� ���ӵ��� �����Ѵ�.
            SetAcceleration(_distanceMoved);
        }

    }


    private void TouchZoomInOut()
    {
        if (GameManager.Instance.FriezeCameraZoom)
            return;

        if (Input.touchCount != 2)
            return;

        Touch touchZero = Input.GetTouch(0); //ù��° �հ��� ��ġ�� ����
        Touch touchOne = Input.GetTouch(1); //�ι�° �հ��� ��ġ�� ����

        //��ġ�� ���� ���� ��ġ���� ���� ������
        //ó�� ��ġ�� ��ġ(touchZero.position)���� ���� �����ӿ����� ��ġ ��ġ�� �̹� �����ӿ��� ��ġ ��ġ�� ���̸� ��
        Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition; //deltaPosition�� �̵����� ������ �� ���
        Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

        // �� �����ӿ��� ��ġ ������ ���� �Ÿ� ����
        float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude; //magnitude�� �� ������ �Ÿ� ��(����)
        float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

        // �Ÿ� ���� ����(�Ÿ��� �������� ũ��(���̳ʽ��� ������)�հ����� ���� ����_���� ����)
        float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

        _camera.orthographicSize += deltaMagnitudeDiff * 0.1f * _zoomSpeed * Time.deltaTime;
        _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize, _minZoomSize, _maxZoomSize);

        _camera.transform.position = LimitPos(transform.position);
    }


    /// <summary>���콺�� ī�޶� �̵���Ű�� �Լ�</summary>
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
            //��Ŭ���� ī�޶�, Ŭ�� ��ġ ���� �����صд�.
            //��Ŭ���� �� ������ ���� �� ��ġ�� �̵���Ű�� ����
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
            //���콺 ��Ŭ�� ��ư�� �� ��� ���ӵ��� �����Ѵ�.
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


    /// <summary>��ġ ���� �޾� �ش� ��ġ ���� ������ ���� ���̸� ������ �ɾ� ��ȯ�ϴ� �Լ�</summary>
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


    /// <summary>���� ��ġ�� ���� ��ġ�� ���Ͽ� ���ӵ� ���� �����ϴ� �Լ�</summary>
    private void CheckAcceleration()
    {
        _distanceMoved = transform.position - _lastPos; //���� �����Ӱ� ���� �������� ��ġ ���̸� ����� ����
        _lastPos = transform.position;
        _velocity = Vector3.zero;

        _touchDownTimer += Time.deltaTime; //üũ �ð��� ���� �ð� ���ϸ� ���ӵ��� �������� �ʴ´�.
    }


    /// <summary>���ӵ� ���� ������ �ʱ�ȭ �ϴ� �Լ�</summary>
    private void ResetAcceleration()
    {
        _distanceMoved = Vector3.zero;
        _lastPos = Vector3.zero;
        _velocity = Vector3.zero;

        _touchDownTimer = 0;
    }


    /// <summary>���ӵ��� �����ϴ� �Լ�</summary>
    private void SetAcceleration(Vector3 acceleration)
    {
        //��ġ�� �ð��� 0.05�� �̸��̸� �����Ѵ�.
        if (_touchDownTimer <= 0.05f)
            return;

        _velocity = (acceleration / Time.deltaTime) * _accelerationRate;
    }


    /// <summary>_velocity���� 0�� �ƴ� ��� ���ӵ��� �ִ� �Լ�</summary>
    private void RunningAcceleration()
    {
        //�ӵ��� 0�� ��� ����
        if (_velocity.sqrMagnitude == 0)
            return;

        //���� �ӵ��� ���� (�ӵ� * Time.deltaTime * ����)���� ���Ѵ�.
        Vector3 _deceleration = _velocity * (Time.deltaTime * _decelerationRate);
        _velocity -= _deceleration;

        //���� �ӵ��� ���� ��ġ ���Ϸ� �������� 0���� �����Ѵ�.
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
