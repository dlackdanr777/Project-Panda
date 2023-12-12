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
    /// PhotoData 클래스의 정보로 PC에 저장된 PNG파일을 Image오브젝트의 이미지를 변환하는 함수  
    /// </summary>
    private void SetImageByPhotoData(PhotoData photoData)
    {
        if (photoData != null)
        {
            //파일이 저장된 위치와 파일이름을 붙여 저장
            string path = photoData.PathFloder + photoData.FileName;

            //저장 위치에 있는 PNG파일을 읽어 Byte배열로 변환후 저장
            byte[] bytes = File.ReadAllBytes(path);

            Texture2D tex = new Texture2D(2, 2);

            //byte[]로 변환된 PNG파일을 읽어 이미지로 변환
            tex.LoadImage(bytes);

            float heightMul = (float)tex.height / (float)tex.width;
            if (_showImage != null)
            {
                //재질의 메인 텍스처를 위에서 읽어들인 이미지로 변경
                _tempMat.mainTexture = tex;

                //_showImage.rectTransform.sizeDelta = new Vector2(0, (RectTransform.rect.height * heightMul) - RectTransform.rect.height);
                //재질을 null로 변경했다 다시 원래대로 변경한다. 이렇게 새로고침을 해야 이미지를 바꾼게 적용됨
                _showImage.material = null;
                _showImage.material = _tempMat;

                Debug.Log((tex.height / tex.width));

            }
            else
            {
                Debug.LogError("사진을 보여줄 이미지 컴포넌트가 없습니다.");
            }
        }

        else
        {
            _showImage.material = null;
        }


    }
}
