using Muks.DataBind;
using Muks.Tween;
using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CostumeView : MonoBehaviour
{
    private CostumeViewModel _costumeViewModel;
    [SerializeField] private GameObject _slots;
    [SerializeField] private GameObject _pandaHead;
    [SerializeField] private GameObject _leftCurtain;
    [SerializeField] private GameObject _rightCurtain;

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
        Bind();
    }

    private void Init()
    {
        Tween.RectTransfromAnchoredPosition(gameObject, new Vector2(0, -650), 1.5f, TweenMode.EaseInOutBack);
        Tween.RectTransfromAnchoredPosition(_leftCurtain, new Vector2(-600, 0), 1.5f, TweenMode.Quadratic);
        Tween.RectTransfromAnchoredPosition(_rightCurtain, new Vector2(600, 0), 1.5f, TweenMode.Quadratic);

        _costumeImagePfs = new GameObject[CostumeManager.Instance.CostumeDic.Count];
        _costumeImageBtn = new Button[CostumeManager.Instance.CostumeDic.Count];

        // 개수만큼 코스튬 프리팹 생성
        for (int i = 0; i < CostumeManager.Instance.CostumeDic.Count; i++)
        {
            int index  = i;
            _costumeImagePfs[i] = Instantiate(_costumeImagePf, _headCostumeImages.transform);
            _costumeImagePfs[i].GetComponent<Image>().sprite = CostumeManager.Instance.CostumeDic[i].Image;
            _costumeImageBtn[i] = _costumeImagePfs[i].GetComponent<Button>();
            if (_costumeImageBtn[i] != null)
            {
                _costumeImageBtn[i].onClick.AddListener(() => this.CostumeImageBtnClick(index));
                Debug.Log(i);
            }
        }
    }

    private void CostumeImageBtnClick(int index)
    {
        Debug.Log(index + "버튼 클릭");
        _costumeViewModel.WearingCostume(CostumeManager.Instance.GetCostumeData(index));
    }

    private void UpdateCostumeID(CostumeData costumeData)
    {
        //_panda.transform.GetChild((int)costumeData.BodyParts).gameObject.SetActive(true);
        //_panda.transform.GetChild((int)costumeData.BodyParts).GetComponent<Image>().sprite = costumeData.Image;
        if(costumeData != null)
        {
            _pandaHead.SetActive(true);
            _pandaHead.GetComponent<Image>().sprite = costumeData.Image;
        }
        else
        {
            _pandaHead.SetActive(false);
        }
    
    }
}
