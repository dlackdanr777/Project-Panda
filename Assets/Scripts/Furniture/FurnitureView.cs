using Muks.DataBind;
using Muks.Tween;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FurnitureView : MonoBehaviour
{
    private FurnitureViewModel _furnitureViewModel;
    [SerializeField] private GameObject _slots;
    [SerializeField] private Image[] _furnitures; // 실제 가구 배치
    [SerializeField] private GameObject _leftDoor;
    [SerializeField] private GameObject _rightDoor;
    
    [SerializeField] private Button _exitButton;
    [SerializeField] private GameObject _uiSave;

    [SerializeField] private GameObject _detailView; // 상세설명 창
    [SerializeField] private Button _closeDetailViewButton; // 상세설명 창 닫기 버튼

    [SerializeField] private int _firstToggleIndex;
    [SerializeField] protected ToggleGroup _field; //토글 종류

    [SerializeField] protected Transform[] _spawnPoint; //spawn할 위치
    [SerializeField] private ScrollRect _scrollrect;


    [SerializeField] private GameObject _furnitureSlot;
    //private GameObject[] _furnitureImagePfs;
    //private Button[] _furnitureImageBtn;


    private int _currentItemIndex;

    protected List<Furniture>[] _lists = new List<Furniture>[System.Enum.GetValues(typeof(EFurnitureViewType)).Length - 1]; //Field 개수만큼 리스트 존재(None 제외) //데이터를 저장할 공간
    protected EFurnitureViewType _currentField;
    protected int[] _maxCount = new int[System.Enum.GetValues(typeof(EFurnitureViewType)).Length - 1];

    private void Start()
    {
        Bind();

        List <Furniture> furnitureInventory = DatabaseManager.Instance.StartPandaInfo.FurnitureInventory;
        for (int i = 0; i < System.Enum.GetValues(typeof(EFurnitureViewType)).Length - 1; i++)
        {
            if (furnitureInventory != null)
            {
                _maxCount[i] = 1;
                _lists[i] = new List<Furniture>();
                _lists[i].Add(null);
                for (int j = 0; j < furnitureInventory.Count; j++)
                {
                    if (furnitureInventory[j].ViewType == (EFurnitureViewType)i)
                    {
                        _maxCount[i]++;
                        _lists[i].Add(furnitureInventory[j]);
                    }
                }
            }
        }

        Init();

        //_furnitures = _furniture.GetComponentsInChildren<Image>();
        Toggle firstToggle = _field.transform.GetChild(_firstToggleIndex).GetComponent<Toggle>(); //다시 들어가도 첫번째가 활성화되도록
        if (firstToggle != null)
        {
            firstToggle.isOn = true;
        }
        GetCurrentField();
        if ((int)(object)_currentField != -1)
        {
            UpdateListSlots();
        }
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

        CreateSlots(); //slot 생성

        //버튼 리스너
        foreach (Toggle toggle in _field.GetComponentsInChildren<Toggle>())
        {
            toggle.onValueChanged.AddListener((bool isOn) => OnClickedFieldButton(isOn, toggle.transform));
        }

        if (_detailView != null)
        {
            _closeDetailViewButton.onClick.AddListener(() => _detailView.SetActive(false));

        }
    }

    //list마다 해당 spawn 위치에 Instantiate하는 함수
    private void CreateSlots()
    {
        for (int i = 0; i < _maxCount.Length; i++)
        {
            for (int j = 0; j < _maxCount[i]; j++)
            {
                GameObject slot = Instantiate(_furnitureSlot, _spawnPoint[i]);
                if(j == 0)
                {
                    slot.transform.GetChild(0).gameObject.SetActive(false);
                }
                int _slotIndex = j;
                slot.GetComponent<Button>().onClick.AddListener(() => FurnitureImageBtnClick(_slotIndex));
            }
        }
    }

    private void GetCurrentField()
    {
        Toggle selectedField = GetOnToggle(); //선택된 토글
        _currentField = GetFieldByTransform(selectedField);
        SetActiveContent();
    }

    //spawnPosition 활성화 함수
    private void SetActiveContent()
    {
        for (int i = 0; i < _spawnPoint.Length; i++) //content false로 변경
        {
            _spawnPoint[i].gameObject.SetActive(false);
        }
        if ((int)(object)_currentField >= 0)
        {
            GameObject activeContent = _spawnPoint[(int)(object)_currentField].gameObject;
            activeContent.SetActive(true);//현재 토글의 content를 setactive 
            _scrollrect.content = activeContent.GetComponent<RectTransform>(); //scrollview 변경 

        }
    }
    private Toggle GetOnToggle()
    {
        foreach (Toggle toggle in _field.transform.GetComponentsInChildren<Toggle>())
        {
            if (toggle.isOn == true)
            {
                return toggle;
            }
        }
        return null;
    }

    private void OnClickedFieldButton(bool isOn, Transform toggle)
    {
        GetCurrentField();
        if ((int)(object)_currentField != -1)
        {
            UpdateListSlots();
        }

    }
    private void UpdateListSlots()
    {
        if (_lists[(int)_currentField] != null)
        {
            for (int j = 1; j < _maxCount[(int)_currentField]; j++) //현재 player의 인벤토리에 저장된 아이템 갯수
            {
                if (j < _lists[(int)_currentField].Count)
                {
                    Transform prefab = _spawnPoint[(int)_currentField].GetChild(j).GetChild(0);
                    prefab.gameObject.SetActive(true); //구조 변경 => Getchild만 켜지도록
                    prefab.GetComponent<Image>().sprite = _lists[(int)_currentField][j].Image;
                }
                else
                {
                    _spawnPoint[(int)_currentField].GetChild(j).GetChild(0).gameObject.SetActive(false);
                }
            }
        }
    }

    //Transform으로 Field값 찾기
    private EFurnitureViewType GetFieldByTransform(Toggle toggle)
    {
        for (int i = 0; i < _field.transform.childCount; i++)
        {
            if (_field.transform.GetChild(i).GetComponent<Toggle>() == toggle)
            {
                return (EFurnitureViewType)(object)i;
            }
        }
        return (EFurnitureViewType)(object)-1;
    }

    //private void Init()
    //{
    //    DatabaseManager.Instance.StartPandaInfo.StarterPanda.gameObject.SetActive(false);
    //    _contents = _contentsParent.GetComponentsInChildren<Transform>().Select(t => t.gameObject).ToArray();
    //    Debug.Log("_contents" + _contents);

    //    // 문 열기
    //    Tween.RectTransfromAnchoredPosition(gameObject, new Vector2(0, -650), 1.5f, TweenMode.EaseInOutBack);
    //    //Tween.RectTransfromAnchoredPosition(_leftDoor, new Vector2(390, 0), 1f, TweenMode.Quadratic);
    //    //Tween.RectTransfromAnchoredPosition(_rightDoor, new Vector2(10, 0), 1f, TweenMode.Quadratic);

    //    // 가구 개수 가져오기 - 가구로 변경
    //    _furnitureImagePfs = new GameObject[DatabaseManager.Instance.StartPandaInfo.FurnitureInventory.Count + _contents.Length];
    //    _furnitureImageBtn = new Button[DatabaseManager.Instance.StartPandaInfo.FurnitureInventory.Count + _contents.Length];

    //    // 개수 + 메뉴 개수만큼 코스튬 프리팹 생성(메뉴의 0번째는 비워두어야 하므로)
    //    for (int i = 0; i < DatabaseManager.Instance.StartPandaInfo.FurnitureInventory.Count + _contents.Length; i++) // 가구 인벤토리 만들어야 함
    //    {
    //        int index = i;
    //        Debug.Log("(int)DatabaseManager.Instance.StartPandaInfo.FurnitureInventory[i].ViewType" + (int)DatabaseManager.Instance.StartPandaInfo.FurnitureInventory[i].ViewType);
    //        _furnitureImagePfs[i] = Instantiate(_furnitureSlot, _contents[(int)DatabaseManager.Instance.StartPandaInfo.FurnitureInventory[i].ViewType].transform).transform.GetChild(0).gameObject; // 일단 다 벽지에 넣음 - 딕셔너리 만들어서 구분.. A, B, C..
    //        if (i < _contents.Length) // 처음 5개는 빈칸
    //        {
    //            _furnitureImagePfs[i].GetComponent<Image>().color = new Color(0, 0, 0, 0);
    //        }
    //        else
    //        {
    //            _furnitureImagePfs[i].GetComponent<Image>().sprite = DatabaseManager.Instance.StartPandaInfo.FurnitureInventory[i-1].Image;
    //        }

    //        _furnitureImageBtn[i] = _furnitureImagePfs[i].GetComponent<Button>();
    //        if (_furnitureImageBtn[i] != null)
    //        {
    //            _furnitureImageBtn[i].onClick.AddListener(() => this.FurnitureImageBtnClick(index));
    //            Debug.Log(i);
    //        }
    //    }

    //    _exitButton.onClick.AddListener(OnExitButtonClicked);
    //    DataBind.SetButtonValue("FurnitureDetailViewCloseButton", OnCloseButtonClicked);
    //    //DataBind.SetButtonValue("ExitCostumeButton", OnExitButtonClicked); // 수정

    //}

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

        // 비어있는게 아니라면 불러올 때 원래 가구 미리 배치
        if (DatabaseManager.Instance.StartPandaInfo.WearingHeadCostumeID != "")
        {
            _furnitureViewModel.ChangedFurniture(DatabaseManager.Instance.GetFurnitureItem()[DatabaseManager.Instance.StartPandaInfo.WallPaperID]);
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

        if(index < 1)
        {
            Debug.Log("가구 제거" + index);
            _furnitureViewModel.RemoveFurniture(_currentField);
            return;
        }

        // 현재 필드의 index를 통해 가구 찾아 변경
        _furnitureViewModel.ChangedFurniture(DatabaseManager.Instance.GetFurnitureItem()[_lists[(int)_currentField][index].Id]);
        //_furnitureViewModel.ChangedFurniture(DatabaseManager.Instance.StartPandaInfo.FurnitureInventory[index - 1]);
    }

    /// <summary>
    /// 입고있는 코스튬 ID가 변경될 경우 실행 </summary>
    private void UpdateFurnitureID(Furniture furnitureData)
    {
        if (furnitureData != null)
        {
            _furnitures[(int)furnitureData.Type].gameObject.SetActive(true);
            _furnitures[(int)furnitureData.Type].sprite = furnitureData.Image;
        }
        else if (_currentField == EFurnitureViewType.WallPaper || _currentField == EFurnitureViewType.Floor)
        {
            _furnitures[(int)_currentField].gameObject.SetActive(false);
        }
        else
        { 
            int field = 2 + ((int)_currentField - 2) * 2;
            _furnitures[field].gameObject.SetActive(false);
            _furnitures[field + 1].gameObject.SetActive(false);
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

}