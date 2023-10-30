using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class CameraApplication : MonoBehaviour
{
    [Tooltip("카메라 모드 활성/비활성")]
    [SerializeField] private bool _isActive;

    [Tooltip("메인 캔버스")]
    [SerializeField] private Canvas _canvas;

    [Tooltip("캡쳐 영역을 표시할 이미지 오브젝트")]
    [SerializeField] private Image _areaImage;

    private Camera _camera => Camera.main;

    private void Update()
    {
        if (!_isActive)
            return;

        if (Input.GetKeyDown(KeyCode.C))
        {
            StartCoroutine(ScreenCaptureByAreaImage());
        }
    }


    private IEnumerator ScreenCaptureByAreaImage()
    {
        yield return new WaitForEndOfFrame();

        //이미지 영역 모서리 위치값을 저장하는 변수
        Vector3[] corners = new Vector3[4];

        //이미지 영역의 모서리 위치값을 저장시킨다.
        RectTransform objToScreenshot = _areaImage.GetComponent<RectTransform>();    
        objToScreenshot.GetWorldCorners(corners);

        //시작위치와 넓이, 높이를 계산한다.
        float startX = corners[0].x;
        float startY = corners[0].y;
        int width = (int)corners[3].x - (int)corners[0].x;
        int height = (int)corners[1].y - (int)corners[0].y;

        Rect pixelsRect = new Rect(startX, startY, width, height);
        Texture2D ss = new Texture2D(width, height, TextureFormat.RGB24, false);

        ss.ReadPixels(pixelsRect, 0, 0);
        ss.Apply();

        byte[] byteArray = ss.EncodeToPNG();
        string savePath = Application.persistentDataPath + "/ScreenshotSave.png";
        File.WriteAllBytes(savePath, byteArray);

        Debug.LogFormat("캡쳐 완료! 저장위치: {0}", savePath);

        if (Application.isPlaying)
            Destroy(ss);
    }


    private float GetScale(int width, int height, Vector2 scalerReferenceResolution, float scalerMatchWidthOrHeight)
    {
        return Mathf.Pow(width / scalerReferenceResolution.x, 1f - scalerMatchWidthOrHeight)
            * Mathf.Pow(height / scalerReferenceResolution.y, scalerMatchWidthOrHeight);
    }
}
