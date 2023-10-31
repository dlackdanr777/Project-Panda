using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class PhotoData
{
    public string Name;
    public string Description;
    public string Date;
    public Texture2D Photo;

    public PhotoData(string name, string description, string date, Texture2D photo)
    {
        Name = name;
        Description = description;
        Date = date;
        Photo = photo;
    }
}



public class PhotoDatabase : MonoBehaviour
{
    [Tooltip("사진 데이터 리스트")]
    [SerializeField] private List<PhotoData> _data;


    private void Awake()
    {
        _data = new List<PhotoData>();

    }


    public void AddPhotoData(PhotoData photoData)
    {
        _data?.Add(photoData);
    }


    public Texture2D SetPhotoDataToTexture2D(int index)
    {
        if (index >= _data.Count || index < 0)
            return default;

        //Texture2D photo = new Texture2D(300, 300);

        //photo.LoadRawTextureData(_data[index].Photo);
        //photo.Apply();

        return _data[index].Photo;
    }

    public Sprite SetPhotoDataToSprite(int index)
    {
        Texture2D photo =  SetPhotoDataToTexture2D(index);
        Rect rect = new Rect(0,0, photo.width,photo.height);
        Sprite sprite = Sprite.Create(photo, rect, new Vector2(0.5f, 0.5f));

        return sprite;
    }
}
