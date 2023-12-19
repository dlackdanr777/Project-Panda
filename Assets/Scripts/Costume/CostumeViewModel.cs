using System.ComponentModel;
using System;
using UnityEngine;
using UnityEngine.UI;

public class CostumeViewModel : INotifyPropertyChanged
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
            OnPropertyChanged("WearingHeadCostumeID");
            CostumeChanged?.Invoke(CostumeManager.Instance.GetCostumeData(value));
        }
    }

    public CostumeViewModel()
    {
        _costumeModel = new CostumeModel();
        _costumeModel.Init();
    }

    // name�� �ش��ϴ� �̸��� ���� �����Ϳ� ��ȭ�� ���� ������ �̺�Ʈ �߻�
    protected void OnPropertyChanged(string name)
    {
        PropertyChangedEventHandler handler = PropertyChanged;
        if(handler != null)
        {
            handler(this, new PropertyChangedEventArgs(name));
        }
    }

    internal void WearingCostume()
    {
        throw new NotImplementedException();
    }
}
