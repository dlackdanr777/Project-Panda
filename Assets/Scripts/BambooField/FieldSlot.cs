using UnityEngine;
using Muks.Tween;
using Muks.DataBind;
using Unity.VisualScripting;
using System;

public class FieldSlot : MonoBehaviour, IInteraction
{
    /// <summary> 성장 단계 </summary>
    public int GrowthStage;
    public float GrowthTime;
    public int Yield;

    /// <summary>
    /// 현재 키우는 작물 정보 </summary>
    public HarvestItem HarvestItem;

    [SerializeField] private string _growingCropID;
    [SerializeField] private GameObject[] _growingCropImage;
    [SerializeField] private GameObject _timeObject;

    private Vector3 _timeObjectPosition;

    private BambooFieldSystem BFieldSystem;
    private float _time;
    private float _second;
    private string _remainingTime;
    private bool _isGrowthComplete;
    private bool _isShowHavestItemDescription;
    private string _dataID;
    private TimeSpan _timeDifference;

    private void Start()
    {
        HarvestItem = DatabaseManager.Instance.GetHarvestItemdata(_growingCropID);
        for(int i = 0; i < _growingCropImage.Length; i++)
        {
            _growingCropImage[i].GetComponent<SpriteRenderer>().sprite = HarvestItem.Image[0];
        }
        GrowthTime = HarvestItem.HarvestTime * 6;

        _timeObjectPosition = _timeObject.transform.position;

        ChangeGrowthStageImage(0);

        _dataID = gameObject.name + "Time";

        SetTimeDifference();

        DataBind.SetTextValue(_dataID, _remainingTime);
    }

    private void Update()
    {
        _time += Time.deltaTime;
        _second += Time.deltaTime;

        if(_timeDifference != TimeSpan.Zero)
        {
            _timeDifference = _timeDifference - TimeSpan.FromSeconds(Mathf.FloorToInt(_second));
            _second -= Mathf.FloorToInt(_second);
            SetRemainimgTime();
        }
        else
        {
            _timeDifference = TimeSpan.Zero;
            _remainingTime = "채집 가능!";
        }

        DataBind.SetTextValue(_dataID, _remainingTime);

        if (HarvestItem != null)
        {
            IsIncreaseYields();
        }
        else
        {
            Debug.Log("HarvestItem null");
            HarvestItem = DatabaseManager.Instance.GetHarvestItemdata(_growingCropID);

        }

        if (_isShowHavestItemDescription == true && Input.GetMouseButtonDown(0))
        {
            ShowHavestItem();
        }
    }

    public void Init(BambooFieldSystem bambooFieldSystem, string growingCropID)
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
        if (!BFieldSystem.HarvestButton.IsSet && GrowthStage == 2)
        {

            BFieldSystem.HarvestButton.IsSet = true;
            //Tween.SpriteRendererAlpha(BFieldSystem.HarvestButton.gameObject, 1, 0.5f, TweenMode.Quadratic);
        }
    }

    /// <summary>
    /// 성장 단계에 맞춰 이미지 변경 </summary>
    public void ChangeGrowthStageImage(int growthStage)
    {
        if(GrowthStage == 0)
        {
            _timeObject.transform.position = _timeObjectPosition;
            for (int i = 0; i < _growingCropImage.Length; i++)
            {
                _growingCropImage[i].transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                _growingCropImage[i].transform.position = new Vector3(_growingCropImage[i].transform.position.x, transform.position.y + 0.5f, 0);
            }
            _isGrowthComplete = false;
        }
        else if(GrowthStage == 1)
        {
            _timeObject.transform.position = _timeObjectPosition + Vector3.up;
            for (int i = 0; i < _growingCropImage.Length; i++)
            {
                _growingCropImage[i].transform.localScale = new Vector3(2 / 3f, 2 / 3f, 2 / 3f);
                _growingCropImage[i].transform.position = _growingCropImage[i].transform.position + new Vector3(0, 0.4f, 0);
            }
        }

        else if (GrowthStage == 2)
        {
            _timeObject.transform.position = _timeObjectPosition + Vector3.up * 3;
            for (int i = 0;i < _growingCropImage.Length; i++)
            {
                _growingCropImage[i].transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
                _growingCropImage[i].transform.position = _growingCropImage[i].transform.position + new Vector3(0, 1f, 0);
            }
        }
        for (int i = 0; i < _growingCropImage.Length; i++)
        {
            _growingCropImage[i].GetComponent<SpriteRenderer>().sprite = HarvestItem.Image[growthStage];
        }
    }

    private void IsIncreaseYields()
    {
        if(_time > HarvestItem.HarvestTime * 60) // 시간 설정
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
        if (Yield == HarvestItem.MaxYield && !_isGrowthComplete)
        {
            _isGrowthComplete = true;
            GrowingCrops(2);
        }
        else if (Yield == GrowthTime * HarvestItem.Yield / HarvestItem.HarvestTime)
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

    public void SetTimeDifference()
    {
        _time = 0;
        _second = 0;
        _timeDifference = TimeSpan.FromSeconds(HarvestItem.HarvestTime * 60 * Mathf.CeilToInt(HarvestItem.MaxYield / (float)HarvestItem.Yield));
        SetRemainimgTime();
    }

    private void SetRemainimgTime()
    {
        if (_timeDifference.Hours == 0)
        {
            if (_timeDifference.Minutes == 0)
            {
                _remainingTime = string.Format("{0}s", _timeDifference.Seconds);

            }
            else
            {
                _remainingTime = string.Format("{0}m {1}s", _timeDifference.Minutes, _timeDifference.Seconds);
            }
        }
        else
        {
            _remainingTime = string.Format("{0}h {1}m", _timeDifference.Hours, _timeDifference.Minutes);
        }
    }

}
