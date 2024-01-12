using System.ComponentModel;
using System;
using UnityEngine;

public class FurnitureViewModel
{
    public event PropertyChangedEventHandler PropertyChanged;
    public event Action<CostumeData> FurnitureChanged;
    public event Action<bool> FurnitureSceneChanged;
    public event Action<bool> SetSaveFurnitureView;
    public event Action ShowDetailView;

    private FurnitureModel _furnitureModel;
    //private bool _isSetSaveCostumeView;

    public string WallID
    {
        get { return _furnitureModel.WallID; }
        set
        {
            _furnitureModel.WallID = value;
            if (value == "")
            {
                FurnitureChanged?.Invoke(null);
            }
            else
            {
                FurnitureChanged?.Invoke(CostumeManager.Instance.GetCostumeData(value));
            }
        }
    }
    public bool IsExitFurniture
    {
        get { return _furnitureModel.IsExitFurniture; }
        set
        {
            _furnitureModel.IsExitFurniture = value;
            if (value == true)
            {
                FurnitureSceneChanged?.Invoke(value);
            }
        }
    }

    public bool IsSaveFurniture
    {
        get { return _furnitureModel.IsSaveFurniture; }
        set { _furnitureModel.IsSaveFurniture = value; }
    }

    public bool IsShowDetailView
    {
        get { return _furnitureModel.IsShowDetailView; }
        set 
        {
            _furnitureModel.IsShowDetailView = value;
            if(value == true)
            {
                ShowDetailView?.Invoke();
            }
        }
    }

    public FurnitureViewModel()
    {
        _furnitureModel = new FurnitureModel();
        _furnitureModel.Init();
    }

    public void ChangedFurniture(CostumeData costumeData)
    {
        if (_furnitureModel.ChangedFurniture(costumeData)) // 가구 배치
        {
            WallID = costumeData.CostumeID;
        }
        else // 가구 정보 띄움
        {
            // 가구 정보 띄우는 코드 작성
            IsShowDetailView = true;
            
        }
    }

    public void RemoveFurniture()
    {
        WallID = "";
    }

    /// <summary>
    /// 나가기 버튼 누르면 실행 </summary>
    public void ExitFurniture()
    {
        Debug.Log("exit: " + IsExitFurniture);
        if (IsExitFurniture == false)
        {
            IsExitFurniture = true;
        }
    }

    /// <summary>
    /// 저장 버튼 누르면 실행</summary>
    public void SaveFurniture()
    {
        IsSaveFurniture = true;
        _furnitureModel.SaveFurniture();
    }
}
