using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PhotoData
{
    [SerializeField] private string _fileName;
    public string FileName => _fileName;

    [SerializeField] private string _date;
    public string Date => _date;

    [SerializeField] private string _pathFloder;
    public string PathFloder => _pathFloder;

    public PhotoData(string fileName, string pathFloder)
    {
        _fileName = fileName;
        _pathFloder = pathFloder;
        _date = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");


        DatabaseManager.Instance.PhotoDatabase.SavePhotoData(this);
    }

}
