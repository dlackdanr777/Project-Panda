using Muks.DataBind;
using Muks.Tween;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 방 종류
public enum ERoom
{
    Starter,
    Jiji
}

public class FurnitureView : MonoBehaviour
{
    private FurnitureViewModel _furnitureViewModel;
    [SerializeField] private GameObject _slots;

    private ERoom _currnetRoom = ERoom.Starter;
    [SerializeField] private GameObject[] _rooms = new GameObject[System.Enum.GetValues(typeof(ERoom)).Length]; // 방 종류
    public RoomFurniture[] _roomFurnitures = new RoomFurniture[System.Enum.GetValues(typeof(ERoom)).Length]; // 실제 가구 배치

    [SerializeField] private GameObject _leftDoor;
    [SerializeField] private GameObject _rightDoor;
    
    [SerializeField] private Button _exitButton;
    //[SerializeField] private Sprite _exitDoorOpenImage;
    [SerializeField] private GameObject _uiSave;

    [SerializeField] private Sprite _transparentSprite; // 투명 이미지
    [SerializeField] private GameObject _detailView; // 상세설명 창
    [SerializeField] private Button _closeDetailViewButton; // 상세설명 창 닫기 버튼

    [SerializeField] private int _firstToggleIndex;
    [SerializeField] private ToggleGroup _field; //토글 종류

    [SerializeField] private Transform[] _spawnPoint; //spawn할 위치
    [SerializeField] private ScrollRect _scrollrect;


    [SerializeField] private GameObject _furnitureSlot;

    private List<Furniture>[] _lists = new List<Furniture>[System.Enum.GetValues(typeof(EFurnitureViewType)).Length - 1]; //Field 개수만큼 리스트 존재(None 제외) //데이터를 저장할 공간
    private EFurnitureViewType _currentField;
    private int[] _maxCount = new int[System.Enum.GetValues(typeof(EFurnitureViewType)).Length - 1];

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
    }

    private void OnDestroy()
    {
        _furnitureViewModel.FurnitureChanged -= UpdateFurnitureID;
    }

    #region 초기 설정
    /// <summary>
    /// 코스튬 뷰모델과 바인드 </summary>
    private void Bind()
    {
        if (_furnitureViewModel == null)
        {
            _furnitureViewModel = new FurnitureViewModel();
            DatabaseManager.Instance.StartPandaInfo.FurnitureViewModel = _furnitureViewModel;
            _furnitureViewModel.FurnitureChanged += UpdateFurnitureID;
            //_furnitureViewModel.ShowDetailView += (() => Invoke("ShowDetailView", 0.2f));
            _furnitureViewModel.ShowDetailView += (() => _detailView.SetActive(true));
        }

        // 비어있는게 아니라면 불러올 때 원래 가구 미리 배치
        FurnitureModel.FurnitureId[] furnitureRooms = DatabaseManager.Instance.StartPandaInfo.FurnitureRooms;

        for (int i = 0; i < furnitureRooms.Length; i++) // 방 설정
        {
            for (int j = 0; j < furnitureRooms[i].FurnitureIds.Length; j++) // 가구 배치
            {
                if (furnitureRooms[i].FurnitureIds[j] != "")
                {
                    _furnitureViewModel.ChangedFurniture(DatabaseManager.Instance.GetFurnitureItem()[furnitureRooms[i].FurnitureIds[j]], (ERoom)i);
                }
                else
                {
                    _roomFurnitures[i]._furnitures[j].gameObject.SetActive(false);
                }
            }
        }
    }

    private void Init()
    {
        DatabaseManager.Instance.StartPandaInfo.StarterPanda.gameObject.SetActive(false);

        // 문 열기
        Tween.RectTransfromAnchoredPosition(gameObject, new Vector2(0, -650), 1.5f, TweenMode.EaseInOutBack);
        Tween.RectTransfromAnchoredPosition(_leftDoor, new Vector2(0, 0), 1.5f, TweenMode.Quadratic);
        Tween.RectTransfromAnchoredPosition(_rightDoor, new Vector2(620, 0), 1.5f, TweenMode.Quadratic);

        CreateSlots(); //slot 생성

        //버튼 리스너
        DataBind.SetButtonValue("LeftRoomButton", () => ChangeRoom(true));
        DataBind.SetButtonValue("RightRoomButton", () => ChangeRoom(false));

        foreach (Toggle toggle in _field.GetComponentsInChildren<Toggle>())
        {
            toggle.onValueChanged.AddListener((bool isOn) => OnClickedFieldButton(isOn, toggle.transform));
        }

        if (_detailView != null)
        {
            _closeDetailViewButton.onClick.AddListener(OnCloseDetailView);

        }

        _exitButton.onClick.AddListener(OnExitButtonClicked);

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

    //list마다 해당 spawn 위치에 Instantiate하는 함수
    private void CreateSlots()
    {
        for (int i = 0; i < _maxCount.Length; i++)
        {
            for (int j = 0; j < _maxCount[i]; j++)
            {
                GameObject slot;
                if(j == 0)
                {
                    slot = _spawnPoint[i].GetChild(0).gameObject;
                    //slot.transform.GetChild(0).gameObject.SetActive(false);
                }
                else
                {
                    slot = Instantiate(_furnitureSlot, _spawnPoint[i]);
                }
                int _slotIndex = j;
                slot.GetComponent<Button>().onClick.AddListener(() => FurnitureImageBtnClick(_slotIndex));
            }
        }
    }
    #endregion

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

    
    /// <summary>
    /// 코스튬 선택 버튼 </summary>
    private void FurnitureImageBtnClick(int index)
    {
        if(index < 1)
        {
            _furnitureViewModel.RemoveFurniture(_currentField, _currnetRoom);
            return;
        }

        // 현재 필드의 index를 통해 가구 찾아 변경
        _furnitureViewModel.ChangedFurniture(DatabaseManager.Instance.GetFurnitureItem()[_lists[(int)_currentField][index].Id], _currnetRoom);
    }

    /// <summary>
    /// 입고있는 코스튬 ID가 변경될 경우 실행 </summary>
    private void UpdateFurnitureID(Furniture furnitureData)
    {
        if (furnitureData != null)
        {
            _roomFurnitures[(int)_currnetRoom]._furnitures[(int)furnitureData.Type].gameObject.SetActive(true);
            _roomFurnitures[(int)_currnetRoom]._furnitures[(int)furnitureData.Type].sprite = furnitureData.Image;
        }
        else if (_currentField == EFurnitureViewType.WallPaper || _currentField == EFurnitureViewType.Floor)
        {
            _roomFurnitures[(int)_currnetRoom]._furnitures[(int)_currentField].gameObject.SetActive(false);
        }
        else
        { 
            int field = 2 + ((int)_currentField - 2) * 2;
            _roomFurnitures[(int)_currnetRoom]._furnitures[field].gameObject.SetActive(false);
            _roomFurnitures[(int)_currnetRoom]._furnitures[field + 1].gameObject.SetActive(false);
        }

    }

    /// <summary>
    /// 방 변경 시 호출 </summary>
    private void ChangeRoom(bool isLeft)
    {
        Tween.RectTransfromAnchoredPosition(_leftDoor, new Vector2(547, 0), 1.5f, TweenMode.Constant);
        Tween.RectTransfromAnchoredPosition(_rightDoor, new Vector2(80, 0), 1.5f, TweenMode.Constant, () =>
        {
            _rooms[(int)_currnetRoom].gameObject.SetActive(false);
            int eroomLength = System.Enum.GetValues(typeof(ERoom)).Length;
            if (isLeft)
            {
                _currnetRoom = (ERoom)(((int)_currnetRoom + eroomLength - 1) % eroomLength);
            }
            else
            {
                _currnetRoom = (ERoom)(((int)_currnetRoom + 1) % eroomLength);
            }
            _rooms[(int)_currnetRoom].gameObject.SetActive(true);
            Tween.RectTransfromAnchoredPosition(_leftDoor, new Vector2(547, 0), 0.5f, TweenMode.Constant);
            Tween.RectTransfromAnchoredPosition(_rightDoor, new Vector2(80, 0), 0.5f, TweenMode.Constant, () =>
            {
                Tween.RectTransfromAnchoredPosition(_leftDoor, new Vector2(0, 0), 1.5f, TweenMode.Quadratic);
                Tween.RectTransfromAnchoredPosition(_rightDoor, new Vector2(620, 0), 1.5f, TweenMode.Quadratic);
            });     
        });
    }

    private void OnExitButtonClicked()
    {
        //_exitButton.GetComponent<Image>().sprite = _exitDoorOpenImage;
        _furnitureViewModel.ExitFurniture();
    }

    private void OnCloseDetailView()
    {
        DataBind.SetSpriteValue("FurnitureDetailImage", _transparentSprite);
        _detailView.SetActive(false);
    }

    [System.Serializable]
    public class RoomFurniture
    {
        public Image[] _furnitures;

        public RoomFurniture()
        {
            _furnitures = new Image[System.Enum.GetValues(typeof(FurnitureType)).Length];
        }
    }

}