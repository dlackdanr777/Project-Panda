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

    // 우선 머리 코스튬만 넣음
    [SerializeField] private GameObject _headCostumeImages; 
    [SerializeField] private GameObject _costumeImagePf;
    private GameObject[] _costumeImagePfs;
    private Button[] _costumeImageBtn;
    private bool _isStart;

    // 나중에 데이터베이스로 변경
    private StarterPandaInfo _startPandaInfo; 

    private void Start()
    {
        _isStart = true;
        Debug.Log("start");
        Init();
    }

    private void OnEnable()
    {
        if (_isStart)
        {
            EnterCostumeRoom();
        }
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

        DataBind.SetButtonValue("ExitCostumeButton", OnExitButtonClicked);

        Bind();
    }

    private void EnterCostumeRoom()
    {
        Tween.RectTransfromAnchoredPosition(gameObject, new Vector2(0, -650), 1.5f, TweenMode.EaseInOutBack);
        Tween.RectTransfromAnchoredPosition(_leftCurtain, new Vector2(-600, 0), 1.5f, TweenMode.Quadratic);
        Tween.RectTransfromAnchoredPosition(_rightCurtain, new Vector2(600, 0), 1.5f, TweenMode.Quadratic);

        _costumeImageBtn = _headCostumeImages.GetComponentsInChildren<Button>();
        for(int i = 0; i < CostumeManager.Instance.CostumeDic.Count; i++)
        {
            int index = i;
            _costumeImagePfs[i] = _costumeImageBtn[i].gameObject;
            if (_costumeImageBtn[i] != null)
            {
                _costumeImageBtn[i].onClick.AddListener(() => this.CostumeImageBtnClick(index));
                Debug.Log(i);
            }
        }

        _costumeViewModel = _startPandaInfo.CostumeViewModelInfo;
    }

    private void Bind()
    {
        _costumeViewModel = new CostumeViewModel();
        _costumeViewModel.CostumeChanged += UpdateCostumeID;
        
    }

    private void CostumeImageBtnClick(int index)
    {
        Debug.Log("index:" + index);
        Debug.Log("custume: " + CostumeManager.Instance.GetCostumeData(index).CostumeName);
        _costumeViewModel.WearingCostume(CostumeManager.Instance.GetCostumeData(index));
    }

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
        SceneManager.LoadScene("CostumeTestMainScene"); // 나중에 메인 씬으로 변경
    }
}
