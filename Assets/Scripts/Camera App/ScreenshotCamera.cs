using Muks.DataBind;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 스크린샷을 찍을때 사용하는 카메라
/// </summary>
public class ScreenshotCamera : MonoBehaviour
{

    [Tooltip("카메라 어플 스크립트를 연결")]
    [SerializeField] private CameraApplication _cameraApp;

    [Tooltip("메인 카메라 연결")]
    [SerializeField] private CameraController _cameraController;

    [Tooltip("캡쳐 영역을 표시할 이미지 오브젝트")]
    [SerializeField] private Image _areaImage;

    [SerializeField] private ShootingRange _shootingImage;

    [Tooltip("랜더 텍스쳐")]
    [SerializeField] private RenderTexture _renderTexture;



    [Space(20)]

    [Tooltip("자석 기능 활성/비활성")]
    [SerializeField] private bool _isMagnetEnable;

    [Tooltip("자석 기능의 감지 범위")]
    [SerializeField] private Vector3 _boxSize;



    private float _zoomSpeed => _cameraController.ZoomSpeed;

    private float _maxZoomSize => _cameraController.MaxZoomSize - 4;

    private float _minZoomSize => _cameraController.MinZoomSize - 4;

    private Vector2 _mapSize => _cameraController.MapSize;

    private Vector2 _mapCenter => _cameraController.MapCenter;

    private Camera _camera;

    private Vector3 _tempTouchPos;

    private Vector3 _tempCameraPos;

    private float _height;

    private float _width;

    private bool _isMagnetMode;

    private bool _isBegan = false;

    private void Start()
    {
        Init();

        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _camera.orthographicSize = _cameraController.GetComponent<Camera>().orthographicSize - 4;
        _camera.transform.position = _cameraController.transform.position;

        _renderTexture.Release();
        _renderTexture.width = (int)_areaImage.rectTransform.rect.width;
        _renderTexture.height = (int)_areaImage.rectTransform.rect.height;
    }

    private void Update()
    {
#if UNITY_EDITOR

        MouseMovement();
        MouseZoomInOut();


#elif PLATFORM_ANDROID

        TouchMovement();
        TouchZoomInOut();
        
#endif

        MagnetFunction();
    }

    private void Init()
    {
        _camera = GetComponent<Camera>();
        DataBind.SetButtonValue("ShowScreenshotCameraButton", () => gameObject.SetActive(true));
        DataBind.SetButtonValue("HideScreenshotCameraButton", () => gameObject.SetActive(false));
    }


   /* private void MoveCamera()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _clickPoint = Input.mousePosition;
            if (_isMagnetEnable) _isMagnetMode = false;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (_isMagnetEnable) _isMagnetMode = true;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 pos = _screenshotCamera.ScreenToViewportPoint((Vector2)Input.mousePosition - _clickPoint);

            Vector3 move = -pos * (Time.deltaTime * _dragSpeed);
            _screenshotCamera.transform.Translate(move);

            _screenshotCamera.transform.position = ClampedPos(_screenshotCamera.transform.position);
        }
    }*/


    /// <summary>
    /// 특정 오브젝트에 범위가 달라붙게 하는 함수
    /// </summary> 

    private void MagnetFunction()
    {
        if (!_isMagnetMode || !_isMagnetEnable)
            return;

        RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, _boxSize, 0, transform.forward);
        Debug.Log(hits.Length);
        foreach(RaycastHit2D hit in hits)
        {
            Debug.Log(hit.transform.name);
            if (hit.transform.TryGetComponent(out Panda panda))
            {

                if (Vector2.Distance(transform.position, panda.transform.position) > 0.5f)
                    StartCoroutine(ImagePosLerp(transform.position, panda.transform.position, 0.3f));

                return;
            }
        }

    }


    private IEnumerator ImagePosLerp(Vector3 startPos, Vector3 endPos, float duration)
    {
        if (_isMagnetEnable)
        {
            float timer = 0;
            endPos = new Vector3(endPos.x, endPos.y, startPos.z);

            while (timer < duration)
            {
                timer += Time.deltaTime;

                float t = timer / duration;
                t = t * t * (3f - 2f * t);

                transform.position = Vector3.Lerp(startPos, endPos, t);

                yield return null;
            }
        }
    }


/*    private Vector3 ClampedPos(Vector3 cameraPos)
    {
        float lx = (_clampSize.x * 0.5f) - _width;
        float clampX = Mathf.Clamp(cameraPos.x, _clampCenter.x - lx, _clampCenter.x + lx ) ;

        float ly = (_clampSize.y * 0.5f) - _height;
        float clampY = Mathf.Clamp(cameraPos.y, _clampCenter.y - ly, _clampCenter.y + ly);

        return new Vector3(clampX, clampY, -10);
    }
*/

    private void TouchMovement()
    {
        if (Input.touchCount != 1)
            return;

        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            if (_isMagnetEnable) _isMagnetMode = false;
            _tempTouchPos = touch.position;
            _tempCameraPos = _camera.transform.position;
        }

        if (touch.phase == TouchPhase.Moved)
        {
            Vector3 position = Camera.main.ScreenToViewportPoint(_tempTouchPos - (Vector3)touch.position);
            transform.position = LimitPos(_tempCameraPos + position * _camera.orthographicSize);
        }

        if(touch.phase == TouchPhase.Ended)
        {
            if (_isMagnetEnable) _isMagnetMode = true;
        }
    }


    private void TouchZoomInOut()
    {
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


    private void MouseMovement()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (_isMagnetEnable) _isMagnetMode = false;

            _isBegan = true;
            _tempTouchPos = Input.mousePosition;
            _tempCameraPos = _camera.transform.position;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (_isMagnetEnable) _isMagnetMode = true;

            _isBegan = false;
        }

        if (!_isBegan)
            return;

        Vector3 position = Camera.main.ScreenToViewportPoint(_tempTouchPos - (Vector3)Input.mousePosition);
        transform.position = LimitPos(_tempCameraPos + position * _camera.orthographicSize);
    }


    private void MouseZoomInOut()
    {
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
        Debug.Log(Screen.width / Screen.height);
        _width = _height * 0.6848f;

        float lx = _mapSize.x - _width;
        float clampX = Mathf.Clamp(pos.x, -lx + _mapCenter.x, lx + _mapCenter.x);

        float ly = _mapSize.y - _height;
        float clampY = Mathf.Clamp(pos.y, -ly + _mapCenter.y, ly + _mapCenter.y);

        return new Vector3(clampX, clampY, -10f);
    }
}
