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
