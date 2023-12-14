using System.Collections;
using System.Collections.Generic;
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

    public int Count => _photoData.Count;

    [Tooltip("�����Ͱ� ����� ������ �̸� ����")]
    private string _fileName => "PhotoData.json";


    public void Register()
    {
        Load();
    }


    public void Save()
    {
        if(_photoData.Count == 0)
        {
            Debug.Log("���� �����Ͱ� �������� �ʽ��ϴ�.");
            return;
        }

        PhotoDatas saveDatas = new PhotoDatas();

        foreach(PhotoData data in _photoData)
        {
            saveDatas.PhotoDataList.Add(data);
        }

        string json = JsonUtility.ToJson(saveDatas, true);

        if (!Directory.Exists(UserInfo.PhotoPath))
            Directory.CreateDirectory(UserInfo.PhotoPath);

        File.WriteAllText(UserInfo.PhotoPath + _fileName, json);
    }


    public void Load()
    {
        if (!File.Exists(UserInfo.PhotoPath + _fileName))
        {
            Debug.Log("���� ������ ���� ������ �������� �ʽ��ϴ�.");
            return;
        }
            
        PhotoDatas photoDatas = new PhotoDatas();

        string loadJson = File.ReadAllText(UserInfo.PhotoPath + _fileName);
        Debug.Log(loadJson);
        photoDatas = JsonUtility.FromJson<PhotoDatas>(loadJson);

        foreach (PhotoData data in photoDatas.PhotoDataList)
        {         
            if(!File.Exists(data.PathFloder + data.FileName))
            {
                Debug.Log(data.FileName + " �� �������� �ʽ��ϴ�.");
                continue;
            }
                _photoData.Add(data);
        }
    }


    public List<PhotoData> GetPhotoDataList()
    {
        return _photoData;
    }

    public PhotoData GetPhotoData(int index)
    {
        return _photoData[index];
    }


    public void SavePhotoData(PhotoData photoData)
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
        _photoData.Add(photoData);
    }

}
