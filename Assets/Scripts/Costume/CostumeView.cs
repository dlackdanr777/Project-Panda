using Muks.DataBind;
using Muks.Tween;
using System;
using System.Linq;
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
        DatabaseManager.Instance.StartPandaInfo.StarterPanda.gameObject.SetActive(false);

        // 커튼 열기
        Tween.RectTransfromAnchoredPosition(gameObject, new Vector2(0, -650), 1.5f, TweenMode.EaseInOutBack);
        Tween.RectTransfromAnchoredPosition(_leftCurtain, new Vector2(400, 0), 1f, TweenMode.Quadratic);
        Tween.RectTransfromAnchoredPosition(_rightCurtain, new Vector2(0, 0), 1f, TweenMode.Quadratic);

        _costumeImagePfs = new GameObject[DatabaseManager.Instance.StartPandaInfo.CostumeInventory.Count];
        _costumeImageBtn = new Button[DatabaseManager.Instance.StartPandaInfo.CostumeInventory.Count];

        // 개수만큼 코스튬 프리팹 생성
        for (int i = 0; i < DatabaseManager.Instance.StartPandaInfo.CostumeInventory.Count; i++)
        {
            int index = i;
            _costumeImagePfs[i] = Instantiate(_costumeImagePf, _headCostumeImages.transform);
            _costumeImagePfs[i].GetComponent<Image>().sprite = DatabaseManager.Instance.StartPandaInfo.CostumeInventory[i].Image;

            //// 없는 코스튬은 어둡게 표시
            //if (!CostumeManager.Instance.CostumeDic[i].IsMine)
            //{
            //    _costumeImagePfs[i].GetComponent<Image>().color = new Color(58/255f, 58/255f, 58/255f, 1);
            //}

            _costumeImageBtn[i] = _costumeImagePfs[i].GetComponent<Button>();
            if (_costumeImageBtn[i] != null)
            {
                _costumeImageBtn[i].onClick.AddListener(() => this.CostumeImageBtnClick(index));
                Debug.Log(i);
            }
        }

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
        if(DatabaseManager.Instance.StartPandaInfo.WearingHeadCostumeID != "")
        {
            _costumeViewModel.WearingCostume(CostumeManager.Instance.GetCostumeData(DatabaseManager.Instance.StartPandaInfo.WearingHeadCostumeID));
        }
    }

    /// <summary>
    /// 코스튬 선택 버튼 </summary>
    private void CostumeImageBtnClick(int index)
    {
        //if (CostumeManager.Instance.GetCostumeData(index).IsMine)
        //{
        //    _costumeViewModel.WearingCostume(CostumeManager.Instance.GetCostumeData(index));
        //}

        //_costumeViewModel.WearingCostume(CostumeManager.Instance.GetCostumeData(index));
        _costumeViewModel.WearingCostume(DatabaseManager.Instance.StartPandaInfo.CostumeInventory[index]);
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
        _costumeViewModel.ExitCostume();
    }
}
