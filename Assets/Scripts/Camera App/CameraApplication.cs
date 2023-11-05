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
    //스크린샷 찍는 함수를 실행하면 이 이벤트도 실행
    public event Action OnScreenshotHandler;

    [Tooltip("카메라 모드 활성/비활성")]
    [SerializeField] private bool _isActive;

    [Tooltip("메인 캔버스")]
    [SerializeField] private Canvas _canvas;

    [Tooltip("촬영할 카메라")]
    [SerializeField] private Camera _ScreenshotCamera;

    [Tooltip("캡쳐 영역을 표시할 이미지 오브젝트")]
    [SerializeField] private Image _areaImage;

    [Tooltip("사진을 찍은 후 인화되는 애니메이션을 출력하는 클래스")]
    [SerializeField] private PhotoPrinting _photoPrinting;


    private Camera _camera => _ScreenshotCamera;

   

    //====================================================//
    [Space(50)]

    [Tooltip("자석 모드 활성/비활성")]
    [SerializeField] private bool _isMagnetEnable;

    [Tooltip("자석 기능의 감지 범위")]
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
    /// 스크린샷을 찍어주는 함수
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

            //너비, 높이를 계산한다.
            //너비 = 오른쪽 하단 x좌표 - 왼쪽 하단 x좌표
            //높이 = 왼쪽 상단 y좌표 - 왼쪽 하단 y좌표
            int width = (int)corners[3].x - (int)corners[0].x;
            int height = (int)corners[1].y - (int)corners[0].y;

            //시작 위치, 너비, 높이로 Rect생성
            Rect pixelsRect = new Rect(startX, startY, width, height);
            Texture2D ss = new Texture2D(width, height, TextureFormat.RGB24, false);

            //pixelsRect위치에 해당하는 화면의 픽셀을 텍스처 데이터로 읽어와 적용
            ss.ReadPixels(pixelsRect, 0, 0);
            ss.Apply();

            //텍스처를 PNG 형식으로 인코딩하여 byte배열에 저장   
            byte[] byteArray = ss.EncodeToPNG();

            //저장 위치 지정
            string savePath = Application.persistentDataPath;
            string fileName = "/Screenshot" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".png";

            //새 파일을 만들고 지정된 바이트 배열을 savePath위치에 파일로 저장
            //대상 파일이 이미 있으면 덮어씀
            File.WriteAllBytes(savePath + fileName, byteArray);

            PhotoData photoData = new PhotoData(fileName, Application.persistentDataPath);
            AddPhotoData(photoData);
            _photoPrinting.Show(ss);
            OnScreenshotHandler?.Invoke();

            Invoke("SceenshotEnable", 2);

            Debug.LogFormat("캡쳐 완료! 저장위치: {0}", savePath + fileName);


        }
    }

    private void SceenshotEnable() => _screenshotEnable = true;


    private void FixedUpdate()
    {
        MagnetFunction();
    }

    /// <summary>
    /// 특정 오브젝트에 범위가 달라붙게 하는 함수
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

        //월드공간에서 계산된 캔버스의 직사각형의 모서리를 가져와 저장
        //왼쪽하단부터 시계방향으로 배열에 저장
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
            Debug.LogError("photoData가 null입니다.");
        }
        
    }
}
