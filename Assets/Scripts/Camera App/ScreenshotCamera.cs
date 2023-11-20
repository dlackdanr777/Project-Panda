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

    [Tooltip("촬영할 카메라")]
    [SerializeField] private Camera _screenshotCamera;

    [Tooltip("카메라 기본 위치")]
    [SerializeField] private Vector3 _defaultPos;

    [Tooltip("캡쳐 영역을 표시할 이미지 오브젝트")]
    [SerializeField] private Image _areaImage;

    [Tooltip("화면을 드래그했을때의 카메라 이동속도")]
    [SerializeField] private float _dragSpeed = 30f;

    [Tooltip("카메라 제한 범위")]
    [SerializeField] private Vector2 _clampSize;

    //카메라 제한 범위의 중앙
    [SerializeField] private Vector2 _clampCenter;



    [Space(20)]

    [Tooltip("자석 기능 활성/비활성")]
    [SerializeField] private bool _isMagnetEnable;

    [Tooltip("자석 기능의 감지 범위")]
    [SerializeField] private Vector3 _boxSize;

    private float _height;

    private float _width;

    private bool _isMagnetMode;

    private Vector2 _clickPoint;

    private void Start()
    {
        Init();
    }

    private void OnEnable()
    {
        _screenshotCamera.transform.position = _defaultPos;
    }

    private void Update()
    {
        MoveCamera();
        MagnetFunction();
    }

    private void Init()
    {
        _height = _screenshotCamera.orthographicSize;
        _width = _height * _areaImage.rectTransform.rect.width / _areaImage.rectTransform.rect.height;
    }


    private void MoveCamera()
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
    }


    /// <summary>
    /// 특정 오브젝트에 범위가 달라붙게 하는 함수
    /// </summary> 

    private void MagnetFunction()
    {
        if (!_isMagnetMode || !_isMagnetEnable)
            return;

        RaycastHit2D hit = Physics2D.BoxCast(_screenshotCamera.transform.position, _boxSize, 0, _screenshotCamera.transform.forward);

        if (hit.collider != null)
        {
            if (hit.transform.gameObject.TryGetComponent(out ScreenshotObject screenshotObject))
            {

                if (Vector2.Distance(_screenshotCamera.transform.position, screenshotObject.transform.position) > 0.5f)
                    StartCoroutine(ImagePosLerp(_screenshotCamera.transform.position, screenshotObject.transform.position, 0.3f));
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

                _screenshotCamera.transform.position = Vector3.Lerp(startPos, endPos, t);

                yield return null;
            }
        }
    }

    private Vector3 ClampedPos(Vector3 cameraPos)
    {
        float lx = (_clampSize.x * 0.5f) - _width;
        float clampX = Mathf.Clamp(cameraPos.x, _clampCenter.x - lx, _clampCenter.x + lx ) ;

        float ly = (_clampSize.y * 0.5f) - _height;
        float clampY = Mathf.Clamp(cameraPos.y, _clampCenter.y - ly, _clampCenter.y + ly);

        Debug.Log(new Vector3(clampX, clampY, -10));
        Debug.Log(lx);
        Debug.Log(ly);
        return new Vector3(clampX, clampY, -10);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_clampCenter, _clampSize);
    }

}
