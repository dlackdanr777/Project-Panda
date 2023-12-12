using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FieldSlot : MonoBehaviour, IInteraction
{
    /// <summary> 성장 단계 </summary>
    public int GrowthStage;

    public HarvestItem HarvestItem;
    [SerializeField] private GameObject _harvestButton;
    [SerializeField] private int _growingCropID;
    private Sprite _growingCropImage;
    private float _time;

    private void Start()
    {
        // 우선 죽순ID로 설정
        _growingCropID = 0;

        HarvestItem = HarvestItemManager.Instance.GetHarvestItemdata(_growingCropID);
        _growingCropImage = HarvestItem.Image[0];
    }

    private void Update()
    {
        _time += Time.deltaTime;
    }


    /// <summary>
    /// 작물 수확 버튼 클릭 </summary>
    public void ClickHavestButton()
    {
        // 수확
        // 애니메이션 - 죽순이 수집되며 우측 상단의 죽순보유량으로 빨려들어가는 느낌의 애니메이션, 하나씩 빨려들어갈 때마다 죽순량이 동적으로 변화
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
    /// 성장 단계 변경 시 </summary>
    public void GrowingCrops(int growthStage)
    {
        if(GrowthStage + growthStage < 3) 
        {
            GrowthStage += growthStage;   
        }
        else
        {
            GrowthStage = 2; // 최대 성장단계로 설정
        }
        // 이미지 변경
        _growingCropImage = HarvestItem.Image[growthStage];

        // 버튼 생성
        _harvestButton.SetActive(true);
    }

    private void IncreaseYields()
    {
        // 1분당 1 수확
        HarvestItem.Yield += Time.deltaTime / 60;
        if(HarvestItem.Yield > HarvestItem.MaxYield)
        {
            HarvestItem.Yield = HarvestItem.MaxYield;
        }

        //일정 시간이 지나면 버튼 생성 후 수확
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
    /// 현재 키우는 작물의 정보 띄움 </summary>
    private void ShowHavestItem()
    {
        Debug.Log(HarvestItem.Description);
    }


}
