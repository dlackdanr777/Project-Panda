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
    [SerializeField] private GameObject _detailView; // �󼼼��� â
    [SerializeField] private Button _closeButton; // �󼼼��� â �ݱ� ��ư

    // �켱 ������ ����
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

        // �� ����
        Tween.RectTransfromAnchoredPosition(gameObject, new Vector2(0, -650), 1.5f, TweenMode.EaseInOutBack);
        //Tween.RectTransfromAnchoredPosition(_leftDoor, new Vector2(390, 0), 1f, TweenMode.Quadratic);
        //Tween.RectTransfromAnchoredPosition(_rightDoor, new Vector2(10, 0), 1f, TweenMode.Quadratic);

        // ���� ���� �������� - ������ ����
        _furnitureImagePfs = new GameObject[DatabaseManager.Instance.StartPandaInfo.CostumeInventory.Count + 1];
        _furnitureImageBtn = new Button[DatabaseManager.Instance.StartPandaInfo.CostumeInventory.Count + 1];

        // ���� + 1��ŭ �ڽ�Ƭ ������ ����(0��°�� None)
        for (int i = 0; i < DatabaseManager.Instance.StartPandaInfo.CostumeInventory.Count + 1; i++) // ���� �κ��丮 ������ ��
        {
            int index = i;
            _furnitureImagePfs[i] = Instantiate(_furnitureSlot, _wallPaperImages.transform).transform.GetChild(0).gameObject; // �ϴ� �� ������ ���� - ��ųʸ� ���� ����.. A, B, C..
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
        //DataBind.SetButtonValue("ExitCostumeButton", OnExitButtonClicked); // ����

    }

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
        if (DatabaseManager.Instance.StartPandaInfo.WearingHeadCostumeID != "")
        {
            _furnitureViewModel.ChangedFurniture(CostumeManager.Instance.GetCostumeData(DatabaseManager.Instance.StartPandaInfo.WearingHeadCostumeID));
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
        if(index == 0)
        {
            _furnitureViewModel.RemoveFurniture();
            return;
        }
        _furnitureViewModel.ChangedFurniture(DatabaseManager.Instance.StartPandaInfo.CostumeInventory[index - 1]);
    }

    /// <summary>
    /// �԰��ִ� �ڽ�Ƭ ID�� ����� ��� ���� </summary>
    private void UpdateFurnitureID(CostumeData costumeData)
    {
        // ���� ������ ���� ���
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