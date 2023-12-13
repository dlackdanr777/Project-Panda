using Muks.DataBind;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// ��ũ������ ������ ����ϴ� ī�޶�
/// </summary>
public class ScreenshotCamera : MonoBehaviour
{
    public static event Action<int, int> OnStartHandler;


    [Tooltip("ī�޶� ���� ��ũ��Ʈ�� ����")]
    [SerializeField] private CameraApplication _cameraApp;

    [Tooltip("���� ī�޶� ����")]
    [SerializeField] private CameraController _cameraController;

    [SerializeField] private ShootingRange _shootingImage;


    [Tooltip("���� �ؽ���")]
    [SerializeField] private RenderTexture _renderTexture;



    [Space(20)]

    [Tooltip("�ڼ� ��� Ȱ��/��Ȱ��")]
    [SerializeField] private bool _isMagnetEnable;

    [Tooltip("�ڼ� ����� ���� ����")]
    [SerializeField] private Vector3 _boxSize;


    private Image _areaImage;

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

    private void Awake()
    {
        Init();
        RenderTextuereResize();
    }


    private void Start()
    {
        
#if UNITY_EDITOR

        _shootingImage.OnDragehandler += MouseMovement;
        _shootingImage.OnPointerDownHandler += MouseDown;
        _shootingImage.OnPointerUpHandler += MouseUp;

#elif PLATFORM_ANDROID

        _shootingImage.OnDragehandler += TouchMovement;
        _shootingImage.OnPointerDownHandler += TouchDown;
        _shootingImage.OnPointerUpHandler += TouchUp;
        
#endif

        int width = (int)_shootingImage.GetComponent<Image>().rectTransform.rect.width;
        int height = (int)_shootingImage.GetComponent<Image>().rectTransform.rect.height;

        OnStartHandler?.Invoke(width, height);
        gameObject.SetActive(false);

    }

    private void OnEnable()
    {
        _camera.transform.position = _cameraController.transform.position;

        Graphics.Blit(_areaImage.mainTexture, _renderTexture);

        _camera.orthographicSize = Camera.main.orthographicSize - 4;
    }

    private void RenderTextuereResize()
    {
        _renderTexture.Release();
        
        int width = (int)_shootingImage.GetComponent<Image>().rectTransform.rect.width;
        int height = (int)_shootingImage.GetComponent<Image>().rectTransform.rect.height;

        _renderTexture.width = width;
        _renderTexture.height = height;
    }



    private void OnDisable()
    {
        RenderTextuereResize();
    }

    private void Update()
    {
#if UNITY_EDITOR

        MouseZoomInOut();

#elif PLATFORM_ANDROID

        TouchZoomInOut();
        
#endif

        MagnetFunction();
    }

    private void Init()
    {
        _camera = GetComponent<Camera>();
        _areaImage = _shootingImage.GetComponent<Image>();
        DataBind.SetButtonValue("ShowScreenshotCameraButton", () => gameObject.SetActive(true));
        DataBind.SetButtonValue("HideScreenshotCameraButton", () => gameObject.SetActive(false));
    }


    /// <summary>
    /// Ư�� ������Ʈ�� ������ �޶�ٰ� �ϴ� �Լ�
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


    Touch _touch;
    private void TouchDown(PointerEventData data)
    {
        if (_isMagnetEnable) _isMagnetMode = false;

        _touch = Input.GetTouch(0);
        _tempTouchPos = _touch.position;
        _tempCameraPos = _camera.transform.position;
    }


    private void TouchUp(PointerEventData data)
    {
        if (_isMagnetEnable) _isMagnetMode = true;
    }


    private void TouchMovement(PointerEventData data)
    {
        if (Input.touchCount != 1)
            return;

        _touch = Input.GetTouch(0);

        Vector3 position = Camera.main.ScreenToViewportPoint(_tempTouchPos - (Vector3)_touch.position);
        transform.position = LimitPos(_tempCameraPos + position * _camera.orthographicSize);

    }


    private void TouchZoomInOut()
    {
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

    private void MouseDown(PointerEventData data)
    {
        if (!Input.GetMouseButtonDown(0))
            return;

        if (_isMagnetEnable) _isMagnetMode = false;
        _isBegan = true;
        _tempTouchPos = Input.mousePosition;
        _tempCameraPos = _camera.transform.position;
    }

    private void MouseUp(PointerEventData data)
    {
        if (!Input.GetMouseButtonUp(0))
            return;

        if (_isMagnetEnable) _isMagnetMode = true;
            _isBegan = false;
    }

    private void MouseMovement(PointerEventData data)
    {

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
        _width = _height * 0.6848f;

        float lx = _mapSize.x - _width;
        float clampX = Mathf.Clamp(pos.x, -lx + _mapCenter.x, lx + _mapCenter.x);

        float ly = _mapSize.y - _height;
        float clampY = Mathf.Clamp(pos.y, -ly + _mapCenter.y, ly + _mapCenter.y);

        return new Vector3(clampX, clampY, -10f);
    }
}
