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
        if (_furnitureModel.ChangedFurniture(costumeData)) // ���� ��ġ
        {
            WallID = costumeData.CostumeID;
        }
        else // ���� ���� ���
        {
            // ���� ���� ���� �ڵ� �ۼ�
            IsShowDetailView = true;
            
        }
    }

    public void RemoveFurniture()
    {
        WallID = "";
    }

    /// <summary>
    /// ������ ��ư ������ ���� </summary>
    public void ExitFurniture()
    {
        Debug.Log("exit: " + IsExitFurniture);
        if (IsExitFurniture == false)
        {
            IsExitFurniture = true;
        }
    }

    /// <summary>
    /// ���� ��ư ������ ����</summary>
    public void SaveFurniture()
    {
        IsSaveFurniture = true;
        _furnitureModel.SaveFurniture();
    }
}
