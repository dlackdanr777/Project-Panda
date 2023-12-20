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

    //// name�� �ش��ϴ� �̸��� ���� �����Ϳ� ��ȭ�� ���� ������ �̺�Ʈ �߻�
    //protected void OnPropertyChanged(string name)
    //{
    //    PropertyChangedEventHandler handler = PropertyChanged;
    //    if(handler != null)
    //    {
    //        handler(this, new PropertyChangedEventArgs(name));
    //    }
    //}

    public void WearingCostume(CostumeData costumeData)
    {
        if (_costumeModel.WearingCostume(costumeData)) // �� ����
        {
            WearingHeadCostumeID = costumeData.CostumeID;
        }
        else // �� ���� ����
        {
            WearingHeadCostumeID = -1;
        }
    }

    public void SaveCostume()
    {
        IsSaveCostume = true;
        _costumeModel.SaveCostume();
    }
}
