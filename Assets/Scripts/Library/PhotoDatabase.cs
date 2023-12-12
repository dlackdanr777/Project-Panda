using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using System.IO;

public class PhotoDatas
{
    public List<PhotoData> PhotoDataList = new List<PhotoData>();
}

public class PhotoDatabase
{
    private List<PhotoData> _photoData = new List<PhotoData>();

    private List<Texture2D> _photo = new List<Texture2D>();
    public void Register()
    {
        Load();
    }


    public void Save()
    {
        PhotoDatas saveDatas = new PhotoDatas();
        foreach(PhotoData data in _photoData)
        {
            saveDatas.PhotoDataList.Add(data);
        }

        string json = JsonUtility.ToJson(saveDatas, true);

        File.WriteAllText(UserInfo.PhotoPath, json);
    }

    public void Load()
    {
        PhotoDatas photoDatas = new PhotoDatas();

        string loadJson = File.ReadAllText(UserInfo.PhotoPath);
        photoDatas = JsonUtility.FromJson<PhotoDatas>(loadJson);

        foreach (PhotoData data in photoDatas.PhotoDataList)
        {
            _photoData.Add(data);
        }
    }

    private void SetImageByPhotoData(PhotoData photoData)
    {
        if (photoData == null)
            return;

        //������ ����� ��ġ�� �����̸��� �ٿ� ����
        string path = photoData.PathFloder + photoData.FileName;

        //���� ��ġ�� �ִ� PNG������ �о� Byte�迭�� ��ȯ�� ����
        byte[] bytes = File.ReadAllBytes(path);

        Texture2D tex = new Texture2D(2, 2);

        //byte[]�� ��ȯ�� PNG������ �о� �̹����� ��ȯ
        tex.LoadImage(bytes);

        _photo.Add(tex);
    }

}
