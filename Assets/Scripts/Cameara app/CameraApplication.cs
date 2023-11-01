using System.Collections;
using System.IO;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class CameraApplication : MonoBehaviour
{
    [Tooltip("카메라 모드 활성/비활성")]
    [SerializeField] private bool _isActive;

    [Tooltip("메인 캔버스")]
    [SerializeField] private Canvas _canvas;

    [Tooltip("캡쳐 영역을 표시할 이미지 오브젝트")]
    [SerializeField] private AreaImage _areaImage;

    [Tooltip("사진 저장소 추가하기")]
    [SerializeField] private PhotoDatabase _photoDatabase;

    [SerializeField] private UIPhotoAlbum _photoAlbum;

    private Camera _camera => Camera.main;

   

    //====================================================//
    [Space(50)]

    [Tooltip("자석 모드 활성/비활성")]
    [SerializeField] private bool _isMagnetEnable;

    [Tooltip("자석 기능의 감지 범위")]
    [SerializeField] private Vector3 _boxSize;

    [SerializeField]  private bool _isMagnetMode;
    private void Awake()
    {
        _areaImage.OnPointerDownHandler += () => { if (_isMagnetEnable) _isMagnetMode = false; };
        _areaImage.OnPointerUpHandler += () => { if (_isMagnetEnable) _isMagnetMode = true; };
    }



    private void Update()
    {
        if (!_isActive)
            return;

        if (Input.GetKeyDown(KeyCode.C))
        {
            StartCoroutine(ScreenshotObjectByAreaImage());
        }
    }


    private IEnumerator ScreenshotObjectByAreaImage()
    {

        yield return new WaitForEndOfFrame();
        Image image = _areaImage.GetComponent<Image>();
        Vector3[] corners = GetIamgeCorners(image);

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

        //여기부터 테스트
        PhotoData photoData = new PhotoData("사진", "사진입니다.", "2023.09.23", ss);

        _photoDatabase.AddPhotoData(photoData);

        _photoAlbum.GetImage().sprite = _photoDatabase.SetPhotoDataToSprite(0);
        //여기까지 테스트


        //저장 위치 지정
        string savePath = Application.persistentDataPath + "/ScreenshotSave.png";

        //새 파일을 만들고 지정된 바이트 배열을 savePath위치에 파일로 저장
        //대상 파일이 이미 있으면 덮어씀
        File.WriteAllBytes(savePath, byteArray);

        Debug.LogFormat("캡쳐 완료! 저장위치: {0}", savePath);

        if (Application.isPlaying)
            Destroy(ss);
    }


    private void FixedUpdate()
    {
        MagnetFunction();
    }

    //특정 오브젝트에 범위가 달라붙게 하는 함수
    private void MagnetFunction()
    {
        if (!_isMagnetMode || !_isMagnetEnable)
            return;

        Vector3 imageWorldPos = _camera.ScreenToWorldPoint(_areaImage.transform.position);

        RaycastHit2D hit = Physics2D.BoxCast(imageWorldPos, _boxSize, 0, _camera.transform.forward);
        if (hit.collider != null)
        {
            if ( hit.transform.gameObject.TryGetComponent(out ScreenshotObject screenshotObject))
            {
                Vector3 hitToScreenPoint = _camera.WorldToScreenPoint(screenshotObject.transform.position);
                Vector2 mousePos = new Vector2(hitToScreenPoint.x, hitToScreenPoint.y);

                if(Vector3.Distance(screenshotObject.transform.position, _camera.ScreenToWorldPoint(mousePos)) > 0.5f)
                    StartCoroutine(ImagePosLerp(_areaImage.transform.position, mousePos, 0.3f));
            }   
        }
    }


    private IEnumerator ImagePosLerp(Vector2 startPos, Vector2 endPos, float duration )
    {
        if(_isMagnetEnable)
        {
            float timer = 0;

            while (timer < duration)
            {
                timer += Time.deltaTime;

                float t = timer / duration;
                t = t * t * (3f - 2f * t);

                _areaImage.transform.position = Vector2.Lerp(startPos, endPos, t);

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
}
