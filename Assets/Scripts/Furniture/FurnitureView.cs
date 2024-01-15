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
    [SerializeField] private Image[] _furnitures; // ���� ���� ��ġ
    [SerializeField] private GameObject _leftDoor;
    [SerializeField] private GameObject _rightDoor;
    
    [SerializeField] private Button _exitButton;
    [SerializeField] private GameObject _uiSave;

    [SerializeField] private GameObject _detailView; // �󼼼��� â
    [SerializeField] private Button _closeDetailViewButton; // �󼼼��� â �ݱ� ��ư

    [SerializeField] private int _firstToggleIndex;
    [SerializeField] protected ToggleGroup _field; //��� ����

    [SerializeField] protected Transform[] _spawnPoint; //spawn�� ��ġ
    [SerializeField] private ScrollRect _scrollrect;


    [SerializeField] private GameObject _furnitureSlot;
    //private GameObject[] _furnitureImagePfs;
    //private Button[] _furnitureImageBtn;


    private int _currentItemIndex;

    protected List<Furniture>[] _lists = new List<Furniture>[System.Enum.GetValues(typeof(EFurnitureViewType)).Length - 1]; //Field ������ŭ ����Ʈ ����(None ����) //�����͸� ������ ����
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
    }

    private void OnDestroy()
    {
        _furnitureViewModel.FurnitureChanged -= UpdateFurnitureID;
    }

    private void Init()
    {
        DatabaseManager.Instance.StartPandaInfo.StarterPanda.gameObject.SetActive(false);

        // �� ����
        Tween.RectTransfromAnchoredPosition(gameObject, new Vector2(0, -650), 1.5f, TweenMode.EaseInOutBack);

        CreateSlots(); //slot ����

        //��ư ������
        foreach (Toggle toggle in _field.GetComponentsInChildren<Toggle>())
        {
            toggle.onValueChanged.AddListener((bool isOn) => OnClickedFieldButton(isOn, toggle.transform));
        }

        if (_detailView != null)
        {
            _closeDetailViewButton.onClick.AddListener(() => _detailView.SetActive(false));

        }
    }

    //list���� �ش� spawn ��ġ�� Instantiate�ϴ� �Լ�
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

    //private void Init()
    //{
    //    DatabaseManager.Instance.StartPandaInfo.StarterPanda.gameObject.SetActive(false);
    //    _contents = _contentsParent.GetComponentsInChildren<Transform>().Select(t => t.gameObject).ToArray();
    //    Debug.Log("_contents" + _contents);

    //    // �� ����
    //    Tween.RectTransfromAnchoredPosition(gameObject, new Vector2(0, -650), 1.5f, TweenMode.EaseInOutBack);
    //    //Tween.RectTransfromAnchoredPosition(_leftDoor, new Vector2(390, 0), 1f, TweenMode.Quadratic);
    //    //Tween.RectTransfromAnchoredPosition(_rightDoor, new Vector2(10, 0), 1f, TweenMode.Quadratic);

    //    // ���� ���� �������� - ������ ����
    //    _furnitureImagePfs = new GameObject[DatabaseManager.Instance.StartPandaInfo.FurnitureInventory.Count + _contents.Length];
    //    _furnitureImageBtn = new Button[DatabaseManager.Instance.StartPandaInfo.FurnitureInventory.Count + _contents.Length];

    //    // ���� + �޴� ������ŭ �ڽ�Ƭ ������ ����(�޴��� 0��°�� ����ξ�� �ϹǷ�)
    //    for (int i = 0; i < DatabaseManager.Instance.StartPandaInfo.FurnitureInventory.Count + _contents.Length; i++) // ���� �κ��丮 ������ ��
    //    {
    //        int index = i;
    //        Debug.Log("(int)DatabaseManager.Instance.StartPandaInfo.FurnitureInventory[i].ViewType" + (int)DatabaseManager.Instance.StartPandaInfo.FurnitureInventory[i].ViewType);
    //        _furnitureImagePfs[i] = Instantiate(_furnitureSlot, _contents[(int)DatabaseManager.Instance.StartPandaInfo.FurnitureInventory[i].ViewType].transform).transform.GetChild(0).gameObject; // �ϴ� �� ������ ���� - ��ųʸ� ���� ����.. A, B, C..
    //        if (i < _contents.Length) // ó�� 5���� ��ĭ
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
    //    //DataBind.SetButtonValue("ExitCostumeButton", OnExitButtonClicked); // ����

    //}

    /// <summary>
    /// �ڽ�Ƭ ��𵨰� ���ε� </summary>
    private void Bind()
    {
        if (_furnitureViewModel == null)
        {
            _furnitureViewModel = new FurnitureViewModel();
            DatabaseManager.Instance.StartPandaInfo.FurnitureViewModel = _furnitureViewModel;
            _furnitureViewModel.FurnitureChanged += UpdateFurnitureID;
            _furnitureViewModel.ShowDetailView += ShowDetailView;
        }

        // ����ִ°� �ƴ϶�� �ҷ��� �� ���� ���� �̸� ��ġ
        if (DatabaseManager.Instance.StartPandaInfo.WearingHeadCostumeID != "")
        {
            _furnitureViewModel.ChangedFurniture(DatabaseManager.Instance.GetFurnitureItem()[DatabaseManager.Instance.StartPandaInfo.WallPaperID]);
        }
    }

    /// <summary>
    /// �ڽ�Ƭ ���� ��ư </summary>
    private void FurnitureImageBtnClick(int index)
    {
        //if (CostumeManager.Instance.GetCostumeData(index).IsMine)
        //{
        //    _costumeViewModel.WearingCostume(CostumeManager.Instance.GetCostumeData(index));
        //}

        //_costumeViewModel.WearingCostume(CostumeManager.Instance.GetCostumeData(index));

        if(index < 1)
        {
            Debug.Log("���� ����" + index);
            _furnitureViewModel.RemoveFurniture(_currentField);
            return;
        }

        // ���� �ʵ��� index�� ���� ���� ã�� ����
        _furnitureViewModel.ChangedFurniture(DatabaseManager.Instance.GetFurnitureItem()[_lists[(int)_currentField][index].Id]);
        //_furnitureViewModel.ChangedFurniture(DatabaseManager.Instance.StartPandaInfo.FurnitureInventory[index - 1]);
    }

    /// <summary>
    /// �԰��ִ� �ڽ�Ƭ ID�� ����� ��� ���� </summary>
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