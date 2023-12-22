using Muks.DataBind;
using Muks.Tween;
using System;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CostumeView : MonoBehaviour
{
    private CostumeViewModel _costumeViewModel;
    [SerializeField] private GameObject _slots;
    [SerializeField] private GameObject _pandaHead;
    [SerializeField] private GameObject _leftCurtain;
    [SerializeField] private GameObject _rightCurtain;
    [SerializeField] private Button _exitButton;
    [SerializeField] private GameObject _uiSave;

    // 우선 머리 코스튬만 넣음
    [SerializeField] private GameObject _headCostumeImages;

    [SerializeField] private GameObject _costumeImagePf;
    private GameObject[] _costumeImagePfs;
    private Button[] _costumeImageBtn;


    private void Start()
    {
        Bind();
        Init();
    }

    private void OnDestroy()
    {
        _costumeViewModel.CostumeChanged -= UpdateCostumeID;
    }

    private void Init()
    {
        // 커튼 열기
        Tween.RectTransfromAnchoredPosition(gameObject, new Vector2(0, -650), 1.5f, TweenMode.EaseInOutBack);
        Tween.RectTransfromAnchoredPosition(_leftCurtain, new Vector2(-600, 0), 1.5f, TweenMode.Quadratic);
        Tween.RectTransfromAnchoredPosition(_rightCurtain, new Vector2(600, 0), 1.5f, TweenMode.Quadratic);

        _costumeImagePfs = new GameObject[CostumeManager.Instance.CostumeDic.Count];
        _costumeImageBtn = new Button[CostumeManager.Instance.CostumeDic.Count];

        // 개수만큼 코스튬 프리팹 생성
        for (int i = 0; i < CostumeManager.Instance.CostumeDic.Count; i++)
        {
            int index = i;
            _costumeImagePfs[i] = Instantiate(_costumeImagePf, _headCostumeImages.transform);
            _costumeImagePfs[i].GetComponent<Image>().sprite = CostumeManager.Instance.CostumeDic[i].Image;
            _costumeImageBtn[i] = _costumeImagePfs[i].GetComponent<Button>();
            if (_costumeImageBtn[i] != null)
            {
                _costumeImageBtn[i].onClick.AddListener(() => this.CostumeImageBtnClick(index));
                Debug.Log(i);
            }
        }

        //Bind();
        _exitButton.onClick.AddListener(OnExitButtonClicked);
        //DataBind.SetButtonValue("ExitCostumeButton", OnExitButtonClicked); // 수정

    }

    /// <summary>
    /// 코스튬 뷰모델과 바인드 </summary>
    private void Bind()
    {
        if(_costumeViewModel == null)
        {
            _costumeViewModel = new CostumeViewModel();
            DatabaseManager.Instance.StartPandaInfo.CostumeViewModel = _costumeViewModel;   
            _costumeViewModel.CostumeChanged += UpdateCostumeID;
        }
        if(DatabaseManager.Instance.StartPandaInfo.WearingHeadCostumeID != -1)
        {
            _costumeViewModel.WearingCostume(CostumeManager.Instance.GetCostumeData(DatabaseManager.Instance.StartPandaInfo.WearingHeadCostumeID));
        }
    }

    /// <summary>
    /// 코스튬 선택 버튼 </summary>
    private void CostumeImageBtnClick(int index)
    {
        _costumeViewModel.WearingCostume(CostumeManager.Instance.GetCostumeData(index));
    }

    /// <summary>
    /// 입고있는 코스튬 ID가 변경될 경우 실행 </summary>
    private void UpdateCostumeID(CostumeData costumeData)
    {
        // 여러 부위가 있을 경우
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

    private void OnExitButtonClicked()
    {
        Debug.Log("나가기 버튼 클릭");
        _costumeViewModel.ExitCostume();

        //// 저장
        //_costumeViewModel.SaveCostume();
        //SceneManager.LoadScene("CostumeTestMainScene"); // 나중에 메인 씬으로 변경
    }
}
