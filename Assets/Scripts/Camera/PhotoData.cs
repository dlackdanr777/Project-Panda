using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PhotoData
{
    [SerializeField] private string _fileName;

    [SerializeField] private string _date = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss")
    private string _pathFloder => Application.persistentDataPath;

    public PhotoData(string fileName)
    {
        _fileName = fileName;
    }
}
