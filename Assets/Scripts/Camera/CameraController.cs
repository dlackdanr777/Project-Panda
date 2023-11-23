using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    [SerializeField] private float _dragSpeed;

    [SerializeField] private float _zoomSpeed;

    [SerializeField] private float _maxZoomSize;

    [SerializeField] private float _minZoomSize;

    [SerializeField] private Vector2 _mapSize;

    [SerializeField] private Vector2 _mapCenter;

    private Vector3 _tempTouchPos;

    private float _height;

    private float _width;

    public static bool FriezePos;

    public static bool FriezeZoom;


    private void Update()
    {

#if UNITY_EDITOR

        MouseMovement();
        MouseZoomInOut();

#elif PLATFORM_ANDROID

        TouchMovement();
        TouchZoomInOut();
        
#endif
    }

    private void Start()
    {
        _height = Camera.main.orthographicSize;
        _width = _height * Screen.width / Screen.height;
    }


    private void TouchMovement()
    {
        if (FriezePos)
            return;

        if (Input.touchCount != 1)
            return;

        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
            _tempTouchPos = touch.position;

        if(touch.phase == TouchPhase.Moved)
        {
            Vector2 position = Camera.main.ScreenToViewportPoint((Vector3)touch.position - _tempTouchPos);
            Vector2 move = position * Time.deltaTime * _dragSpeed;
            Camera.main.transform.Translate(-move);
            transform.position = LimitPos(transform.position);
        }
    }

    private void TouchZoomInOut()
    {
        if (FriezeZoom)
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


    bool _isBegan = false;
    private void MouseMovement()
    {
        if (FriezePos)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            _isBegan = true;
            _tempTouchPos = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _isBegan = false;
        }

        if (!_isBegan)
            return;

        Vector2 position = Camera.main.ScreenToViewportPoint(Input.mousePosition - _tempTouchPos);
        Vector2 move = position * Time.deltaTime * _dragSpeed;

        Camera.main.transform.Translate(-move);
        transform.position = LimitPos(transform.position);
    }


    private void MouseZoomInOut()
    {
        if (FriezeZoom)
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
