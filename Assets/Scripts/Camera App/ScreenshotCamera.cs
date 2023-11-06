using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ��ũ������ ������ ����ϴ� ī�޶�
/// </summary>
public class ScreenshotCamera : MonoBehaviour
{

    [Tooltip("ī�޶� ���� ��ũ��Ʈ�� ����")]
    [SerializeField] private CameraApplication _cameraApp;

    [Tooltip("�Կ��� ī�޶�")]
    [SerializeField] private Camera _screenshotCamera;

    [Tooltip("ī�޶� �⺻ ��ġ")]
    [SerializeField] private Vector3 _defaultPos;

    [Tooltip("ĸ�� ������ ǥ���� �̹��� ������Ʈ")]
    [SerializeField] private Image _areaImage;

    [Tooltip("ȭ���� �巡���������� ī�޶� �̵��ӵ�")]
    [SerializeField] private float _dragSpeed = 30f;



    [Space(20)]

    [Tooltip("�ڼ� ��� Ȱ��/��Ȱ��")]
    [SerializeField] private bool _isMagnetEnable;

    [Tooltip("�ڼ� ����� ���� ����")]
    [SerializeField] private Vector3 _boxSize;

    private bool _isMagnetMode;

    private Vector2 _clickPoint;

    private void OnEnable()
    {
        _screenshotCamera.transform.position = _defaultPos;
    }

    private void Update()
    {
        MoveCamera();
        MagnetFunction();
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
        }
    }


    /// <summary>
    /// Ư�� ������Ʈ�� ������ �޶�ٰ� �ϴ� �Լ�
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

}
