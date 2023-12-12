using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FieldSlot : MonoBehaviour, IInteraction
{
    /// <summary> ���� �ܰ� </summary>
    public int GrowthStage;

    public HarvestItem HarvestItem;
    [SerializeField] private GameObject _harvestButton;
    [SerializeField] private int _growingCropID;
    private Sprite _growingCropImage;
    private float _time;

    private void Start()
    {
        // �켱 �׼�ID�� ����
        _growingCropID = 0;

        HarvestItem = HarvestItemManager.Instance.GetHarvestItemdata(_growingCropID);
        _growingCropImage = HarvestItem.Image[0];
    }

    private void Update()
    {
        _time += Time.deltaTime;
    }


    /// <summary>
    /// �۹� ��Ȯ ��ư Ŭ�� </summary>
    public void ClickHavestButton()
    {
        // ��Ȯ
        // �ִϸ��̼� - �׼��� �����Ǹ� ���� ����� �׼����������� �������� ������ �ִϸ��̼�, �ϳ��� ������ ������ �׼����� �������� ��ȭ
    }

    public void StartInteraction()
    {
        ShowHavestItem();
    }

    public void UpdateInteraction()
    {

    }
    public void ExitInteraction()
    {

    }

    /// <summary>
    /// ���� �ܰ� ���� �� </summary>
    public void GrowingCrops(int growthStage)
    {
        if(GrowthStage + growthStage < 3) 
        {
            GrowthStage += growthStage;   
        }
        else
        {
            GrowthStage = 2; // �ִ� ����ܰ�� ����
        }
        // �̹��� ����
        _growingCropImage = HarvestItem.Image[growthStage];

        // ��ư ����
        _harvestButton.SetActive(true);
    }

    private void IncreaseYields()
    {
        // 1�д� 1 ��Ȯ
        HarvestItem.Yield += Time.deltaTime / 60;
        if(HarvestItem.Yield > HarvestItem.MaxYield)
        {
            HarvestItem.Yield = HarvestItem.MaxYield;
        }

        //���� �ð��� ������ ��ư ���� �� ��Ȯ
        if (HarvestItem.Yield > 10)
        {
            GrowingCrops(2);
        }
        else if (HarvestItem.Yield > 5)
        {
            GrowingCrops(1);
        }
    }
    
    /// <summary>
    /// ���� Ű��� �۹��� ���� ��� </summary>
    private void ShowHavestItem()
    {
        Debug.Log(HarvestItem.Description);
    }


}
