using Muks.Tween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;


public class UIPicture : UIView
{
    [SerializeField] private UIAlbum _uiAlbum;

    [SerializeField] private Image _showImage;

    [SerializeField] private Image _maskImage;

    [SerializeField] private Image _background;

    [SerializeField] private Button _backgroundButton;

    [SerializeField] private float _animeDuration;

    [SerializeField] private TweenMode _animeTween;

    private RectTransform _uiAlbumRect;

    private Material _tempMat;

    private Vector2 _targetPos;

    private Vector2 _slotSize => new Vector2(285, 285);

    private Vector2 _tempUiAlbumSize;

    private float _texRatio;

    public override void Init(UINavigation uiNav)
    {
        base.Init(uiNav);
        //gameObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        _uiAlbumRect = _uiAlbum.GetComponent<RectTransform>();
        _maskImage.rectTransform.sizeDelta = _slotSize;
        _tempMat = new Material(_showImage.material);

        _background.color = new Color(_background.color.r, _background.color.g, _background.color.b, 0);
        _background.gameObject.SetActive(false);
        UIAlbumSlot.OnButtonClickHandler += OnAlbumButtonClicked;
    }


    public override void Show()
    {
        Tween.Stop(gameObject);
        _background.gameObject.SetActive(true);
        _backgroundButton.gameObject.SetActive(true);
        gameObject.SetActive(true);
        VisibleState = VisibleState.Appearing;

        _tempUiAlbumSize = new Vector2(_uiAlbumRect.rect.width, _uiAlbumRect.rect.height);

        float _albumRatio =  _tempUiAlbumSize.y / _tempUiAlbumSize.x;
        Vector2 targetSize = Vector2.zero;

        if (_texRatio < _albumRatio)
            targetSize = new Vector2(_tempUiAlbumSize.x, _tempUiAlbumSize.x * _texRatio);
        else
            targetSize = new Vector2(_tempUiAlbumSize.y / _texRatio, _tempUiAlbumSize.y);

        Tween.RectTransfromAnchoredPosition(_maskImage.gameObject, new Vector2(0, 0), _animeDuration, _animeTween);

        Tween.IamgeAlpha(_background.gameObject, 1, _animeDuration * 0.5f, _animeTween);
        Tween.RectTransfromSizeDelta(_maskImage.gameObject, _tempUiAlbumSize, _animeDuration, _animeTween);
        Tween.RectTransfromSizeDelta(_showImage.gameObject, targetSize, _animeDuration, _animeTween, () =>
        {
            _maskImage.rectTransform.anchoredPosition = new Vector2(0, 0);
            _showImage.rectTransform.sizeDelta = _tempUiAlbumSize;
            _maskImage.rectTransform.sizeDelta = targetSize;
            VisibleState = VisibleState.Appeared;
        });

    }


    public override void Hide()
    {
        Tween.Stop(gameObject);
        VisibleState = VisibleState.Disappearing;

        Tween.TransformMove(_maskImage.gameObject, _targetPos, _animeDuration, _animeTween);

        Tween.IamgeAlpha(_background.gameObject, 0, _animeDuration * 0.5f, _animeTween);
        Tween.RectTransfromSizeDelta(_maskImage.gameObject, _slotSize, _animeDuration, _animeTween);
        Tween.RectTransfromSizeDelta(_showImage.gameObject, new Vector2(_slotSize.x, _slotSize.y * _texRatio), _animeDuration, _animeTween, () =>
        {
            VisibleState = VisibleState.Disappeared;

            _maskImage.rectTransform.anchoredPosition = _targetPos;
            _maskImage.rectTransform.sizeDelta = _slotSize;
            _showImage.rectTransform.sizeDelta = new Vector2(_slotSize.x, _slotSize.y * _texRatio);

            gameObject.SetActive(false);
            _background.gameObject.SetActive(false);
            _backgroundButton.gameObject.SetActive(false);
        });
    }


    private void OnAlbumButtonClicked(PhotoData photoData, Vector3 buttonPos)
    {
        if (VisibleState == VisibleState.Appearing || VisibleState == VisibleState.Disappearing)
            return;

        _targetPos = buttonPos;
        transform.position = buttonPos;

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

                _texRatio = (float)tex.height / tex.width;
                _showImage.rectTransform.sizeDelta = new Vector2(_slotSize.x, _slotSize.y * _texRatio);

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
