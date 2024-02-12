using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    [SerializeField] private Rigidbody _rigidbody;

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
    public Vector2 MapSize => _mapSize;

    [Tooltip("ī�޶� �̵� ���� ����")]
    [SerializeField] private Vector2 _mapCenter;

    [Space]
    [Tooltip("ī�޶� ���ӵ� ����")]
    [SerializeField] private float _accelerationMul;

    public Vector2 MapCenter
    {
        get { return _mapCenter; }
        set { _mapCenter = value; }
    }

    private Vector3 _tempTouchPos;

    private Vector3 _tempCameraPos;

    private Vector3 distancemoved;

    private Vector3 lastdistancemoved;

    private Vector3 last;

    private float _height;

    private float _width;

    private IInteraction _currentInteraction;

    private IInteraction _tempInteaction;


    private void Update()
    {

#if UNITY_EDITOR

        MouseMovement();
        MouseZoomInOut();


#elif PLATFORM_ANDROID

        TouchMovement();
        TouchZoomInOut();
        
#endif


        TouchInteraction();
        _currentInteraction?.UpdateInteraction();
    }


    private void FixedUpdate()
    {
        // velocity���� 0�� �ƴҶ� ������ ī�޶� ��ġ���� ����� �ʰ� �ϵ��� �� 
        if (_rigidbody.velocity.sqrMagnitude != 0)
        {
            transform.position = LimitPos(transform.position);
        }
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
            _tempTouchPos = touch.position;
            _tempCameraPos = _camera.transform.position;

            ResetAcceleration();
        }

        else if(touch.phase == TouchPhase.Moved)
        {
            Vector3 position = Camera.main.ScreenToViewportPoint(_tempTouchPos - (Vector3)touch.position);
            transform.position = LimitPos(_tempCameraPos + position * _camera.orthographicSize);

            CheckAcceleration();
        }

        else if(touch.phase == TouchPhase.Ended)
        {
            //��ġ�� �� ��� ���ӵ��� �����Ѵ�.
            SetAcceleration(distancemoved);
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

        transform.position = LimitPos(transform.position);
    }


    /// <summary>���콺�� ī�޶� �̵���Ű�� �Լ�</summary>
    private void MouseMovement()
    {
        if (GameManager.Instance.FriezeCameraMove)
        {
            ResetAcceleration();
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            //��Ŭ���� ī�޶�, Ŭ�� ��ġ ���� �����صд�.
            //��Ŭ���� �� ������ ���� �� ��ġ�� �̵���Ű�� ����
            _tempTouchPos = Input.mousePosition;
            _tempCameraPos = _camera.transform.position;

            ResetAcceleration();
        }

        else if (Input.GetMouseButton(0))
        {
            Vector3 position = Camera.main.ScreenToViewportPoint(_tempTouchPos - Input.mousePosition);
            transform.position = LimitPos(_tempCameraPos + position * _camera.orthographicSize);

            CheckAcceleration();
        }

        else if (Input.GetMouseButtonUp(0))
        {
            //���콺 ��Ŭ�� ��ư�� �� ��� ���ӵ��� �����Ѵ�.
            SetAcceleration(distancemoved);
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
            transform.position = LimitPos(transform.position);
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
        distancemoved = transform.position - last;
        lastdistancemoved = distancemoved;
        last = transform.position;
    }


    /// <summary>���ӵ� ���� ������ �ʱ�ȭ �ϴ� �Լ�</summary>
    private void ResetAcceleration()
    {
        distancemoved = Vector3.zero;
        lastdistancemoved = Vector3.zero;
        last = Vector3.zero;
        _rigidbody.velocity = Vector3.zero;
    }


    /// <summary>���ӵ��� �޾� RigidBody�� �����ϴ� �Լ�</summary>
    private void SetAcceleration(Vector3 acceleration)
    {
        _rigidbody.velocity = acceleration * _accelerationMul;
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_mapCenter, _mapSize * 2);
    }


}
