using UnityEngine;
using Muks.Tween;
using Muks.DataBind;
using Unity.VisualScripting;
using System;

public class FieldSlot : MonoBehaviour, IInteraction
{
    /// <summary> ���� �ܰ� </summary>
    public int GrowthStage;
    public float GrowthTime;
    public int Yield;

    /// <summary>
    /// ���� Ű��� �۹� ���� </summary>
    public HarvestItem HarvestItem;

    [SerializeField] private string _growingCropID;
    [SerializeField] private GameObject[] _growingCropImage;
    [SerializeField] private GameObject _timeObject;

    private Vector3 _timeObjectPosition;

    private BambooFieldSystem BFieldSystem;
    //private float _time;
    //private float _second;
    private string _remainingTime;
    private bool _isGrowthComplete;
    private bool _isShowHavestItemDescription;
    private bool _isShowHavestItemDescriptionTrue;
    private string _dataID;
    private TimeSpan _timeDifference;

    private void Start()
    {
        HarvestItem = DatabaseManager.Instance.GetHarvestItemdata(_growingCropID);
        for(int i = 0; i < _growingCropImage.Length; i++)
        {
            _growingCropImage[i].GetComponent<SpriteRenderer>().sprite = HarvestItem.Image[0];
        }
        GrowthTime = HarvestItem.HarvestTime * 6; // HarvestTime�� 6���̸� �۹� ����

        _timeObjectPosition = _timeObject.transform.position;

        ChangeGrowthStageImage(0);

        _dataID = gameObject.name + "Time";

        //SetTimeDifference();

        DataBind.SetTextValue(_dataID, _remainingTime);
    }

    private void Update()
    {
        //_time += Time.deltaTime;
        //_second += Time.deltaTime;

        //if(_timeDifference != TimeSpan.Zero)
        //{
        //    _timeDifference = _timeDifference - TimeSpan.FromSeconds(Mathf.FloorToInt(_second));
        //    _second -= Mathf.FloorToInt(_second);
        //    SetRemainingTime();
        //}
        //else
        //{
        //    _timeDifference = TimeSpan.Zero;
        //    SetRemainingTimeZero();
        //}

        DataBind.SetTextValue(_dataID, _remainingTime);

        //if (HarvestItem != null)
        //{
        //    IsIncreaseYields();
        //}
        //else
        //{
        //    Debug.Log("HarvestItem null");
        //    HarvestItem = DatabaseManager.Instance.GetHarvestItemdata(_growingCropID);

        //}

        //if (_isShowHavestItemDescription == true && Input.GetMouseButtonDown(0))
        //{
        //    ShowHavestItem();
        //}
    }

    public void Init(BambooFieldSystem bambooFieldSystem, string growingCropID)
    {
        BFieldSystem = bambooFieldSystem;
        _growingCropID = growingCropID;
    }

    public void StartInteraction()
    {
        //if (_isShowHavestItemDescriptionTrue == false)
        //{
        //    ShowHavestItem();
        //}

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

        ChangeGrowthStageImage(GrowthStage);

        // ��ư ����
        if (!BFieldSystem.HarvestButton.IsSet && GrowthStage == 2)
        {
            BFieldSystem.HarvestButton.IsSet = true;
            //Tween.SpriteRendererAlpha(BFieldSystem.HarvestButton.gameObject, 1, 0.5f, TweenMode.Quadratic);
        }
    }

    /// <summary>
    /// ���� �ܰ迡 ���� �̹��� ���� </summary>
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

    public float IsIncreaseYields(float time)
    {
        if (time > HarvestItem.HarvestTime) // �ð� ����  * 60
        {
            IncreaseYields();
            time -= HarvestItem.HarvestTime;
        }
        return time;
    }

    private void IncreaseYields()
    {
        Yield += HarvestItem.Yield;
        if(Yield > HarvestItem.MaxYield)
        {
            Yield = HarvestItem.MaxYield;
        }
        //���� �ð��� ������ ��ư ���� �� ��Ȯ
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
    /// ���� Ű��� �۹��� ���� ��� </summary>
    private void ShowHavestItem()
    {
        _isShowHavestItemDescription = !_isShowHavestItemDescription;

        if (_isShowHavestItemDescription)
        {
            _isShowHavestItemDescriptionTrue = true;
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
                _isShowHavestItemDescriptionTrue = false;
            });
        }

    }

    /// <summary>
    /// ��Ȯ �ð� ����</summary>
    public TimeSpan SetTimeDifference()
    {
        //_time = 0;
        //_second = 0;
        _timeDifference = TimeSpan.FromSeconds(HarvestItem.HarvestTime * Mathf.CeilToInt(HarvestItem.MaxYield / (float)HarvestItem.Yield)); //  * 60
        SetRemainingTime(_timeDifference);

        return _timeDifference;
    }

    /// <summary>
    /// ��Ȯ���� ���� �ð� ���� </summary>
    public void SetRemainingTime(TimeSpan timeDifference)
    {
        if (timeDifference.Hours == 0)
        {
            if (timeDifference.Minutes == 0)
            {
                _remainingTime = string.Format("{0}s", timeDifference.Seconds);

            }
            else
            {
                _remainingTime = string.Format("{0}m {1}s", timeDifference.Minutes, timeDifference.Seconds);
            }
        }
        else
        {
            _remainingTime = string.Format("{0}h {1}m", timeDifference.Hours, timeDifference.Minutes);
        }
    }

    public void SetRemainingTimeZero()
    {
        _remainingTime = "ä�� ����!";

        // ���� ä�� �ð��� ������ ��츦 ����� �� �� �� ����
        IncreaseYields();
    }

}
