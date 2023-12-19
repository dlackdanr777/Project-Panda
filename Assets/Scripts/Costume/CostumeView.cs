using Muks.DataBind;
using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CostumeView : MonoBehaviour, IInteraction
{
    private CostumeViewModel _costumeViewModel;
    [SerializeField] private GameObject _slots;
    [SerializeField] private GameObject _pandaHead;

    // 우선 머리 코스튬만 넣음
    [SerializeField] private GameObject _headCostumeImages; 
    [SerializeField] private GameObject _costumeImagePf;
    private GameObject[] _costumeImagePfs;
    private Button[] _costumeImageBtn;

    private void Bind()
    {
        _costumeViewModel = new CostumeViewModel();
        _costumeViewModel.CostumeChanged += UpdateCostumeID;
        
    }


    private void Start()
    {
        
        Init();
    }

    private void Init()
    {
        _costumeImagePfs = new GameObject[CostumeManager.Instance.CostumeDic.Count];
        _costumeImageBtn = new Button[CostumeManager.Instance.CostumeDic.Count];
        // 개수만큼 코스튬 프리팹 생성
        for (int i = 0; i < CostumeManager.Instance.CostumeDic.Count; i++)
        {
            _costumeImagePfs[i] = Instantiate(_costumeImagePf, _headCostumeImages.transform);
            _costumeImagePfs[i].GetComponent<Image>().sprite = CostumeManager.Instance.CostumeDic[i].Image;
            _costumeImageBtn[i] = _costumeImagePfs[i].GetComponent<Button>();
            if (_costumeImageBtn[i] != null)
            {
                _costumeImageBtn[i].onClick.AddListener(CostumeImageBtnClick);
            }
        }
    }

    private void CostumeImageBtnClick()
    {
        throw new NotImplementedException();
    }

    private void UpdateCostumeID(CostumeData costumeData)
    {
        //_panda.transform.GetChild((int)costumeData.BodyParts).gameObject.SetActive(true);
        //_panda.transform.GetChild((int)costumeData.BodyParts).GetComponent<Image>().sprite = costumeData.Image;
        _pandaHead.SetActive(true);
        _pandaHead.GetComponent<Image>().sprite = costumeData.Image;
    
    }

    public void StartInteraction()
    {
        // 현재 눌린 코스튬 아이디 알아야 함
        _costumeViewModel.WearingCostume();
    }
    public void UpdateInteraction()
    {

    }
    public void ExitInteraction()
    {

    }

}
