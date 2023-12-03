using Muks.DataBind;
using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class CameraApplication : MonoBehaviour
{
    //��ũ���� ��� �Լ��� �����ϸ� �� �̺�Ʈ�� ����
    public event Action OnSavePhotoHandler;

    [Tooltip("UIī�޶� �� ��ũ��Ʈ�� �ִ°�")]
    [SerializeField] private UICameraApp _uiCameraApp;

    //[Tooltip("��ũ���� ī�޶� ��ũ��Ʈ")]
    //[SerializeField] private ScreenshotCamera _screenshotCamera;

    [Tooltip("������ ���� �� ��ȭ�Ǵ� �ִϸ��̼��� ����ϴ� Ŭ����")]
    [SerializeField] private UIPhoto _photoPrinting;

    [Tooltip("ī�޶� ��� Ȱ��/��Ȱ��")]
    [SerializeField] private bool _isActive;

    [Tooltip("���� ĵ����")]
    [SerializeField] private Canvas _canvas;


    [Tooltip("ĸ�� ������ ǥ���� �̹��� ������Ʈ")]
    [SerializeField] private ShootingRange _shootingRange;

    private bool _screenshotEnable = true;

    /// <summary> ���� ���� ĸó �̹����� �����صδ� ���� </summary>
    private Texture2D _currentCaptureTexture;

    private void Awake()
    {
        DataBind.SetButtonValue("PhotoSaveButton", SavePhoto);
    }

    private void OnEnable()
    {
        _screenshotEnable = true;
    }

    private void OnDisable()
    {
        _screenshotEnable = false;
    }


    public void ShowCameraUI()
    {
        _uiCameraApp.gameObject.SetActive(!_uiCameraApp.gameObject.activeSelf);
    }


    /// <summary>
    /// ��ũ������ ����ִ� �Լ�
    /// </summary>
    /// 
    public void Screenshot()
    {
        StartCoroutine(ScreenshotByAreaImage());
    }

    private IEnumerator ScreenshotByAreaImage()
    {

        yield return new WaitForEndOfFrame();

        Image image = _shootingRange.GetComponent<Image>();
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

        _photoPrinting.Show(ss);

        _currentCaptureTexture = ss;
    }


    private Vector3[] GetIamgeCorners(Image image)
    {
        Vector3[] corners = new Vector3[4];

        //����������� ���� ĵ������ ���簢���� �𼭸��� ������ ����
        //�����ϴܺ��� �ð�������� �迭�� ����
        RectTransform objToScreenshot = image.GetComponent<RectTransform>();
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


    private void SavePhoto()
    {
        //�ؽ�ó�� PNG �������� ���ڵ��Ͽ� byte�迭�� ����   
        byte[] byteArray = _currentCaptureTexture.EncodeToPNG();

        //���� ��ġ ����
        string savePath = Application.persistentDataPath;
        string fileName = "/Screenshot" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".png";

        //�� ������ ����� ������ ����Ʈ �迭�� savePath��ġ�� ���Ϸ� ����
        //��� ������ �̹� ������ ���
        File.WriteAllBytes(savePath + fileName, byteArray);

        PhotoData photoData = new PhotoData(fileName, Application.persistentDataPath);
        AddPhotoData(photoData);

        Debug.LogFormat("ĸ�� �Ϸ�! ������ġ: {0}", savePath + fileName);
        OnSavePhotoHandler?.Invoke();
    }
}
