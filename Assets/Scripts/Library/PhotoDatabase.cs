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

        //파일이 저장된 위치와 파일이름을 붙여 저장
        string path = photoData.PathFloder + photoData.FileName;

        //저장 위치에 있는 PNG파일을 읽어 Byte배열로 변환후 저장
        byte[] bytes = File.ReadAllBytes(path);

        Texture2D tex = new Texture2D(2, 2);

        //byte[]로 변환된 PNG파일을 읽어 이미지로 변환
        tex.LoadImage(bytes);

        _photo.Add(tex);
    }

}
