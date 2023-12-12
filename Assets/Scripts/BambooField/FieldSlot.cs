using UnityEngine;
using Muks.Tween;

public class FieldSlot : MonoBehaviour, IInteraction
{
    /// <summary> ���� �ܰ� </summary>
    public int GrowthStage;

    public HarvestItem HarvestItem;
    [SerializeField] private HarvestButton _harvestButton;
    [SerializeField] private int _growingCropID;
    [SerializeField] private GameObject _growingCropImage;
    private float _time;
    public int Yield;

    private void Start()
    {
        // �켱 �׼�ID�� ����
        _growingCropID = 0;
        Debug.Log("����");
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
        if (!_harvestButton.IsSet)
        {
            _harvestButton.IsSet = true;
            Tween.SpriteRendererAlpha(_harvestButton.gameObject, 1, 0.5f, TweenMode.Quadratic);
        }
    }

    /// <summary>
    /// ���� �ܰ迡 ���� �̹��� ���� </summary>
    public void ChangeGrowthStageImage(int growthStage)
    {
        _growingCropImage.GetComponent<SpriteRenderer>().sprite = HarvestItem.Image[growthStage];
    }

    private void IsIncreaseYields()
    {
        if(_time > HarvestItem.HarvestTime *2)// ���߿� ���� *60
        {
            IncreaseYields();
            _time = 0;
        }
    }

    private void IncreaseYields()
    {
        Yield++; //Yield += HarvestItem.Yield; // �ٲٱ�
        if(Yield > HarvestItem.MaxYield)
        {
            Yield = HarvestItem.MaxYield;
        }

        Debug.Log("��Ȯ��: " + Yield);
        //���� �ð��� ������ ��ư ���� �� ��Ȯ
        if (Yield == 10) //HarvestItem.Yield * 10 ����
        {
            GrowingCrops(2);
        }
        else if (Yield == 5)
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
