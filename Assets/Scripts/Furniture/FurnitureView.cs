using BT;
using Muks.DataBind;
using Muks.Tween;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// �� ����
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
    [SerializeField] private GameObject[] _rooms = new GameObject[System.Enum.GetValues(typeof(ERoom)).Length]; // �� ����
    public RoomFurniture[] _roomFurnitures = new RoomFurniture[System.Enum.GetValues(typeof(ERoom)).Length]; // ���� ���� ��ġ

    [SerializeField] private TweenMode _doorTweenMode;
    [SerializeField] private Image _leftDoor;
    [SerializeField] private Image _rightDoor;
    
    [SerializeField] private Button _exitButton;
    //[SerializeField] private Sprite _exitDoorOpenImage;
    [SerializeField] private GameObject _uiSave;

    [SerializeField] private Sprite _transparentSprite; // ���� �̹���
    [SerializeField] private GameObject _detailView; // �󼼼��� â
    [SerializeField] private Button _closeDetailViewButton; // �󼼼��� â �ݱ� ��ư

    [SerializeField] private int _firstToggleIndex;
    [SerializeField] private ToggleGroup _field; //��� ����

    [SerializeField] private Transform[] _spawnPoint; //spawn�� ��ġ
    [SerializeField] private ScrollRect _scrollrect;

    [SerializeField] private GameObject _furnitureSlot;

    [SerializeField] private GameObject _dontTouchArea; //�ִϸ��̼� �� ��ư Ŭ���� �������� Obj

    private List<Furniture>[] _lists = new List<Furniture>[System.Enum.GetValues(typeof(EFurnitureViewType)).Length - 1]; //Field ������ŭ ����Ʈ ����(None ����) //�����͸� ������ ����
    private EFurnitureViewType _currentField;
    private int[] _maxCount = new int[System.Enum.GetValues(typeof(EFurnitureViewType)).Length - 1];

    //�� �ִϸ��̼ǿ� �ʿ��� Pos��
    private Vector3 _leftDoorTmpPos;
    private Vector3 _leftDoorTargetPos => _leftDoorTmpPos + new Vector3(-1000, 0, 0);
    private Vector3 _rightDoorTmpPos;
    private Vector3 _rightDoorTargetPos => _rightDoorTmpPos + new Vector3(1000, 0, 0);

    private void Start()
    {
        Bind();

        // ���� �κ��丮 ���� ����Ʈ�� ����
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

    #region �ʱ� ����
    /// <summary>
    /// �ڽ�Ƭ ��𵨰� ���ε� </summary>
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

        // ����ִ°� �ƴ϶�� �ҷ��� �� ���� ���� �̸� ��ġ
        FurnitureModel.FurnitureId[] furnitureRooms = DatabaseManager.Instance.StartPandaInfo.FurnitureRooms;

        for (int i = 0; i < furnitureRooms.Length; i++) // �� ����
        {
            for (int j = 0; j < furnitureRooms[i].FurnitureIds.Length; j++) // ���� ��ġ
            {
                Debug.Log("i: " + i + "j: " + j);
                if (!string.IsNullOrEmpty(furnitureRooms[i].FurnitureIds[j])) // ���ڿ��� null�̰ų� �� ���ڿ����� Ȯ��
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
        StarterPanda.Instance.gameObject.SetActive(false);
        _detailView.SetActive(false);
        _dontTouchArea.SetActive(true);
        // �� ����
        Tween.RectTransfromAnchoredPosition(gameObject, new Vector2(0, 300), 1f, _doorTweenMode);

        _leftDoorTmpPos = _leftDoor.rectTransform.anchoredPosition;
        _rightDoorTmpPos = _rightDoor.rectTransform.anchoredPosition;

        OpenDoor(1.5f);

        CreateSlots(); //slot ����
        //BindRoomFurniture();

        //��ư ������
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

        Toggle firstToggle = _field.transform.GetChild(_firstToggleIndex).GetComponent<Toggle>(); //�ٽ� ���� ù��°�� Ȱ��ȭ�ǵ���
        if (firstToggle != null)
        {
            firstToggle.isOn = true;
        }
        GetCurrentField();
        if ((int)(object)_currentField != -1)
        {
            UpdateListSlots();
        }

        // ���� ���̾� ���� ���� RectTransform ����
        for(int i = 0; i<_roomFurnitures.Length; i++)
        {
            for(int j = 0; j < _roomFurnitures[i]._furnitures.Length; j++)
            {
                _roomFurnitures[i].FurnituresRectTransforms[j] = _roomFurnitures[i]._furnitures[j].gameObject.GetComponent<RectTransform>();
            }
        }
    }

    //list���� �ش� spawn ��ġ�� Instantiate�ϴ� �Լ�
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

    ///// <summary>
    ///// �濡 �ִ� ������ Furniture�� ���� </summary>
    //private void BindRoomFurniture()
    //{
    //    for(int i = 0; i < _roomFurnitures.Length; i++)
    //    {
    //        for(int j = 0; j < _roomFurnitures[i]._furnitures.Length; j++)
    //        {

    //        }
    //    }
    //}
    #endregion

    private void GetCurrentField()
    {
        Toggle selectedField = GetOnToggle(); //���õ� ���
        _currentField = GetFieldByTransform(selectedField);
        SetActiveContent();
    }

    //spawnPosition Ȱ��ȭ �Լ�
    private void SetActiveContent()
    {
        for (int i = 0; i < _spawnPoint.Length; i++) //content false�� ����
        {
            _spawnPoint[i].gameObject.SetActive(false);
        }
        if ((int)(object)_currentField >= 0)
        {
            GameObject activeContent = _spawnPoint[(int)(object)_currentField].gameObject;
            activeContent.SetActive(true);//���� ����� content�� setactive 
            _scrollrect.content = activeContent.GetComponent<RectTransform>(); //scrollview ���� 

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
            for (int j = 1; j < _maxCount[(int)_currentField]; j++) //���� player�� �κ��丮�� ����� ������ ����
            {
                if (j < _lists[(int)_currentField].Count)
                {
                    Transform prefab = _spawnPoint[(int)_currentField].GetChild(j).GetChild(0);
                    prefab.gameObject.SetActive(true); //���� ���� => Getchild�� ��������
                    prefab.GetComponent<Image>().sprite = _lists[(int)_currentField][j].Image;
                }
                else
                {
                    _spawnPoint[(int)_currentField].GetChild(j).GetChild(0).gameObject.SetActive(false);
                }
            }
        }
    }

    //Transform���� Field�� ã��
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
    /// ���� ���� ��ư </summary>
    private void FurnitureImageBtnClick(int index)
    {
        if(index < 1)
        {
            _furnitureViewModel.RemoveFurniture(_currentField, _currnetRoom);
            return;
        }

        // ���� �ʵ��� index�� ���� ���� ã�� ����
        _furnitureViewModel.ChangedFurniture(DatabaseManager.Instance.GetFurnitureItem()[_lists[(int)_currentField][index].Id], _currnetRoom);
    }

    /// <summary>
    /// ��ġ�� ���� ID�� ����� ��� ���� </summary>
    private void UpdateFurnitureID(Furniture furnitureData)
    {
        if (furnitureData != null)
        {
            _roomFurnitures[(int)_currnetRoom].FurnituresRectTransforms[(int)furnitureData.Type].SetSiblingIndex(furnitureData.Layer);
            _roomFurnitures[(int)_currnetRoom]._furnitures[(int)furnitureData.Type].gameObject.SetActive(true);
            _roomFurnitures[(int)_currnetRoom]._furnitures[(int)furnitureData.Type].sprite = furnitureData.RoomImage;
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
    /// �� ���� �� ȣ�� </summary>
    private void ChangeRoom(bool isLeft)
    {
        _dontTouchArea.SetActive(true);

        CloseDoor(1.5f, () =>
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

            CloseDoor(1f, () =>
            {
                OpenDoor(1.5f);
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


    public void CloseDoor(float duration, Action onCompleted = null)
    {
        _dontTouchArea.SetActive(true);
        Tween.RectTransfromAnchoredPosition(_leftDoor.gameObject, _leftDoorTmpPos, duration, _doorTweenMode);
        Tween.RectTransfromAnchoredPosition(_rightDoor.gameObject, _rightDoorTmpPos, duration, _doorTweenMode, () =>
        {
            _dontTouchArea.SetActive(false);
            onCompleted?.Invoke();
        });
    }


    public void OpenDoor(float duration, Action onCompleted = null)
    {
        _dontTouchArea.SetActive(true);
        Tween.RectTransfromAnchoredPosition(_leftDoor.gameObject, _leftDoorTargetPos, duration, _doorTweenMode);
        Tween.RectTransfromAnchoredPosition(_rightDoor.gameObject, _rightDoorTargetPos, duration, _doorTweenMode, () =>
        {
            _dontTouchArea.SetActive(false);
            onCompleted?.Invoke();
        });
    }


    [System.Serializable]
    public class RoomFurniture
    {
        public Image[] _furnitures;
        public RectTransform[] FurnituresRectTransforms;

        public RoomFurniture()
        {
            _furnitures = new Image[System.Enum.GetValues(typeof(FurnitureType)).Length - 1];
            FurnituresRectTransforms = new RectTransform[System.Enum.GetValues(typeof(FurnitureType)).Length - 1];
        }
    }

}