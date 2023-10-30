using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class CameraApplication : MonoBehaviour
{
    [Tooltip("ī�޶� ��� Ȱ��/��Ȱ��")]
    [SerializeField] private bool _isActive;

    [Tooltip("���� ĵ����")]
    [SerializeField] private Canvas _canvas;

    [Tooltip("ĸ�� ������ ǥ���� �̹��� ������Ʈ")]
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

        //�̹��� ���� �𼭸� ��ġ���� �����ϴ� ����
        Vector3[] corners = new Vector3[4];

        //�̹��� ������ �𼭸� ��ġ���� �����Ų��.
        RectTransform objToScreenshot = _areaImage.GetComponent<RectTransform>();    
        objToScreenshot.GetWorldCorners(corners);

        //������ġ�� ����, ���̸� ����Ѵ�.
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

        Debug.LogFormat("ĸ�� �Ϸ�! ������ġ: {0}", savePath);

        if (Application.isPlaying)
            Destroy(ss);
    }


    private float GetScale(int width, int height, Vector2 scalerReferenceResolution, float scalerMatchWidthOrHeight)
    {
        return Mathf.Pow(width / scalerReferenceResolution.x, 1f - scalerMatchWidthOrHeight)
            * Mathf.Pow(height / scalerReferenceResolution.y, scalerMatchWidthOrHeight);
    }
}
