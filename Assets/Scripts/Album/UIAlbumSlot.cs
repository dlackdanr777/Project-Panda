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
    public void Init(int slotIndex, PhotoData photoData)
    {
        _slotIndex = slotIndex;
        _photoData = photoData;
        _tempMat = new Material(_image.material);
        SetImageByPhotoData(photoData);
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

            if (_image != null)
            {
                //������ ���� �ؽ�ó�� ������ �о���� �̹����� ����
                _tempMat.mainTexture = tex;

                //������ null�� �����ߴ� �ٽ� ������� �����Ѵ�. �̷��� ���ΰ�ħ�� �ؾ� �̹����� �ٲ۰� �����
                _image.material = null;
                _image.material = _tempMat;

            }
            else
            {
                Debug.LogError("������ ������ �̹��� ������Ʈ�� �����ϴ�.");
            }
        }

        else
        {
            _image.material = null;
        }
    }
}
