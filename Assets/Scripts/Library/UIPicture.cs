using Muks.Tween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class UIPicture : UIView
{
    [SerializeField] private Image _showImage;

    [SerializeField] private Button _backgroundButton;

    private Material _tempMat;


    public override void Init(UINavigation uiNav)
    {
        base.Init(uiNav);
        gameObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        _tempMat = new Material(_showImage.material);

        UIAlbumSlot.OnButtonClickHandler += OnAlbumButtonClicked;
    }


    public override void Show()
    {
        Tween.Stop(gameObject);
        _backgroundButton.gameObject.SetActive(true);
        gameObject.SetActive(true);
        VisibleState = VisibleState.Appearing;
        Tween.TransformScale(gameObject, new Vector3(1, 1, 1), 0.3f, TweenMode.Smootherstep, () =>
        {
            VisibleState = VisibleState.Appeared;
        });
    }


    public override void Hide()
    {
        Tween.Stop(gameObject);

        VisibleState = VisibleState.Disappearing;
        Tween.TransformScale(gameObject, new Vector3(0.1f, 0.1f, 0.1f), 0.3f, TweenMode.Smootherstep, () =>
        {
            VisibleState = VisibleState.Disappeared;

            gameObject.SetActive(false);
            _backgroundButton.gameObject.SetActive(false);
        });
    }


    private void OnAlbumButtonClicked(PhotoData photoData)
    {
        SetImageByPhotoData(photoData);
        _uiNav.Push("Picture");
    }


    /// <summary>
    /// PhotoData Ŭ������ ������ PC�� ����� PNG������ Image������Ʈ�� �̹����� ��ȯ�ϴ� �Լ�  
    /// </summary>
    private void SetImageByPhotoData(PhotoData photoData)
    {
        if (photoData != null)
        {
            //������ ����� ��ġ�� �����̸��� �ٿ� ����
            string path = photoData.PathFloder + photoData.FileName;

            //���� ��ġ�� �ִ� PNG������ �о� Byte�迭�� ��ȯ�� ����
            byte[] bytes = File.ReadAllBytes(path);

            Texture2D tex = new Texture2D(2, 2);

            //byte[]�� ��ȯ�� PNG������ �о� �̹����� ��ȯ
            tex.LoadImage(bytes);

            float heightMul = (float)tex.height / (float)tex.width;
            if (_showImage != null)
            {
                //������ ���� �ؽ�ó�� ������ �о���� �̹����� ����
                _tempMat.mainTexture = tex;

                //_showImage.rectTransform.sizeDelta = new Vector2(0, (RectTransform.rect.height * heightMul) - RectTransform.rect.height);
                //������ null�� �����ߴ� �ٽ� ������� �����Ѵ�. �̷��� ���ΰ�ħ�� �ؾ� �̹����� �ٲ۰� �����
                _showImage.material = null;
                _showImage.material = _tempMat;

                Debug.Log((tex.height / tex.width));

            }
            else
            {
                Debug.LogError("������ ������ �̹��� ������Ʈ�� �����ϴ�.");
            }
        }

        else
        {
            _showImage.material = null;
        }


    }
}
