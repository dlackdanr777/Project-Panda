using Muks.DataBind;
using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class CameraApplication : MonoBehaviour
{
    //스크린샷 찍는 함수를 실행하면 이 이벤트도 실행
    public event Action<PhotoData> OnSavePhotoHandler;

    [Tooltip("UI카메라 앱 스크립트를 넣는곳")]
    [SerializeField] private UICameraApp _uiCameraApp;

    //[Tooltip("스크린샷 카메라 스크립트")]
    //[SerializeField] private ScreenshotCamera _screenshotCamera;

    [Tooltip("사진을 찍은 후 인화되는 애니메이션을 출력하는 클래스")]
    [SerializeField] private UIPhoto _photoPrinting;

    [Tooltip("카메라 모드 활성/비활성")]
    [SerializeField] private bool _isActive;

    [Tooltip("메인 캔버스")]
    [SerializeField] private Canvas _canvas;


    [Tooltip("캡쳐 영역을 표시할 이미지 오브젝트")]
    [SerializeField] private ShootingRange _shootingRange;

    /// <summary> 현재 찍은 캡처 이미지를 보관해두는 변수 </summary>
    private Texture2D _currentCaptureTexture;

    private void Awake()
    {
        DataBind.SetUnityActionValue("PhotoSaveButton", SavePhoto);
    }


    public void ShowCameraUI()
    {
        _uiCameraApp.gameObject.SetActive(!_uiCameraApp.gameObject.activeSelf);
    }


    /// <summary>
    /// 스크린샷을 찍어주는 함수
    /// </summary>
    /// 
    public void Screenshot()
    {
        StartCoroutine(ScreenshotByAreaImage());
        DatabaseManager.Instance.Challenges.Photo(true); // 도전과제 달성 체크
    }

    private IEnumerator ScreenshotByAreaImage()
    {

        yield return new WaitForEndOfFrame();

        Image image = _shootingRange.GetComponent<Image>();
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

        _photoPrinting.Show(ss);

        _currentCaptureTexture = ss;
    }


    private Vector3[] GetIamgeCorners(Image image)
    {
        Vector3[] corners = new Vector3[4];

        //월드공간에서 계산된 캔버스의 직사각형의 모서리를 가져와 저장
        //왼쪽하단부터 시계방향으로 배열에 저장
        RectTransform objToScreenshot = image.GetComponent<RectTransform>();
        objToScreenshot.GetWorldCorners(corners);

        return corners;
    }


    private void SavePhoto()
    {
        //텍스처를 PNG 형식으로 인코딩하여 byte배열에 저장   
        byte[] byteArray = _currentCaptureTexture.EncodeToPNG();

        //저장 위치 지정
        string savePath = UserInfo.PhotoPath;
        string fileName = "/Screenshot" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".png";

        if (!Directory.Exists(UserInfo.PhotoPath))
            Directory.CreateDirectory(UserInfo.PhotoPath);

        //새 파일을 만들고 지정된 바이트 배열을 savePath위치에 파일로 저장
        //대상 파일이 이미 있으면 덮어씀
        File.WriteAllBytes(savePath + fileName, byteArray);

        PhotoData photoData = new PhotoData(fileName, savePath);

        Debug.LogFormat("캡쳐 완료! 저장위치: {0}", savePath + fileName);
        OnSavePhotoHandler?.Invoke(photoData);
    }
}
