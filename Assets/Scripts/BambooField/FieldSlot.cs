using UnityEngine;
using Muks.Tween;

public class FieldSlot : MonoBehaviour, IInteraction
{
    /// <summary> 성장 단계 </summary>
    public int GrowthStage;

    public HarvestItem HarvestItem;
    [SerializeField] private HarvestButton _harvestButton;
    [SerializeField] private int _growingCropID;
    [SerializeField] private GameObject _growingCropImage;
    private float _time;
    public int Yield;

    private void Start()
    {
        // 우선 죽순ID로 설정
        _growingCropID = 0;
        Debug.Log("시작");
        HarvestItem = HarvestItemManager.Instance.GetHarvestItemdata(_growingCropID);
        _growingCropImage.GetComponent<SpriteRenderer>().sprite = HarvestItem.Image[0];
    }

    private void Update()
    {
        _time += Time.deltaTime;
        IsIncreaseYields();
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

        ChangeGrowthStageImage(GrowthStage);

        // 버튼 생성
        if (!_harvestButton.IsSet)
        {
            _harvestButton.IsSet = true;
            Tween.SpriteRendererAlpha(_harvestButton.gameObject, 1, 0.5f, TweenMode.Quadratic);
        }
    }

    /// <summary>
    /// 성장 단계에 맞춰 이미지 변경 </summary>
    public void ChangeGrowthStageImage(int growthStage)
    {
        _growingCropImage.GetComponent<SpriteRenderer>().sprite = HarvestItem.Image[growthStage];
    }

    private void IsIncreaseYields()
    {
        if(_time > HarvestItem.HarvestTime *2)// 나중에 수정 *60
        {
            IncreaseYields();
            _time = 0;
        }
    }

    private void IncreaseYields()
    {
        Yield++; //Yield += HarvestItem.Yield; // 바꾸기
        if(Yield > HarvestItem.MaxYield)
        {
            Yield = HarvestItem.MaxYield;
        }

        Debug.Log("수확량: " + Yield);
        //일정 시간이 지나면 버튼 생성 후 수확
        if (Yield == 10) //HarvestItem.Yield * 10 수정
        {
            GrowingCrops(2);
        }
        else if (Yield == 5)
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
