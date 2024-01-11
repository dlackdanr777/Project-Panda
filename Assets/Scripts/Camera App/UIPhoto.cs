using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPhoto : UIView
{
    [Tooltip("인화된 사진을 출력할 이미지 오브젝트")]
    [SerializeField] private Image _image;

    [Tooltip("인화된 사진의 프레임")]
    [SerializeField] private RectTransform _background;

    Material _tempMat;

    public override void Init(UINavigation uiNav)
    {
        base.Init(uiNav);
        _tempMat = _image.material;
        ScreenshotCamera.OnStartHandler += ResizeImage;
    }

    public void OnDestroy()
    {
        ScreenshotCamera.OnStartHandler -= ResizeImage;
    }

    public override void Show()
    {
        gameObject.SetActive(true);
        VisibleState = VisibleState.Appeared;

    }

    public override void Hide()
    {
        gameObject.SetActive(false);
        VisibleState = VisibleState.Disappeared;
    }


    public void Show(Texture2D texture)
    {
        _uiNav.Push("Photo");
        _tempMat.mainTexture = texture;
        _image.material = _tempMat;
    }

    public void ResizeImage(int width, int height)
    {
        float heightRatio = (float)height / width;

        float imageWidth = _background.rect.width;

        _background.sizeDelta = new Vector2(imageWidth, imageWidth * heightRatio);
    }
}
