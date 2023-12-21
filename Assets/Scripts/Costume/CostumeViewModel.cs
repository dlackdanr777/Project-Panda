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

    public int WearingHeadCostumeID
    {
        get { return _costumeModel.WearingHeadCostumeID; }
        set
        {
            _costumeModel.WearingHeadCostumeID = value;
            if(value == -1)
            {
                CostumeChanged?.Invoke(null);
            }
            else
            {
                Debug.Log("코스튬 변경 이벤트 실행 " + WearingHeadCostumeID);
                CostumeChanged?.Invoke(CostumeManager.Instance.GetCostumeData(value));
            }
        }
    }
    public bool IsExitCostume
    {
        get { return _costumeModel.IsExitCostume; }
        set 
        {
            //if (!_isSetSaveCostumeView)
            //{
            //    SetSaveCostumeView?.Invoke(value);
            //}
            //_isSetSaveCostumeView = true;
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
        Debug.Log("판다 뷰모델 생성 WearingHeadCostumeID: " + WearingHeadCostumeID);
    }

    public void WearingCostume(CostumeData costumeData)
    {
        if (_costumeModel.WearingCostume(costumeData)) // 옷 장착
        {
            WearingHeadCostumeID = costumeData.CostumeID;
        }
        else // 옷 장착 해제
        {
            WearingHeadCostumeID = -1;
        }
    }

    /// <summary>
    /// 나가기 버튼 누르면 실행 </summary>
    public void ExitCostume()
    {
        Debug.Log("exit: "+ IsExitCostume);
        if (IsExitCostume == false)
        {
            IsExitCostume = true;
        }
    }

    /// <summary>
    /// 저장 버튼 누르면 실행</summary>
    public void SaveCostume()
    {
        IsSaveCostume = true;
        _costumeModel.SaveCostume();
    }
}
