using UnityEngine;
using Muks.Tween;
using Muks.DataBind;
using Unity.VisualScripting;

public class FieldSlot : MonoBehaviour, IInteraction
{
    /// <summary> 성장 단계 </summary>
    public int GrowthStage;
    public float GrowthTime;
    public int Yield;

    /// <summary>
    /// 현재 키우는 작물 정보 </summary>
    public HarvestItem HarvestItem;

    [SerializeField] private int _growingCropID;
    [SerializeField] private GameObject _growingCropImage;

    private BambooFieldSystem BFieldSystem;
    private float _time;
    private bool _isGrowthComplete;
    private bool _isShowHavestItemDescription;

    private void Start()
    {
        HarvestItem = DatabaseManager.Instance.GetHarvestItemdata(_growingCropID);
        _growingCropImage.GetComponent<SpriteRenderer>().sprite = HarvestItem.Image[0];
        GrowthTime = HarvestItem.HarvestTime * 5;
    }

    private void Update()
    {
        _time += Time.deltaTime;
        if(HarvestItem != null)
        {
            IsIncreaseYields();
        }
        else
        {
            Debug.Log("HarvestItem null");
        }

        if(_isShowHavestItemDescription == true && Input.GetMouseButtonDown(0))
        {
            ShowHavestItem();
        }
    }

    public void Init(BambooFieldSystem bambooFieldSystem, int growingCropID)
    {
        BFieldSystem = bambooFieldSystem;
        _growingCropID = growingCropID;
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
        if (!BFieldSystem.HarvestButton.IsSet)
        {
            BFieldSystem.HarvestButton.IsSet = true;
            Tween.SpriteRendererAlpha(BFieldSystem.HarvestButton.gameObject, 1, 0.5f, TweenMode.Quadratic);
        }
    }

    /// <summary>
    /// 성장 단계에 맞춰 이미지 변경 </summary>
    public void ChangeGrowthStageImage(int growthStage)
    {
        if(GrowthStage == 0)
        {
            _growingCropImage.transform.localScale = new Vector3(3, 3, 3);
            _growingCropImage.transform.position = transform.position + new Vector3(0, 0.5f, 0);
            _isGrowthComplete = false;
        }
        else if(GrowthStage == 1)
        {
            //_growingCropImage.transform.localScale = new Vector3(5, 5, 5);
            _growingCropImage.transform.position = _growingCropImage.transform.position + new Vector3(0, 1, 0);
        }

        else if (GrowthStage == 2)
        {
            //_growingCropImage.transform.localScale = new Vector3(5, 5, 5);
            _growingCropImage.transform.position = _growingCropImage.transform.position + new Vector3(0, 1.5f, 0);
        }
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
        Yield += HarvestItem.Yield;
        if(Yield > HarvestItem.MaxYield)
        {
            Yield = HarvestItem.MaxYield;
        }

        //일정 시간이 지나면 버튼 생성 후 수확
        if (Yield == GrowthTime * 2 && !_isGrowthComplete)
        {
            _isGrowthComplete = true;
            GrowingCrops(2);
        }
        else if (Yield == GrowthTime)
        {
            GrowingCrops(1);
        }
    }
    
    /// <summary>
    /// 현재 키우는 작물의 정보 띄움 </summary>
    private void ShowHavestItem()
    {
        _isShowHavestItemDescription = !_isShowHavestItemDescription;
        if (_isShowHavestItemDescription)
        {
            BFieldSystem._UIBambooField.transform.position = Camera.main.WorldToScreenPoint(transform.position);
            BFieldSystem._UIBambooField.SetActive(true);
            Tween.IamgeAlpha(BFieldSystem._UIBambooField, 0.5f, 0.2f, TweenMode.Quadratic);
            DataBind.SetTextValue("CropDescription", HarvestItem.Description);
        }
        else
        {
            Tween.IamgeAlpha(BFieldSystem._UIBambooField, 0, 0.2f, TweenMode.Quadratic, () =>
            {
                BFieldSystem._UIBambooField.SetActive(false);
            });
        }

    }


}
