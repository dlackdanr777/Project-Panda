using Muks.DataBind;
using Muks.Tween;
using System;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FurnitureView : MonoBehaviour
{
    private FurnitureViewModel _furnitureViewModel;
    [SerializeField] private GameObject _slots;
    [SerializeField] private GameObject _wallPaper;
    [SerializeField] private GameObject _leftDoor;
    [SerializeField] private GameObject _rightDoor;
    [SerializeField] private Button _exitButton;
    [SerializeField] private GameObject _uiSave;
    [SerializeField] private GameObject _detailView; // 상세설명 창
    [SerializeField] private Button _closeButton; // 상세설명 창 닫기 버튼

    // 우선 벽지만 넣음
    [SerializeField] private GameObject _wallPaperImages;

    [SerializeField] private GameObject _furnitureSlot;
    private GameObject[] _furnitureImagePfs;
    private Button[] _furnitureImageBtn;


    private void Start()
    {
        Bind();
        Init();
    }

    private void OnDestroy()
    {
        _furnitureViewModel.FurnitureChanged -= UpdateFurnitureID;
    }

    private void Init()
    {
        DatabaseManager.Instance.StartPandaInfo.StarterPanda.gameObject.SetActive(false);

        // 문 열기
        Tween.RectTransfromAnchoredPosition(gameObject, new Vector2(0, -650), 1.5f, TweenMode.EaseInOutBack);
        //Tween.RectTransfromAnchoredPosition(_leftDoor, new Vector2(390, 0), 1f, TweenMode.Quadratic);
        //Tween.RectTransfromAnchoredPosition(_rightDoor, new Vector2(10, 0), 1f, TweenMode.Quadratic);

        // 가구 개수 가져오기 - 가구로 변경
        _furnitureImagePfs = new GameObject[DatabaseManager.Instance.StartPandaInfo.CostumeInventory.Count + 1];
        _furnitureImageBtn = new Button[DatabaseManager.Instance.StartPandaInfo.CostumeInventory.Count + 1];

        // 개수 + 1만큼 코스튬 프리팹 생성(0번째는 None)
        for (int i = 0; i < DatabaseManager.Instance.StartPandaInfo.CostumeInventory.Count + 1; i++) // 가구 인벤토리 만들어야 함
        {
            int index = i;
            _furnitureImagePfs[i] = Instantiate(_furnitureSlot, _wallPaperImages.transform).transform.GetChild(0).gameObject; // 일단 다 벽지에 넣음 - 딕셔너리 만들어서 구분.. A, B, C..
            if (i == 0)
            {
                _furnitureImagePfs[i].GetComponent<Image>().color = new Color(0, 0, 0, 0);
            }
            else
            {
                _furnitureImagePfs[i].GetComponent<Image>().sprite = DatabaseManager.Instance.StartPandaInfo.CostumeInventory[i-1].Image;
            }

            _furnitureImageBtn[i] = _furnitureImagePfs[i].GetComponent<Button>();
            if (_furnitureImageBtn[i] != null)
            {
                _furnitureImageBtn[i].onClick.AddListener(() => this.FurnitureImageBtnClick(index));
                Debug.Log(i);
            }
        }

        _exitButton.onClick.AddListener(OnExitButtonClicked);
        DataBind.SetButtonValue("FurnitureDetailViewCloseButton", OnCloseButtonClicked);
        //DataBind.SetButtonValue("ExitCostumeButton", OnExitButtonClicked); // 수정

    }

    /// <summary>
    /// 코스튬 뷰모델과 바인드 </summary>
    private void Bind()
    {
        if (_furnitureViewModel == null)
        {
            _furnitureViewModel = new FurnitureViewModel();
            DatabaseManager.Instance.StartPandaInfo.FurnitureViewModel = _furnitureViewModel;
            _furnitureViewModel.FurnitureChanged += UpdateFurnitureID;
            _furnitureViewModel.ShowDetailView += ShowDetailView;
        }
        if (DatabaseManager.Instance.StartPandaInfo.WearingHeadCostumeID != "")
        {
            _furnitureViewModel.ChangedFurniture(CostumeManager.Instance.GetCostumeData(DatabaseManager.Instance.StartPandaInfo.WearingHeadCostumeID));
        }
    }

    /// <summary>
    /// 코스튬 선택 버튼 </summary>
    private void FurnitureImageBtnClick(int index)
    {
        //if (CostumeManager.Instance.GetCostumeData(index).IsMine)
        //{
        //    _costumeViewModel.WearingCostume(CostumeManager.Instance.GetCostumeData(index));
        //}

        //_costumeViewModel.WearingCostume(CostumeManager.Instance.GetCostumeData(index));
        if(index == 0)
        {
            _furnitureViewModel.RemoveFurniture();
            return;
        }
        _furnitureViewModel.ChangedFurniture(DatabaseManager.Instance.StartPandaInfo.CostumeInventory[index - 1]);
    }

    /// <summary>
    /// 입고있는 코스튬 ID가 변경될 경우 실행 </summary>
    private void UpdateFurnitureID(CostumeData costumeData)
    {
        // 여러 부위가 있을 경우
        //_panda.transform.GetChild((int)costumeData.BodyParts).gameObject.SetActive(true);
        //_panda.transform.GetChild((int)costumeData.BodyParts).GetComponent<Image>().sprite = costumeData.Image;

        if (costumeData != null)
        {
            _wallPaper.SetActive(true);
            _wallPaper.GetComponent<Image>().sprite = costumeData.Image;
        }
        else
        {
            _wallPaper.SetActive(false);
        }

    }

    private void OnExitButtonClicked()
    {
        _furnitureViewModel.ExitFurniture();
    }

    private void ShowDetailView()
    {
        _detailView.SetActive(true);
    }

    private void OnCloseButtonClicked()
    {
        _detailView.SetActive(false);
    }

}