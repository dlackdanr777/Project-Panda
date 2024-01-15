using System.ComponentModel;
using System;
using UnityEngine;
using Newtonsoft.Json.Linq;

public class FurnitureViewModel
{
    public event PropertyChangedEventHandler PropertyChanged;
    public event Action<Furniture> FurnitureChanged;
    public event Action<bool> FurnitureSceneChanged;
    public event Action<bool> SetSaveFurnitureView;
    public event Action ShowDetailView;

    private FurnitureModel _furnitureModel;
    private FurnitureType _furnitureType;
    //private bool _isSetSaveCostumeView;

    public string[] FurnitureId { get{ return _furnitureModel.FurnitureId; } set{ } }
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

    private void ChangedFurnitureId()
    {
        _furnitureModel.FurnitureId = FurnitureId;

        if (FurnitureId[(int)_furnitureType] == "")
        {
            FurnitureChanged?.Invoke(null);
        }
        else
        {
            FurnitureChanged?.Invoke(DatabaseManager.Instance.GetFurnitureItem()[FurnitureId[(int)_furnitureType]]);
        }
    }


    public void ChangedFurniture(Furniture furnitureData)
    {
        _furnitureType = furnitureData.Type;
        if (_furnitureModel.ChangedFurniture(furnitureData)) // 가구 배치
        {
            FurnitureId[(int)_furnitureType] = furnitureData.Id;
            ChangedFurnitureId();
        }
        else // 가구 정보 띄움
        {
            // 가구 정보 띄우는 코드 작성
            IsShowDetailView = true;
            
        }
    }

    /// <summary>
    /// 가구 제거 </summary>
    public void RemoveFurniture(EFurnitureViewType currentField)
    {
        if (currentField == EFurnitureViewType.WallPaper || currentField == EFurnitureViewType.Floor)
        {
            _furnitureType = (FurnitureType)currentField;

            FurnitureId[(int)_furnitureType] = "";
        }
        else
        {
            int field = 2 + ((int)currentField - 2) * 2;
            _furnitureType = (FurnitureType)field;

            FurnitureId[field] = "";
            FurnitureId[field + 1] = "";
        }
            ChangedFurnitureId();
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
