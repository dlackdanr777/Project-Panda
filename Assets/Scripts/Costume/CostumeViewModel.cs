using System.ComponentModel;
using System;
using UnityEngine;
using UnityEngine.UI;

public class CostumeViewModel //: INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    public event Action<CostumeData> CostumeChanged;
    
    private CostumeModel _costumeModel;

    public int WearingHeadCostumeID
    {
        get { return _costumeModel.WearingHeadCostumeID; }
        set
        {
            _costumeModel.WearingHeadCostumeID = value;
            //OnPropertyChanged("WearingHeadCostumeID");
            if(value == -1)
            {
                CostumeChanged?.Invoke(null);
            }
            else
            {
                CostumeChanged?.Invoke(CostumeManager.Instance.GetCostumeData(value));
            }
        }
    }

    public CostumeViewModel()
    {
        _costumeModel = new CostumeModel();
        _costumeModel.Init();
    }

    //// name에 해당하는 이름을 갖는 데이터에 변화가 생길 때마다 이벤트 발생
    //protected void OnPropertyChanged(string name)
    //{
    //    PropertyChangedEventHandler handler = PropertyChanged;
    //    if(handler != null)
    //    {
    //        handler(this, new PropertyChangedEventArgs(name));
    //    }
    //}

    internal void WearingCostume(CostumeData costumeData)
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
}
