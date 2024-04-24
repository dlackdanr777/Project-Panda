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

    public FurnitureModel.FurnitureId[] FurnitureRooms { get{ return _furnitureModel.FurnitureRooms; } set{ } }
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

    /// <summary>
    /// FurnitureId ���� �� ���� </summary>
    private void ChangedFurnitureId(ERoom room)
    {
        _furnitureModel.FurnitureRooms = FurnitureRooms;

        if (FurnitureRooms[(int)room].FurnitureIds[(int)_furnitureType] == "")
        {
            FurnitureChanged?.Invoke(null);
        }
        else
        {
            FurnitureChanged?.Invoke(DatabaseManager.Instance.GetFurnitureItem()[FurnitureRooms[(int)room].FurnitureIds[(int)_furnitureType]]);
        }
    }


    public void ChangedFurniture(Furniture furnitureData, ERoom room)
    {
        _furnitureType = furnitureData.Type;
        if (_furnitureModel.ChangedFurniture(furnitureData, room)) // ���� ��ġ
        {
            FurnitureRooms[(int)room].FurnitureIds[(int)_furnitureType] = furnitureData.Id;
            ChangedFurnitureId(room);
        }
        else // ���� ���� ���
        {
            // ���� ���� ���� �ڵ� �ۼ�
            IsShowDetailView = true;
            
        }
    }

    /// <summary>
    /// ���� ���� </summary>
    public void RemoveFurniture(EFurnitureViewType currentField, ERoom room)
    {
        if (currentField == EFurnitureViewType.WallPaper || currentField == EFurnitureViewType.Floor)
        {
            _furnitureType = (FurnitureType)currentField;

            FurnitureRooms[(int)room].FurnitureIds[(int)_furnitureType] = "";
        }
        else
        {
            int field = 2 + ((int)currentField - 2) * 2;
            _furnitureType = (FurnitureType)field;

            FurnitureRooms[(int)room].FurnitureIds[field] = "";
            FurnitureRooms[(int)room].FurnitureIds[field + 1] = "";
        }
        ChangedFurnitureId(room);
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
