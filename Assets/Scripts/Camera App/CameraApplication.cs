using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CameraApplication : MonoBehaviour
{
    //��ũ���� ��� �Լ��� �����ϸ� �� �̺�Ʈ�� ����
    public event Action OnScreenshotHandler;

    [Tooltip("ī�޶� ��� Ȱ��/��Ȱ��")]
    [SerializeField] private bool _isActive;

    [Tooltip("���� ĵ����")]
    [SerializeField] private Canvas _canvas;

    [Tooltip("�Կ��� ī�޶�")]
    [SerializeField] private Camera _ScreenshotCamera;

    [Tooltip("ĸ�� ������ ǥ���� �̹��� ������Ʈ")]
    [SerializeField] private Image _areaImage;

    [Tooltip("������ ���� �� ��ȭ�Ǵ� �ִϸ��̼��� ����ϴ� Ŭ����")]
    [SerializeField] private PhotoPrinting _photoPrinting;


    private Camera _camera => _ScreenshotCamera;

   

    //====================================================//
    [Space(50)]

    [Tooltip("�ڼ� ��� Ȱ��/��Ȱ��")]
    [SerializeField] private bool _isMagnetEnable;

    [Tooltip("�ڼ� ����� ���� ����")]
    [SerializeField] private Vector3 _boxSize;

    [SerializeField]  private bool _isMagnetMode;


    private void OnEnable()
    {
        _screenshotEnable = true;
    }


    private void Update()
    {
        if (!_isActive)
            return;

        MoveCamera();
    }


    Vector2 _clickPoint;
    float dragSpeed = 30f;
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
            Vector3 pos = _ScreenshotCamera.ScreenToViewportPoint((Vector2)Input.mousePosition - _clickPoint);

            Vector3 move = pos * (Time.deltaTime * dragSpeed);
            _ScreenshotCamera.transform.Translate(move);
        }
    }

    private bool _screenshotEnable = true;
    /// <summary>
    /// ��ũ������ ����ִ� �Լ�
    /// </summary>
    public IEnumerator ScreenshotByAreaImage(float waitTime)
    {
        if (_screenshotEnable)
        {
            _screenshotEnable = false;

            yield return new WaitForEndOfFrame();
            Image image = _areaImage;
            Vector3[] corners = GetIamgeCorners(_areaImage);

            float startX = corners[0].x;
            float startY = corners[0].y;

            //�ʺ�, ���̸� ����Ѵ�.
            //�ʺ� = ������ �ϴ� x��ǥ - ���� �ϴ� x��ǥ
            //���� = ���� ��� y��ǥ - ���� �ϴ� y��ǥ
            int width = (int)corners[3].x - (int)corners[0].x;
            int height = (int)corners[1].y - (int)corners[0].y;

            //���� ��ġ, �ʺ�, ���̷� Rect����
            Rect pixelsRect = new Rect(startX, startY, width, height);
            Texture2D ss = new Texture2D(width, height, TextureFormat.RGB24, false);

            //pixelsRect��ġ�� �ش��ϴ� ȭ���� �ȼ��� �ؽ�ó �����ͷ� �о�� ����
            ss.ReadPixels(pixelsRect, 0, 0);
            ss.Apply();

            //�ؽ�ó�� PNG �������� ���ڵ��Ͽ� byte�迭�� ����   
            byte[] byteArray = ss.EncodeToPNG();

            //���� ��ġ ����
            string savePath = Application.persistentDataPath;
            string fileName = "/Screenshot" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".png";

            //�� ������ ����� ������ ����Ʈ �迭�� savePath��ġ�� ���Ϸ� ����
            //��� ������ �̹� ������ ���
            File.WriteAllBytes(savePath + fileName, byteArray);

            PhotoData photoData = new PhotoData(fileName, Application.persistentDataPath);
            AddPhotoData(photoData);
            _photoPrinting.Show(ss);
            OnScreenshotHandler?.Invoke();

            Invoke("SceenshotEnable", 2);

            Debug.LogFormat("ĸ�� �Ϸ�! ������ġ: {0}", savePath + fileName);


        }
    }

    private void SceenshotEnable() => _screenshotEnable = true;


    private void FixedUpdate()
    {
        MagnetFunction();
    }

    /// <summary>
    /// Ư�� ������Ʈ�� ������ �޶�ٰ� �ϴ� �Լ�
    /// </summary>
    private void MagnetFunction()
    {
        if (!_isMagnetMode || !_isMagnetEnable)
            return;

        Vector3 imageWorldPos = _camera.ScreenToWorldPoint(_areaImage.transform.position);

        RaycastHit2D hit = Physics2D.BoxCast(_camera.transform.position, _boxSize, 0, _camera.transform.forward);
        
        if (hit.collider != null)
        {
            if ( hit.transform.gameObject.TryGetComponent(out ScreenshotObject screenshotObject))
            {

                if(Vector2.Distance(_ScreenshotCamera.transform.position, screenshotObject.transform.position) > 0.5f)
                    StartCoroutine(ImagePosLerp(_ScreenshotCamera.transform.position, screenshotObject.transform.position, 0.3f));
            }   
        }
    }


    private IEnumerator ImagePosLerp(Vector3 startPos, Vector3 endPos, float duration )
    {
        if(_isMagnetEnable)
        {
            float timer = 0;
            endPos = new Vector3(endPos.x, endPos.y, startPos.z);

            while (timer < duration)
            {
                timer += Time.deltaTime;

                float t = timer / duration;
                t = t * t * (3f - 2f * t);

                _ScreenshotCamera.transform.position = Vector3.Lerp(startPos, endPos, t);

                yield return null;
            }
        }
    }

    private Vector3[] GetIamgeCorners(Image image)
    {
        Vector3[] corners = new Vector3[4];

        //����������� ���� ĵ������ ���簢���� �𼭸��� ������ ����
        //�����ϴܺ��� �ð�������� �迭�� ����
        RectTransform objToScreenshot = _areaImage.GetComponent<RectTransform>();
        objToScreenshot.GetWorldCorners(corners);

        return corners;
    }


    private void AddPhotoData(PhotoData photoData)
    {
        if(photoData != null)
        {
            Database.Instance.Photos.Add(photoData);
        }
        else
        {
            Debug.LogError("photoData�� null�Դϴ�.");
        }
        
    }
}
