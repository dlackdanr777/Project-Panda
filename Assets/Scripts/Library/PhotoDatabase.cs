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

    [Tooltip("데이터가 저장된 파일의 이름 지정")]
    private string _fileName => "PhotoData.json";


    public void Register()
    {
        Load();
    }


    public void Save()
    {
        if(_photoData.Count == 0)
        {
            Debug.Log("사진 데이터가 존재하지 않습니다.");
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
            Debug.Log("사진 데이터 저장 문서가 존재하지 않습니다.");
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
                Debug.Log(data.FileName + " 이 존재하지 않습니다.");
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

        //파일이 저장된 위치와 파일이름을 붙여 저장
        string path = photoData.PathFloder + photoData.FileName;

        //저장 위치에 있는 PNG파일을 읽어 Byte배열로 변환후 저장
        byte[] bytes = File.ReadAllBytes(path);

        Texture2D tex = new Texture2D(2, 2);

        //byte[]로 변환된 PNG파일을 읽어 이미지로 변환
        tex.LoadImage(bytes);

        _photo.Add(tex);
        _photoData.Add(photoData);
    }

}
