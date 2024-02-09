using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    [SerializeField] private float _zoomSpeed;
    public float ZoomSpeed => _zoomSpeed;

    [SerializeField] private float _maxZoomSize;
    public float MaxZoomSize => _maxZoomSize;

    [SerializeField] private float _minZoomSize;
    public float MinZoomSize => _minZoomSize;

    [SerializeField] private Vector2 _mapSize;
    public Vector2 MapSize => _mapSize;

    [SerializeField] private Vector2 _mapCenter;
    public Vector2 MapCenter
    {
        get { return _mapCenter; }
        set { _mapCenter = value; }
    }

    private Vector3 _tempTouchPos;

    private Vector3 _tempCameraPos;

    private Vector3 _tmpMovePos;

    private Vector3 _updateIntervalPos;

    private float _touchTime;

    private float _height;

    private float _width;

    private IInteraction _currentInteraction;

    private IInteraction _tempInteaction;

    private bool _isBegan = false;

    private Coroutine SmoothMoveToPositionRoutine;


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

            if (SmoothMoveToPositionRoutine != null)
            {
                StopCoroutine(SmoothMoveToPositionRoutine);
                SmoothMoveToPositionRoutine = null;
            }

            return;
        }


        if (Input.touchCount != 1)
            return;

        Touch touch = Input.GetTouch(0);


        if (touch.phase == TouchPhase.Began)
        {
            _tempTouchPos = touch.position;
            _tempCameraPos = _camera.transform.position;
            _updateIntervalPos = touch.position;
            _tmpMovePos = Vector3.zero;
            _touchTime = 0;
        }

        else if(touch.phase == TouchPhase.Moved)
        {
            Vector3 position = Camera.main.ScreenToViewportPoint(_tempTouchPos - (Vector3)touch.position);
            transform.position = LimitPos(_tempCameraPos + position * _camera.orthographicSize);

            _touchTime += Time.deltaTime;

            if (0.2f < _touchTime)
            {
                _updateIntervalPos = (Vector3)touch.position;
                _touchTime = 0;
            }

            _tmpMovePos = _updateIntervalPos - (Vector3)touch.position;
        }

        else if(touch.phase == TouchPhase.Ended)
        {
            Vector3 targetPos = transform.position + (Camera.main.ScreenToViewportPoint(_tmpMovePos) * _camera.orthographicSize);
            SmoothMoveToPositionRoutine = StartCoroutine(SmoothMoveToPosition(transform.position, targetPos));
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


    private void MouseMovement()
    {
        if (GameManager.Instance.FriezeCameraMove)
        {
            _isBegan = false;

            if (SmoothMoveToPositionRoutine != null)
            {
                StopCoroutine(SmoothMoveToPositionRoutine);
                SmoothMoveToPositionRoutine = null;
            }
                
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            _isBegan = true;
            _tempTouchPos = Input.mousePosition;
            _tempCameraPos = _camera.transform.position;
            _touchTime = 0;
            _updateIntervalPos = Input.mousePosition;
            _tmpMovePos = Vector3.zero;

            if (SmoothMoveToPositionRoutine != null)
            {
                StopCoroutine(SmoothMoveToPositionRoutine);
                SmoothMoveToPositionRoutine = null;
            }            
        }

        else if (Input.GetMouseButtonUp(0))
        {
            _isBegan = false;
            Vector3 targetPos = transform.position + (Camera.main.ScreenToViewportPoint(_tmpMovePos) * _camera.orthographicSize);
            SmoothMoveToPositionRoutine = StartCoroutine(SmoothMoveToPosition(transform.position, targetPos));

        }

        if (!_isBegan)
            return;

        else
        {
            _touchTime += Time.deltaTime;
        }


        Vector3 position = Camera.main.ScreenToViewportPoint(_tempTouchPos - Input.mousePosition);

        if(0.2f < _touchTime)
        {
            _updateIntervalPos = Input.mousePosition;
            _touchTime = 0;
        }

        _tmpMovePos = _updateIntervalPos - Input.mousePosition;
        transform.position = LimitPos(_tempCameraPos + position * _camera.orthographicSize);
    }


    private IEnumerator SmoothMoveToPosition(Vector3 pos, Vector3 targetPos)
    {
        float duration = 0;
        float totalDuration = 1;

        while (duration < totalDuration)
        {
            duration += 0.01f * totalDuration;

            float percent = duration / totalDuration;
            percent = 1 - Mathf.Pow((1 - percent), 5);

            Vector3 lerpPos = Vector3.Lerp(pos, targetPos, percent);
            transform.position = LimitPos(lerpPos);

            yield return YieldCache.WaitForSeconds(0.01f);
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


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_mapCenter, _mapSize * 2);
    }


}
