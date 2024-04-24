using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Muks.Tween;
using TMPro;
using BT;

public class SaveFurnitureView : MonoBehaviour
{
    [SerializeField] private FurnitureView _furnitureView;
    [SerializeField] private GameObject _saveFurniture;
    [SerializeField] private TextMeshProUGUI _saveText;
    [SerializeField] private Button _yesButton;
    [SerializeField] private Button _noButton;
    [SerializeField] private Button _notExitButton;
    //[SerializeField] private Button _exitButton;
    //[SerializeField] private Sprite _exitDoorCloseImage;

    private FurnitureViewModel _furnitureViewModel;

    private float _loadTime = 0.05f; // 창 띄우고 끄는데 걸리는 시간

    // CostumeView 이후에 실행돼야 함
    void Start()
    {
        Invoke("Init", 1f);
    }

    private void OnDestroy()
    {
        _furnitureViewModel.FurnitureSceneChanged -= SaveCostume;
    }

    private void Init()
    {
        _yesButton.onClick.AddListener(OnSaveYesButtonClicked);
        _noButton.onClick.AddListener(OnSaveNoButtonClicked);
        _notExitButton.onClick.AddListener(NotExitButtonClicked);

        Bind();
    }

    private void Bind()
    {
        _furnitureViewModel = DatabaseManager.Instance.StartPandaInfo.FurnitureViewModel;
        if (_furnitureViewModel != null)
        {
            _furnitureViewModel.FurnitureSceneChanged += SaveCostume;
        }

    }

    private void SaveCostume(bool IsExit)
    {
        _saveFurniture.gameObject.SetActive(true);

        Tween.IamgeAlpha(_saveFurniture, 1, _loadTime, TweenMode.Quadratic);
        Tween.IamgeAlpha(_yesButton.gameObject, 1, _loadTime, TweenMode.Quadratic);
        Tween.IamgeAlpha(_noButton.gameObject, 1, _loadTime, TweenMode.Quadratic);
        //Tween.TextAlpha(_saveText.gameObject, 1, _loadTime, TweenMode.Quadratic);
    }

    private void OnSaveYesButtonClicked()
    {
        gameObject.SetActive(false);
        _furnitureView.CloseDoor(1.5f, () =>
        {
            StarterPanda.Instance.gameObject.SetActive(true);
            StarterPanda.Instance.IsSwitchingScene = true;
            _furnitureViewModel.SaveFurniture();
            SceneManager.LoadScene("FurnitureTestMainScene"); // 나중에 메인 씬으로 변경
        });

    }
    private void OnSaveNoButtonClicked()
    {
        StarterPanda.Instance.gameObject.SetActive(true);
        StarterPanda.Instance.IsSwitchingScene = true;
        SceneManager.LoadScene("FurnitureTestMainScene"); // 나중에 메인 씬으로 변경
    }

    private void NotExitButtonClicked()
    {
        _furnitureViewModel.IsExitFurniture = false;
        //_exitButton.GetComponent<Image>().sprite = _exitDoorCloseImage;

        Tween.IamgeAlpha(_saveFurniture, 0, _loadTime, TweenMode.Quadratic, () =>
        {
            _saveFurniture.gameObject.SetActive(false);
        });
        Tween.IamgeAlpha(_yesButton.gameObject, 0, _loadTime, TweenMode.Quadratic);
        Tween.IamgeAlpha(_noButton.gameObject, 0, _loadTime, TweenMode.Quadratic);
        //Tween.TextAlpha(_saveText.gameObject, 0, _loadTime, TweenMode.Quadratic);

    }

}
