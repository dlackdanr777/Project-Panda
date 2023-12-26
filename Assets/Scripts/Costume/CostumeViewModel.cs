using System.ComponentModel;
using System;
using UnityEngine;
using UnityEngine.UI;

public class CostumeViewModel
{
    public event PropertyChangedEventHandler PropertyChanged;
    public event Action<CostumeData> CostumeChanged;
    public event Action<bool> CostumeSceneChanged;
    public event Action<bool> SetSaveCostumeView;
    
    private CostumeModel _costumeModel;
    //private bool _isSetSaveCostumeView;

    public string WearingHeadCostumeID
    {
        get { return _costumeModel.WearingHeadCostumeID; }
        set
        {
            _costumeModel.WearingHeadCostumeID = value;
            if(value == null)
            {
                CostumeChanged?.Invoke(null);
            }
            else
            {
                CostumeChanged?.Invoke(CostumeManager.Instance.GetCostumeData(value));
            }
        }
    }
    public bool IsExitCostume
    {
        get { return _costumeModel.IsExitCostume; }
        set 
        {
            _costumeModel.IsExitCostume = value;
            if(value == true)
            {
                CostumeSceneChanged?.Invoke(value);
            }
        }
    }

    public bool IsSaveCostume
    {
        get { return _costumeModel.IsSaveCostume; }
        set { _costumeModel.IsSaveCostume = value; }
    }


    public CostumeViewModel()
    {
        _costumeModel = new CostumeModel();
        _costumeModel.Init();
    }

    public void WearingCostume(CostumeData costumeData)
    {
        if (_costumeModel.WearingCostume(costumeData)) // �� ����
        {
            WearingHeadCostumeID = costumeData.CostumeID;
        }
        else // �� ���� ����
        {
            WearingHeadCostumeID = null;
        }
    }

    /// <summary>
    /// ������ ��ư ������ ���� </summary>
    public void ExitCostume()
    {
        Debug.Log("exit: "+ IsExitCostume);
        if (IsExitCostume == false)
        {
            IsExitCostume = true;
        }
    }

    /// <summary>
    /// ���� ��ư ������ ����</summary>
    public void SaveCostume()
    {
        IsSaveCostume = true;
        _costumeModel.SaveCostume();
    }
}
