using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class UIAlbumSlot : MonoBehaviour
{
    [SerializeField] private Image _image;
    private PhotoData _photoData;
    private int _slotIndex;

    private Material _tempMat;

    private RectTransform _albumRect;

    public void Init(int slotIndex, PhotoData photoData)
    {
        _slotIndex = slotIndex;
        _photoData = photoData;
        _tempMat = new Material(_image.material);
        _albumRect = GetComponent<RectTransform>();
        SetImageByPhotoData(photoData);
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

            float heightMul =  (float)tex.height / (float)tex.width;
            if (_image != null)
            {
                //재질의 메인 텍스처를 위에서 읽어들인 이미지로 변경
                _tempMat.mainTexture = tex;

                _image.rectTransform.sizeDelta = new Vector2(0, (_albumRect.rect.height * heightMul) - _albumRect.rect.height);
                //재질을 null로 변경했다 다시 원래대로 변경한다. 이렇게 새로고침을 해야 이미지를 바꾼게 적용됨
                _image.material = null;
                _image.material = _tempMat;

                Debug.Log((tex.height / tex.width));


            }
            else
            {
                Debug.LogError("사진을 보여줄 이미지 컴포넌트가 없습니다.");
            }
        }

        else
        {
            _image.material = null;
        }
    }
}
