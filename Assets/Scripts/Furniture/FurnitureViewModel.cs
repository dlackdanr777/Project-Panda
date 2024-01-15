using System.ComponentModel;
using System;
using UnityEngine;

public class FurnitureViewModel
{
    public event PropertyChangedEventHandler PropertyChanged;
    public event Action<Furniture> FurnitureChanged;
    public event Action<bool> FurnitureSceneChanged;
    public event Action<bool> SetSaveFurnitureView;
    public event Action ShowDetailView;

    private FurnitureModel _furnitureModel;
    //private bool _isSetSaveCostumeView;

    public string WallPaperId
    {
        get { return _furnitureModel.WallPaperId; }
        set
        {
            _furnitureModel.WallPaperId = value;
            if (value == "")
            {
                FurnitureChanged?.Invoke(null);
            }
            else
            {
                FurnitureChanged?.Invoke(DatabaseManager.Instance.GetFurnitureItem()[value]);
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

    public void ChangedFurniture(Furniture furnitureData)
    {
        if (_furnitureModel.ChangedFurniture(furnitureData)) // ���� ��ġ
        {
            WallPaperId = furnitureData.Id;
        }
        else // ���� ���� ���
        {
            // ���� ���� ���� �ڵ� �ۼ�
            IsShowDetailView = true;
            
        }
    }

    public void RemoveFurniture()
    {
        WallPaperId = "";
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
