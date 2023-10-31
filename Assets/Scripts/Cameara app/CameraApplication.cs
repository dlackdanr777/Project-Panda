using System.Collections;
using System.IO;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class CameraApplication : MonoBehaviour
{
    [Tooltip("ī�޶� ��� Ȱ��/��Ȱ��")]
    [SerializeField] private bool _isActive;

    [Tooltip("���� ĵ����")]
    [SerializeField] private Canvas _canvas;

    [Tooltip("ĸ�� ������ ǥ���� �̹��� ������Ʈ")]
    [SerializeField] private AreaImage _areaImage;

    private Camera _camera => Camera.main;

    //====================================================//
    [Space(50)]

    [Tooltip("�ڼ� ��� Ȱ��/��Ȱ��")]
    [SerializeField] private bool _isMagnetMode;

    [Tooltip("�ڼ� ����� ���� ����")]
    [SerializeField] private Vector3 _boxSize;

    private bool _isLerpStart;

    private void Awake()
    {
        _areaImage.OnPointerDownHandler += () => _isMagnetMode = false;
        _areaImage.OnPointerUpHandler += () => _isMagnetMode = true;
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
        string savePath = Application.persistentDataPath + "/ScreenshotSave.png";

        //�� ������ ����� ������ ����Ʈ �迭�� savePath��ġ�� ���Ϸ� ����
        //��� ������ �̹� ������ ���
        File.WriteAllBytes(savePath, byteArray);

        Debug.LogFormat("ĸ�� �Ϸ�! ������ġ: {0}", savePath);

        if (Application.isPlaying)
            Destroy(ss);
    }


    private void FixedUpdate()
    {
        MagnetFunction();
    }


    private void MagnetFunction()
    {
        if (!_isMagnetMode || _isLerpStart)
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
        _isLerpStart = true;
        float timer = 0;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            float t = timer / duration;
            t = t * t * (3f - 2f * t);

            _areaImage.transform.position = Vector2.Lerp(startPos, endPos, t);

            yield return null;
        }
        _isLerpStart = false;
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
}
